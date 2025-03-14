using System.Linq.Expressions;
using StoreNet.Domain.Layer.Entities;


namespace StoreNet.Domain.Layer.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task AddAsync(Customer customer);
    Task<bool> AnyAsync(Func<Customer, bool> predicate);
    Task<Customer> GetByEmailAsync(string email);
    Task<Customer> GetByIdAsync(Guid id);
    Task UpdateAsync(Customer customer);
}

