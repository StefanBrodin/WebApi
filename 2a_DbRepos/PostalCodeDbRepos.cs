using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

namespace DbRepos;

public class PostalCodeDbRepos
{
    private readonly ILogger<PostalCodeDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public PostalCodeDbRepos(ILogger<PostalCodeDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<PostalCodeDbM>> ReadAllPostalCodesAsync()
    {
        return await _dbContext.PostalCodes
            .AsNoTracking()
            .ToListAsync();
    }
}