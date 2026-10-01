using Models;
using Models.DTO;
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

    public async Task<ResponseItemDto<ICustomer>> ReadCustomerAsync(Guid id, bool flat)
    {
        return await _repo.ReadCustomerAsync(id, flat);
    }

    public async Task<ResponsePageDto<ICustomer>> ReadCustomersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return await _repo.ReadCustomersAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public async Task<ResponseItemDto<ICustomer>> CreateCustomerAsync(CustomerCuDto itemDto)
    => await _repo.CreateCustomerAsync(itemDto);

    public async Task<ResponseItemDto<ICustomer>> DeleteCustomerAsync(Guid id)
    => await _repo.DeleteCustomerAsync(id);

}


