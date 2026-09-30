using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

namespace DbRepos;

public class AddressDbRepos
{
    private readonly ILogger<AddressDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AddressDbRepos(ILogger<AddressDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<AddressDbM>> ReadAllAddressesAsync()
    {
        return await _dbContext.Addresses
            .AsNoTracking()
            .ToListAsync();
    }
}

