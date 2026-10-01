using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class AttractionDbRepos
{
    private readonly ILogger<AttractionDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionDbRepos(ILogger<AttractionDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

// The initial rudimentary raw-read/dump of everything in the table, for testing purposes
    public async Task<List<AttractionDbM>> ReadAllAttractionsAsync()
    {
        return await _dbContext.Attractions
            .AsNoTracking()
            .ToListAsync();
    }

    // Read single item with optional flat or full details
    public async Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat)
    {
        IAttraction item;
        if (!flat)
        {
            var query = _dbContext.Attractions.AsNoTracking()
                .Include(i => i.AddressDbM)
                    .ThenInclude(a => a.PostalCodeDbM)
                    .ThenInclude(p => p.CityDbM)
                    .ThenInclude(c => c.CountryDbM)
                .Include(i => i.AttractionCategoriesDbM)
                    .ThenInclude(ac => ac.CategoryDbM)
                .Include(i => i.CustomerAttractionRatingsDbM)
                    .ThenInclude(r => r.CustomerDbM)
                .Where(i => i.AttractionId == id);

            item = await query.FirstOrDefaultAsync<IAttraction>();
        }
        else
        {
            var query = _dbContext.Attractions.AsNoTracking()
                .Where(i => i.AttractionId == id);

            item = await query.FirstOrDefaultAsync<IAttraction>();
        }

        if (item == null) throw new ArgumentException($"Attraction {id} does not exist");

        return new ResponseItemDto<IAttraction>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            Item = item
        };
    }

    // Read paginated list with filter 
    public async Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        filter = filter.ToLower().Trim();

        IQueryable<AttractionDbM> query;
        if (flat)
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(i => i.AddressDbM)
                    .ThenInclude(a => a.PostalCodeDbM)
                    .ThenInclude(p => p.CityDbM)
                    .ThenInclude(c => c.CountryDbM)
                .Include(i => i.AttractionCategoriesDbM)
                    .ThenInclude(ac => ac.CategoryDbM);
        }
        else
        {
            query = _dbContext.Attractions.AsNoTracking()
                .Include(i => i.AddressDbM)
                    .ThenInclude(a => a.PostalCodeDbM)
                    .ThenInclude(p => p.CityDbM)
                    .ThenInclude(c => c.CountryDbM)
                .Include(i => i.AttractionCategoriesDbM)
                    .ThenInclude(ac => ac.CategoryDbM)
                .Include(i => i.CustomerAttractionRatingsDbM);
        }

        var ret = new ResponsePageDto<IAttraction>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            DbItemsCount = await query
            
                // Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            (string.IsNullOrEmpty(filter) ||
                             i.AttractionName.ToLower().Contains(filter) ||
                             i.AttractionDescription.ToLower().Contains(filter) ||
                             i.AddressDbM.PostalCodeDbM.CityDbM.CityName.ToLower().Contains(filter) ||
                             i.AddressDbM.PostalCodeDbM.CityDbM.CountryDbM.CountryName.ToLower().Contains(filter) ||
                             i.AttractionCategoriesDbM.Any(ac => ac.CategoryDbM.CategoryName.ToLower().Contains(filter)))).CountAsync(),

            PageItems = await query

                // Adding filter functionality
                .Where(i => (i.Seeded == seeded) &&
                            (string.IsNullOrEmpty(filter) ||
                             i.AttractionName.ToLower().Contains(filter) ||
                             i.AttractionDescription.ToLower().Contains(filter) ||
                             i.AddressDbM.PostalCodeDbM.CityDbM.CityName.ToLower().Contains(filter) ||
                             i.AddressDbM.PostalCodeDbM.CityDbM.CountryDbM.CountryName.ToLower().Contains(filter) ||
                             i.AttractionCategoriesDbM.Any(ac => ac.CategoryDbM.CategoryName.ToLower().Contains(filter))))
                
                // Adding paging
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                
                .ToListAsync<IAttraction>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };

        return ret;
    }


    // Read from SQL-view supusr.vw_Attractions_Without_Reviews
    public async Task<ResponsePageDto<AttractionWithoutReviewsDto>> ReadAttractionsWithoutReviewsAsync(
        bool seeded, 
        string filter, 
        int pageNumber, 
        int pageSize)
    {
        filter ??= "";
        filter = filter.ToLower().Trim();

        var query = _dbContext.AttractionsWithoutReviewsView.AsNoTracking();

        var ret = new ResponsePageDto<AttractionWithoutReviewsDto>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            DbItemsCount = await query
                .Where(i => (i.Seeded == seeded) &&
                            (string.IsNullOrEmpty(filter) ||
                             i.AttractionName.ToLower().Contains(filter) ||
                             i.AttractionDescription.ToLower().Contains(filter) ||
                             i.CityName.ToLower().Contains(filter) ||
                             i.CountryName.ToLower().Contains(filter))).CountAsync(),

            PageItems = await query
                .Where(i => (i.Seeded == seeded) &&
                            (string.IsNullOrEmpty(filter) ||
                             i.AttractionName.ToLower().Contains(filter) ||
                             i.AttractionDescription.ToLower().Contains(filter) ||
                             i.CityName.ToLower().Contains(filter) ||
                             i.CountryName.ToLower().Contains(filter)))
                .OrderBy(i => i.AttractionName)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .ToListAsync(),

            PageNr = pageNumber,
            PageSize = pageSize
        };

        return ret;
    }

    #region Navigation Property Resolver
    // Helper method resolving navigation properties based on the CU-DTO
    private async Task navProp_AttractionCUdto_to_AttractionDbM(AttractionCuDto itemDtoSrc, AttractionDbM itemDst)
    {
        // 1. Resolve Address
        if (itemDtoSrc.AddressId != null && itemDtoSrc.AddressId != Guid.Empty)
        {
            var address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.AddressId == itemDtoSrc.AddressId);
            if (address == null)
                throw new ArgumentException($"Address id {itemDtoSrc.AddressId} does not exist");

            itemDst.AddressDbM = address;
            itemDst.AddressId = address.AddressId;
        }

        // 2. Resolve many-to-many relationship with Categories
        if (itemDtoSrc.CategoryIds != null)
        {
            var categories = new List<AttractionCategoryDbM>();
            foreach (var catId in itemDtoSrc.CategoryIds)
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == catId);
                if (category == null)
                    throw new ArgumentException($"Category id {catId} does not exist");

                categories.Add(new AttractionCategoryDbM
                {
                    AttractionId = itemDst.AttractionId,
                    CategoryId = catId,
                    CategoryDbM = category,
                    Seeded = false
                });
            }
            itemDst.AttractionCategoriesDbM = categories;
        }
    }
    #endregion

    // Create a new Attraction
    public async Task<ResponseItemDto<IAttraction>> CreateAttractionAsync(AttractionCuDto itemDto)
    {
        if (itemDto.AttractionId != null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be null when creating a new object");

        var item = new AttractionDbM(itemDto);

        // Connect navigation properties (Address and Categories)
        await navProp_AttractionCUdto_to_AttractionDbM(itemDto, item);

        _dbContext.Attractions.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadAttractionAsync(item.AttractionId, false);
    }

    // Update an existing Attraction
    public async Task<ResponseItemDto<IAttraction>> UpdateAttractionAsync(AttractionCuDto itemDto)
    {
        if (itemDto.AttractionId == null)
            throw new ArgumentException($"{nameof(itemDto.AttractionId)} must be specified when updating an object");

        // Find the instance with matching id and load related navigation properties
        var item = await _dbContext.Attractions
            .Include(i => i.AddressDbM)
            .Include(i => i.AttractionCategoriesDbM)
            .FirstOrDefaultAsync(i => i.AttractionId == itemDto.AttractionId);

        if (item == null)
            throw new ArgumentException($"Attraction {itemDto.AttractionId} does not exist");

        // Update scalar properties
        item.UpdateFromDTO(itemDto);

        // Update navigation properties
        await navProp_AttractionCUdto_to_AttractionDbM(itemDto, item);

        // Write to database model
        _dbContext.Attractions.Update(item);

        // Write to database in a Unit of Work
        await _dbContext.SaveChangesAsync();

        return await ReadAttractionAsync(item.AttractionId, false);
    }

    // Delete Attraction (Cascade delete automatically removes related ratings and category connections)
    public async Task<ResponseItemDto<IAttraction>> DeleteAttractionAsync(Guid id)
    {
        var item = await _dbContext.Attractions
            .Include(a => a.AttractionCategoriesDbM)
            .Include(a => a.CustomerAttractionRatingsDbM)
            .FirstOrDefaultAsync(a => a.AttractionId == id);

        if (item == null)
            throw new ArgumentException($"Attraction {id} does not exist");

        _dbContext.Attractions.Remove(item);
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<IAttraction>
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            Item = item
        };
    }

}

