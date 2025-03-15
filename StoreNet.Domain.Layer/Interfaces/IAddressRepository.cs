using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IAddressRepository
{
    Task<Address?> GetByIdAsync(string id);
    Task<List<Address>> GetByCustomerIdAsync(string customerId);
    Task AddAsync(Address address);
    Task UpdateAsync(Address address);
    Task DeleteAsync(Address address);
    Task<Address?> GetAddressByTypeAsync(string customerId, AddressType addressType);

}
