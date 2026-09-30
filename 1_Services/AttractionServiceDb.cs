using Models;
using Models.DTO;
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

    public async Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat)
    {
        return await _repo.ReadAttractionAsync(id, flat);
    }

    public async Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return await _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
    }
}




