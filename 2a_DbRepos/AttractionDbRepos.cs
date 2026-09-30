using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

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

    public async Task<List<AttractionDbM>> ReadAllAttractionsAsync()
    {
        return await _dbContext.Attractions
            .AsNoTracking()
            .ToListAsync();
    }
}





