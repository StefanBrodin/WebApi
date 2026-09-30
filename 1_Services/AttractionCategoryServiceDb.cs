using DbModels;
using DbRepos;

namespace Services;

public class AttractionCategoryServiceDb : IAttractionCategoryService
{
    private readonly AttractionCategoryDbRepos _repo;

    public AttractionCategoryServiceDb(AttractionCategoryDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<AttractionCategoryDbM>> ReadAllAttractionCategoriesAsync()
    {
        return await _repo.ReadAllAttractionCategoriesAsync();
    }
}




