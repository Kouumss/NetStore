using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;

namespace StoreNet.Infrastructure.Layer.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Méthode pour récupérer l'adresse en fonction du type
        public async Task<Address?> GetAddressByTypeAsync(string customerId, AddressType addressType)
        {
            return await _context.Addresses
                .Where(a => a.CustomerId == customerId && a.AddressType == addressType)
                .FirstOrDefaultAsync();
        }
        public async Task<Address?> GetByIdAsync(string id)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Address>> GetByCustomerIdAsync(string customerId)
        {
            return await _context.Addresses
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Address address)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}
