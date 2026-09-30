using DbModels;

namespace Services;

public interface ICategoryService
{
    public Task<List<CategoryDbM>> ReadAllCategoriesAsync();
}

