using DbModels;

namespace Services;

public interface ICityService
{
    public Task<List<CityDbM>> ReadAllCitiesAsync();
}