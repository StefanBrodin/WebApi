using DbModels;
using DbRepos;

namespace Services;

public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _repo;

    public AttractionServiceDb(AttractionDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<AttractionDbM>> ReadAllAttractionsAsync()
    {
        return await _repo.ReadAllAttractionsAsync();
    }
}



