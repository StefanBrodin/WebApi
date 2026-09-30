using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

namespace DbRepos;

public class CategoryDbRepos
{
    private readonly ILogger<CategoryDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public CategoryDbRepos(ILogger<CategoryDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<CategoryDbM>> ReadAllCategoriesAsync()
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .ToListAsync();
    }
}

