using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;

namespace StoreNet.Infrastructure.Layer.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Checks if any customer exists based on a provided predicate
    public async Task<bool> AnyAsync(Func<Customer, bool> predicate)
    {
        return await _context.Customers
                             .AsNoTracking() // Improves performance by not tracking the entity
                             .AnyAsync(c => predicate(c)); // Directly uses AnyAsync to check existence
    }

    // Adds a new customer to the database
    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer); // Adds the customer to the DbSet
        await _context.SaveChangesAsync(); // Saves changes to the database after adding
    }

    // Retrieves a customer by email (case-insensitive)
    public async Task<Customer> GetByEmailAsync(string email)
    {
        var customer = await _context.Customers
            .AsNoTracking() // No need to track the entity as it's a read-only operation
            .FirstOrDefaultAsync(c => c.Email == email);

        return customer;
    }

    // Retrieves an active customer by their ID
    public async Task<Customer> GetByIdAsync(Guid id)
    {
        var customer = await _context.Customers
            .AsNoTracking() // No need to track the entity for this read-only operation
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive); // Checks if the customer exists and is active

        if (customer is null)
        {
            throw new KeyNotFoundException($"No active customer found with ID {id}."); // Throws an exception if not found
        }

        return customer;
    }
    // Updates an existing customer in the database
    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer); // Marks the customer entity as updated
        await _context.SaveChangesAsync(); // Saves the changes to the database
    }

    // Soft deletes a customer by marking them as inactive
    public async Task DeleteAsync(Customer customer)
    {
        customer.IsActive = false; // Marks the customer as inactive (soft delete)
        _context.Customers.Update(customer); // Marks the entity as updated
        await _context.SaveChangesAsync(); // Saves the changes to the database
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.Include(c => c.Addresses).ToListAsync();
    }
}