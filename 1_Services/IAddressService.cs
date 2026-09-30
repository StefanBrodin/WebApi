using DbModels;

namespace Services;

public interface IAddressService
{
    public Task<List<AddressDbM>> ReadAllAddressesAsync();
}

