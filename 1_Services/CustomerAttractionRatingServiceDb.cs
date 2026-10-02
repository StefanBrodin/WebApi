using DbModels;
using DbRepos;

using Models;
using Models.DTO;


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

    public async Task<ResponseItemDto<ICustomerAttractionRating>> ReadRatingAsync(Guid customerId, Guid attractionId)
    {
        return await _repo.ReadRatingAsync(customerId, attractionId);
    }

    public async Task<ResponseItemDto<ICustomerAttractionRating>> CreateRatingAsync(CustomerAttractionRatingCuDto itemDto)
    {
        return await _repo.CreateRatingAsync(itemDto);
    }

    public async Task<ResponseItemDto<ICustomerAttractionRating>> DeleteRatingAsync(Guid customerId, Guid attractionId)
    {
        return await _repo.DeleteRatingAsync(customerId, attractionId);
    }


}
