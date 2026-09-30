using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

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

    public async Task<List<CustomerAttractionRatingDbM>> ReadAllRatingsAsync()
    {
        return await _dbContext.CustomerAttractionRatings
            .AsNoTracking()
            .ToListAsync();
    }
}



