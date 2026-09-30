using DbModels;
using DbRepos;

namespace Services;

public class CustomerAttractionRatingServiceDb : ICustomerAttractionRatingService
{
    private readonly CustomerAttractionRatingDbRepos _repo;

    public CustomerAttractionRatingServiceDb(CustomerAttractionRatingDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<CustomerAttractionRatingDbM>> ReadAllRatingsAsync()
    {
        return await _repo.ReadAllRatingsAsync();
    }
}




