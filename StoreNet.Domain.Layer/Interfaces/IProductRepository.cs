using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> GetByNameAsync(string name);
    Task<List<Product>> GetAllAsync();
    Task<List<Product>> GetAllByCategoryAsync(Guid categoryId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
