using DbModels;
using DbRepos;

namespace Services;

public class CountryServiceDb : ICountryService
{
    private readonly CountryDbRepos _repo;

    public CountryServiceDb(CountryDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<CountryDbM>> ReadAllCountriesAsync()
    {
        return await _repo.ReadAllCountriesAsync();
    }
}