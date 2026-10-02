using DbModels;

using Models;
using Models.DTO;


namespace Services;

public interface ICustomerAttractionRatingService
{
    public Task<List<CustomerAttractionRatingDbM>> ReadAllRatingsAsync();
    
    public Task<ResponseItemDto<ICustomerAttractionRating>> ReadRatingAsync(Guid customerId, Guid attractionId);
    
    public Task<ResponseItemDto<ICustomerAttractionRating>> CreateRatingAsync(CustomerAttractionRatingCuDto itemDto);

    public Task<ResponseItemDto<ICustomerAttractionRating>> DeleteRatingAsync(Guid customerId, Guid attractionId);

}

