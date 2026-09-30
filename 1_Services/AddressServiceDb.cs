using DbModels;
using DbRepos;

namespace Services;

public class AddressServiceDb : IAddressService
{
    private readonly AddressDbRepos _repo;

    public AddressServiceDb(AddressDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<AddressDbM>> ReadAllAddressesAsync()
    {
        return await _repo.ReadAllAddressesAsync();
    }
}

