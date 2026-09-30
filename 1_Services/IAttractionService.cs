using Models;
using Models.DTO;
using DbModels;

namespace Services;

public interface IAttractionService
{
    public Task<List<AttractionDbM>> ReadAllAttractionsAsync();
    
    public Task<ResponseItemDto<IAttraction>> ReadAttractionAsync(Guid id, bool flat);
    
    public Task<ResponsePageDto<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);


    // The view showing attractions without reviews 
    public Task<ResponsePageDto<AttractionWithoutReviewsDto>> ReadAttractionsWithoutReviewsAsync(bool seeded, string filter, int pageNumber, int pageSize);

}


