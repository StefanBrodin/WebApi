using DbModels;

namespace Services;

public interface IAttractionService
{
    public Task<List<AttractionDbM>> ReadAllAttractionsAsync();
}



