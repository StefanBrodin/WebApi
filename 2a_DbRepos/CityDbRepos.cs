using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

namespace DbRepos;

public class CityDbRepos
{
    private readonly ILogger<CityDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CityDbRepos(ILogger<CityDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<CityDbM>> ReadAllCitiesAsync()
    {
        return await _dbContext.Cities
            .AsNoTracking()
            .ToListAsync();
    }
}