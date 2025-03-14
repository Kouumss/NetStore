using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task DeleteAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(Guid id);
        Task<Category?> GetByNameAsync(string name);
        Task UpdateAsync(Category category);
    }
}
