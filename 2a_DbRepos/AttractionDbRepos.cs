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

        if (item == null) throw new ArgumentException($"Item {id} is not existing");

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

}