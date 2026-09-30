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

        // Remove existing countries in the database in the right order to avoid foreign key constraint violations
        _dbContext.Cities.RemoveRange(_dbContext.Cities);
        _dbContext.Countries.RemoveRange(_dbContext.Countries);
        await _dbContext.SaveChangesAsync();

        // Seeding at least 4 unique countries into the database and saving them
        var countries = seeder.UniqueItemsToList<CountryDbM>(Math.Max(nrItems, 4));
        _dbContext.Countries.AddRange(countries);
        await _dbContext.SaveChangesAsync();

        // Create unique cities and assign them to random countries from the seeded countries
        // UniqueItemsToList ensures that the cities are unique. We want 100 cities. 
        var cities = seeder.ItemsToList<CityDbM>(100);

        // Keep track of the number of cities with the same name in each country to ensure uniqueness
        var cityCountInCountry = new Dictionary<(Guid, string), int>();

        foreach (var city in cities)
        {
            var randomCountry = seeder.FromList(countries);
            city.CountryId = randomCountry.CountryId;

            var key = (city.CountryId, city.CityName.ToLower().Trim());

            if (cityCountInCountry.ContainsKey(key))
            {
                cityCountInCountry[key]++;
                
                // If "Stockholm" already exists in Sweden, the next one will be "Stockholm 2", etc.
                city.CityName = $"{city.CityName} {cityCountInCountry[key]}";
            }
            else
            {
                cityCountInCountry[key] = 1;
            }
        }

        _dbContext.Cities.AddRange(cities);
        await _dbContext.SaveChangesAsync();
    }

}