using Models;
using Models.DTO;
using DbModels;

namespace Services;

public interface ICustomerService
{
    public Task<List<CustomerDbM>> ReadAllCustomersAsync();

    public Task<ResponseItemDto<ICustomer>> ReadCustomerAsync(Guid id, bool flat);

    public Task<ResponsePageDto<ICustomer>> ReadCustomersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

    public Task<ResponseItemDto<ICustomer>> CreateCustomerAsync(CustomerCuDto itemDto);

    public Task<ResponseItemDto<ICustomer>> DeleteCustomerAsync(Guid id);

    
}


