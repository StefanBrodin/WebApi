using DbModels;

namespace Services;

public interface IAttractionCategoryService
{
    public Task<List<AttractionCategoryDbM>> ReadAllAttractionCategoriesAsync();
}

