using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

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

    public async Task<List<CustomerDbM>> ReadAllCustomersAsync()
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .ToListAsync();
    }
}



