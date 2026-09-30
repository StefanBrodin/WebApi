using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Models;
using Models.DTO;
using DbModels;
using DbContext;

namespace DbRepos;

public class CustomerDbRepos
{
    private readonly ILogger<CustomerDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CustomerDbRepos(ILogger<CustomerDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    // The initial rudimentary raw-read/dump of everything in the table, for testing purposes
    public async Task<List<CustomerDbM>> ReadAllCustomersAsync()
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    // Read single item with optional flat or full details
    public async Task<ResponseItemDto<ICustomer>> ReadCustomerAsync(Guid id, bool flat)
    {
        ICustomer item;
        if (!flat)
        {
            var query = _dbContext.Customers.AsNoTracking()
                .Include(i => i.AddressDbM)
                    .ThenInclude(a => a.PostalCodeDbM)
                    .ThenInclude(p => p.CityDbM)
                    .ThenInclude(c => c.CountryDbM)
                .Include(i => i.CustomerAttractionRatingsDbM)
                    .ThenInclude(r => r.AttractionDbM)
                .Where(i => i.CustomerId == id);

            item = await query.FirstOrDefaultAsync<ICustomer>();
        }
        else
        {
            var query = _dbContext.Customers.AsNoTracking()
                .Where(i => i.CustomerId == id);

            item = await query.FirstOrDefaultAsync<ICustomer>();
        }

        if (item == null) throw new ArgumentException($"Customer {id} does not exist");

        return new ResponseItemDto<ICustomer>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            Item = item
        };
    }

    // Read paginated list with filter on first name, last name and username
    public async Task<ResponsePageDto<ICustomer>> ReadCustomersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        filter ??= "";
        filter = filter.ToLower().Trim();

        IQueryable<CustomerDbM> query;
        if (flat)
        {
            query = _dbContext.Customers.AsNoTracking();
        }
        else
        {
            query = _dbContext.Customers.AsNoTracking()
                .Include(i => i.AddressDbM)
                    .ThenInclude(a => a.PostalCodeDbM)
                    .ThenInclude(p => p.CityDbM)
                    .ThenInclude(c => c.CountryDbM)
                .Include(i => i.CustomerAttractionRatingsDbM)
                    .ThenInclude(r => r.AttractionDbM);
        }

        var ret = new ResponsePageDto<ICustomer>()
        {
#if DEBUG
            ConnectionString = _dbContext.Database.GetConnectionString(),
#endif
            DbItemsCount = await query

            // Adding filter functionality
            .Where(i => (i.Seeded == seeded) &&
                        (string.IsNullOrEmpty(filter) ||
                         i.CustomerFirstName.ToLower().Contains(filter) ||
                         i.CustomerLastName.ToLower().Contains(filter) ||
                         i.CustomerUserName.ToLower().Contains(filter))).CountAsync(),

            PageItems = await query

            // Adding filter functionality
            .Where(i => (i.Seeded == seeded) &&
                        (string.IsNullOrEmpty(filter) ||
                         i.CustomerFirstName.ToLower().Contains(filter) ||
                         i.CustomerLastName.ToLower().Contains(filter) ||
                         i.CustomerUserName.ToLower().Contains(filter)))

            // Adding paging
            .Skip(pageNumber * pageSize)
            .Take(pageSize)

            .ToListAsync<ICustomer>(),

            PageNr = pageNumber,
            PageSize = pageSize
        };

        return ret;
    }
}


