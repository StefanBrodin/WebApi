namespace Services;
using Models.DTO;

public interface IAdminService
{
    public Task SeedAsync(int nrItems);

    public Task<ResponseItemDto<DatabaseOverviewDto>> GetDatabaseOverviewAsync();

    public Task<ResponseItemDto<DatabaseOverviewDto>> RemoveSeedAsync(bool seeded);

}


