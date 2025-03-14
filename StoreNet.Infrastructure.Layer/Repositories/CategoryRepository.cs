using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using StoreNet.Infrastructure.Layer.Data;

namespace StoreNet.Infrastructure.Layer.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Adds a new category to the database
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category); // Adds the category to the DbSet
            await _context.SaveChangesAsync(); // Saves changes to the database
        }

        // Retrieves a category by its ID
        public async Task<Category> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories
                .AsNoTracking() // No need to track the entity for this read-only operation
                .FirstOrDefaultAsync(c => c.Id == id); // Fetches the category by ID

            if (category is null)
            {
                // Throws an exception if no category is found
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            return category;
        }

        // Retrieves a category by its name (case-insensitive)
        public async Task<Category?> GetByNameAsync(string name)
        {
            var category = await _context.Categories
                .AsNoTracking() // No need to track the entity for this read-only operation
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower()); 

            return category; // Retourne la catégorie trouvée, ou null si aucune catégorie n'est trouvée
        }



        // Updates an existing category in the database
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category); // Marks the category as updated
            await _context.SaveChangesAsync(); // Saves changes to the database
        }

        // Soft deletes a category by marking it as inactive
        public async Task DeleteAsync(Category category)
        {
            category.IsActive = false; // Marks the category as inactive (soft delete)
            _context.Categories.Update(category); // Marks the category as updated
            await _context.SaveChangesAsync(); // Saves changes to the database
        }

        // Retrieves all categories from the database
        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .AsNoTracking() // No need to track the entity for this read-only operation
                .ToListAsync(); // Fetches all categories
        }
    }
}
