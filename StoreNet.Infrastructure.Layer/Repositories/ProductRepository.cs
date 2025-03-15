using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;

namespace StoreNet.Infrastructure.Layer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<List<Product>> GetAllByCategoryAsync(string categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
