using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DbContext;
using DbModels;

namespace DbRepos;

public class AttractionCategoryDbRepos
{
    private readonly ILogger<AttractionCategoryDbRepos> _logger;
    private readonly MainDbContext _dbContext;

    public AttractionCategoryDbRepos(ILogger<AttractionCategoryDbRepos> logger, MainDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<AttractionCategoryDbM>> ReadAllAttractionCategoriesAsync()
    {
        return await _dbContext.AttractionCategories
            .AsNoTracking()
            .ToListAsync();
    }
}


