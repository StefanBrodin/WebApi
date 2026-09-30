using DbModels;
using DbRepos;

namespace Services;

public class CategoryServiceDb : ICategoryService
{
    private readonly CategoryDbRepos _repo;

    public CategoryServiceDb(CategoryDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoryDbM>> ReadAllCategoriesAsync()
    {
        return await _repo.ReadAllCategoriesAsync();
    }
}

