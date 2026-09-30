using DbModels;

namespace Services;

public interface ICustomerAttractionRatingService
{
    public Task<List<CustomerAttractionRatingDbM>> ReadAllRatingsAsync();
}


