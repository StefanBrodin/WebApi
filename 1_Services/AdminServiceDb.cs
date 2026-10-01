using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;

namespace Services;
    
public class AdminServiceDb : IAdminService
{
    private readonly AdminDbRepos _repo;
    private readonly ILogger<AdminServiceDb> _logger;

    public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);

    #region constructors
    public AdminServiceDb(AdminDbRepos repo)
    {
        _repo = repo;
    }
    public AdminServiceDb(AdminDbRepos repo, ILogger<AdminServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion

    public async Task<ResponseItemDto<DatabaseOverviewDto>> GetDatabaseOverviewAsync()
    {
        return await _repo.GetDatabaseOverviewAsync();
    }

    public async Task<ResponseItemDto<DatabaseOverviewDto>> RemoveSeedAsync(bool seeded)
    {
        return await _repo.RemoveSeedAsync(seeded);
    }

}