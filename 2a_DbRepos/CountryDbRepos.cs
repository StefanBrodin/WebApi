using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

namespace DbRepos;

public class CountryDbRepos
{
    private readonly ILogger<CountryDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CountryDbRepos(ILogger<CountryDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    // Retrieve all countries from the database asynchronously
    public async Task<List<CountryDbM>> ReadAllCountriesAsync()
    {
        return await _dbContext.Countries
            .AsNoTracking()
            .ToListAsync();
    }
}