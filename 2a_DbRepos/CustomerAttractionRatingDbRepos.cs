using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;
using Models;
using Models.DTO;

namespace DbRepos;

public class CustomerAttractionRatingDbRepos
{
    private readonly ILogger<CustomerAttractionRatingDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CustomerAttractionRatingDbRepos(ILogger<CustomerAttractionRatingDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    // The initial raw-read/dump of all ratings for testing purposes
    public async Task<List<CustomerAttractionRatingDbM>> ReadAllRatingsAsync()
    {
        return await _dbContext.CustomerAttractionRatings
            .AsNoTracking()
            .ToListAsync();
    }

    // Read single rating item by composite key
    public async Task<ResponseItemDto<ICustomerAttractionRating>> ReadRatingAsync(Guid customerId, Guid attractionId)
    {
        var query = _dbContext.CustomerAttractionRatings.AsNoTracking()
            .Include(r => r.CustomerDbM)
            .Include(r => r.AttractionDbM)
            .Where(r => r.CustomerId == customerId && r.AttractionId == attractionId);

        var item = await query.FirstOrDefaultAsync<ICustomerAttractionRating>();
        if (item == null) 
            throw new ArgumentException($"Rating for customer {customerId} and attraction {attractionId} does not exist");

        return new ResponseItemDto<ICustomerAttractionRating>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            Item = item
        };
    }

    #region Navigation Property Resolver
    // Helper method resolving customer and attraction foreign keys
    private async Task navProp_CustomerAttractionRatingCUdto_to_CustomerAttractionRatingDbM(
        CustomerAttractionRatingCuDto itemDtoSrc, CustomerAttractionRatingDbM itemDst)
    {
        // 1. Resolve Customer
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == itemDtoSrc.CustomerId);
        if (customer == null) 
            throw new ArgumentException($"Customer id {itemDtoSrc.CustomerId} does not exist");

        itemDst.CustomerDbM = customer;
        itemDst.CustomerId = customer.CustomerId;

        // 2. Resolve Attraction
        var attraction = await _dbContext.Attractions.FirstOrDefaultAsync(a => a.AttractionId == itemDtoSrc.AttractionId);
        if (attraction == null) 
            throw new ArgumentException($"Attraction id {itemDtoSrc.AttractionId} does not exist");

        itemDst.AttractionDbM = attraction;
        itemDst.AttractionId = attraction.AttractionId;
    }
    #endregion

    // Create a new rating/comment linked to customer and attraction
    public async Task<ResponseItemDto<ICustomerAttractionRating>> CreateRatingAsync(CustomerAttractionRatingCuDto itemDto)
    {
        // Verify that this customer hasn't already rated this attraction (composite key uniqueness)
        var existing = await _dbContext.CustomerAttractionRatings
            .FirstOrDefaultAsync(r => r.CustomerId == itemDto.CustomerId && r.AttractionId == itemDto.AttractionId);
            
        if (existing != null)
            throw new ArgumentException($"A rating by customer {itemDto.CustomerId} for attraction {itemDto.AttractionId} already exists");

        var item = new CustomerAttractionRatingDbM(itemDto);

        // Connect navigation properties
        await navProp_CustomerAttractionRatingCUdto_to_CustomerAttractionRatingDbM(itemDto, item);

        _dbContext.CustomerAttractionRatings.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadRatingAsync(item.CustomerId, item.AttractionId);
    }

    // Delete a specific rating/comment
    public async Task<ResponseItemDto<ICustomerAttractionRating>> DeleteRatingAsync(Guid customerId, Guid attractionId)
    {
        var item = await _dbContext.CustomerAttractionRatings
            .Include(r => r.CustomerDbM)
            .Include(r => r.AttractionDbM)
            .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.AttractionId == attractionId);

        if (item == null)
            throw new ArgumentException($"Rating for customer {customerId} and attraction {attractionId} does not exist");

        _dbContext.CustomerAttractionRatings.Remove(item);
        await _dbContext.SaveChangesAsync();

        return new ResponseItemDto<ICustomerAttractionRating>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            Item = item
        };

    }


}


