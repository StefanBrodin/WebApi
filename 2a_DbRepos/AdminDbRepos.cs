using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public AdminDbRepos(ILogger<AdminDbRepos> logger, Encryptions encryptions, MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }

    public async Task SeedAsync(int nrItems)
    {
        // Create a seeder
        var fn = Path.GetFullPath(_seedSource);
        var seeder = new SeedGenerator(fn);

        // Remove existing countries in the database
        _dbContext.Countries.RemoveRange(_dbContext.Countries);

        // Seeding at least 4 unique countries into the database
        var countries = seeder.UniqueItemsToList<CountryDbM>(Math.Max(nrItems, 4));
        _dbContext.Countries.AddRange(countries);

        // Save changes to the database
        await _dbContext.SaveChangesAsync();
    }
}