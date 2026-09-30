using DbModels;

namespace Services;

public interface IPostalCodeService
{
    public Task<List<PostalCodeDbM>> ReadAllPostalCodesAsync();
}