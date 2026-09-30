using DbModels;

namespace Services;

public interface ICountryService
{
    public Task<List<CountryDbM>> ReadAllCountriesAsync();
}