using DbModels;
using DbRepos;

namespace Services;

public class CityServiceDb : ICityService
{
    private readonly CityDbRepos _repo;

    public CityServiceDb(CityDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<CityDbM>> ReadAllCitiesAsync()
    {
        return await _repo.ReadAllCitiesAsync();
    }
}