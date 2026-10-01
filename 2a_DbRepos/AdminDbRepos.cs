using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;

using Models.DTO;

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
        _dbContext.CustomerAttractionRatings.RemoveRange(_dbContext.CustomerAttractionRatings);
        _dbContext.AttractionCategories.RemoveRange(_dbContext.AttractionCategories);
        _dbContext.Attractions.RemoveRange(_dbContext.Attractions);
        _dbContext.Customers.RemoveRange(_dbContext.Customers);
        _dbContext.Categories.RemoveRange(_dbContext.Categories);
        _dbContext.Addresses.RemoveRange(_dbContext.Addresses);
        _dbContext.PostalCodes.RemoveRange(_dbContext.PostalCodes);
        _dbContext.Cities.RemoveRange(_dbContext.Cities);
        _dbContext.Countries.RemoveRange(_dbContext.Countries);
        await _dbContext.SaveChangesAsync();

        // Seeding at least 4 unique countries into the database and saving them
        var countries = seeder.UniqueItemsToList<CountryDbM>(Math.Max(nrItems, 4));
        _dbContext.Countries.AddRange(countries);
        await _dbContext.SaveChangesAsync();

        // Seed 100 unique cities into the database, ensuring that each city has a unique name within its country
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

        // Seed 200 unique postal codes into the database, ensuring that each postal code is unique within its city
        var postalCodes = seeder.ItemsToList<PostalCodeDbM>(200);
        var postalCodeCountInCity = new Dictionary<(Guid, string), int>();

        foreach (var pc in postalCodes)
        {
            var randomCity = seeder.FromList(cities);
            pc.CityId = randomCity.CityId;

            var key = (pc.CityId, pc.PostalCodeNumber.Trim());
            if (postalCodeCountInCity.ContainsKey(key))
            {
                postalCodeCountInCity[key]++;

                // If there's a conflict in the same city, generate a unique 5-digit number
                pc.PostalCodeNumber = $"{seeder.Next(10000, 99999)}";
            }
            else
            {
                postalCodeCountInCity[key] = 1;
            }
        }
        _dbContext.PostalCodes.AddRange(postalCodes);
        await _dbContext.SaveChangesAsync();

        // Seed 1100 addresses and link them to postal codes (enough for 50 users and 1000 attractions, and then some)
        var addresses = seeder.ItemsToList<AddressDbM>(1100);
        foreach (var address in addresses)
        {
            var randomPostalCode = seeder.FromList(postalCodes);
            address.PostalCodeId = randomPostalCode.PostalCodeId;
        }
        _dbContext.Addresses.AddRange(addresses);
        await _dbContext.SaveChangesAsync();

        // Seed the attraction categories into the database
        var categories = seeder.UniqueItemsToList<CategoryDbM>(20);
        _dbContext.Categories.AddRange(categories);
        await _dbContext.SaveChangesAsync();

        // Seed 50 customers and link them to addresses (there are enough seeded addresses for 50 users)
        var customers = seeder.UniqueItemsToList<CustomerDbM>(50);
        foreach (var customer in customers)
        {
            var randomAddress = seeder.FromList(addresses);
            customer.AddressId = randomAddress.AddressId;
        }
        _dbContext.Customers.AddRange(customers);
        await _dbContext.SaveChangesAsync();

        // Seed 1000 attractions and link them to addresses (there are enough seeded addresses for 1000 attractions)
        var attractions = seeder.ItemsToList<AttractionDbM>(1000);
        foreach (var attraction in attractions)
        {
            var randomAddress = seeder.FromList(addresses);
            attraction.AddressId = randomAddress.AddressId;
        }
        _dbContext.Attractions.AddRange(attractions);
        await _dbContext.SaveChangesAsync();

        // Seed attraction categories for each attraction, so that each attraction has between 1 and 3 unique categories
        var attractionCategories = new List<AttractionCategoryDbM>();
        foreach (var attraction in attractions)
        {
            // Randomly pick between 1 and 3 unique categories for this attraction
            int nrOfCategories = seeder.Next(1, 4);
            var pickedCategories = seeder.UniqueItemsPickedFromList(nrOfCategories, categories);

            foreach (var category in pickedCategories)
            {
                attractionCategories.Add(new AttractionCategoryDbM
                {
                    AttractionId = attraction.AttractionId,
                    CategoryId = category.CategoryId,
                    Seeded = true
                });
            }
        }
        _dbContext.AttractionCategories.AddRange(attractionCategories);
        await _dbContext.SaveChangesAsync();

        // Seed customer attraction ratings (each attraction gets between 0 and 20 reviews)
        var ratings = new List<CustomerAttractionRatingDbM>();

        foreach (var attraction in attractions)
        {
            // Randomize the number of ratings *for this* attraction (between 0 and 20)
            int nrOfRatings = seeder.Next(0, 21);

            if (nrOfRatings > 0)
            {
                // Pick 'nrOfRatings' unique customers who have written these reviews
                var commentingCustomers = seeder.UniqueItemsPickedFromList(nrOfRatings, customers);

                foreach (var customer in commentingCustomers)
                {
                    ratings.Add(new CustomerAttractionRatingDbM
                    {
                        AttractionId = attraction.AttractionId,
                        CustomerId = customer.CustomerId,
                        RatingScore = (byte)seeder.Next(1, 6),
                        RatingReview = seeder.LatinSentence, // Garanterar en kommentar
                        Seeded = true
                    });
                }
            }
        }

        _dbContext.CustomerAttractionRatings.AddRange(ratings);
        await _dbContext.SaveChangesAsync();

        await ExecuteInitScriptAsync();

    }

    // Execute the SQL script to create the view in the database
    private async Task ExecuteInitScriptAsync()
    {
        // Path to the SQL script file, relative to the output directory of the application
        var scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlScripts", "sqlserver", "initDatabase.sql");
        if (File.Exists(scriptPath))
        {
            var sql = await File.ReadAllTextAsync(scriptPath);
            
            // SQL Server uses "GO" as a batch separator, so we need to split the script into individual commands
            var statements = sql.Split(new[] { "GO\r\n", "GO\n", "GO" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var statement in statements)
            {
                if (!string.IsNullOrWhiteSpace(statement))
                {
                    await _dbContext.Database.ExecuteSqlRawAsync(statement);
                }
            }
        }
    }

    public async Task<ResponseItemDto<DatabaseOverviewDto>> GetDatabaseOverviewAsync()
    {
        var overview = await _dbContext.DatabaseOverviewView.FirstAsync();
        return new ResponseItemDto<DatabaseOverviewDto>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            Item = overview
        };
    }

    public async Task<ResponseItemDto<DatabaseOverviewDto>> RemoveSeedAsync(bool seeded)
    {
        // Run the stored procedure to remove seeded data
        await _dbContext.Database.ExecuteSqlInterpolatedAsync($"EXEC supusr.sp_RemoveSeed @seeded = {seeded}");

        // Return the database overview after removing the seeded data
        return await GetDatabaseOverviewAsync();
    }



}




