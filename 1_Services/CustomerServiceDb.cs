using DbModels;
using DbRepos;

namespace Services;

public class CustomerServiceDb : ICustomerService
{
    private readonly CustomerDbRepos _repo;

    public CustomerServiceDb(CustomerDbRepos repo)
    {
        _repo = repo;
    }

    public async Task<List<CustomerDbM>> ReadAllCustomersAsync()
    {
        return await _repo.ReadAllCustomersAsync();
    }
}


