using DbModels;
using DbRepos;

namespace Services;

public class PostalCodeServiceDb : IPostalCodeService
{
    private readonly PostalCodeDbRepos _repo;

    public PostalCodeServiceDb(PostalCodeDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<PostalCodeDbM>> ReadAllPostalCodesAsync()
    {
        return await _repo.ReadAllPostalCodesAsync();
    }
}