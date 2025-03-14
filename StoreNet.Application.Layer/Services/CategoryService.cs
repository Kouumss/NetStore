using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Application.Layer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO categoryDto)
        {
            try
            {
                // Vérifier si le nom de la catégorie existe déjà
                var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
                if (existingCategory is not null)
                {
                    return new ApiResponse<CategoryResponseDTO>(400, "Category name already exists.");
                }

                // Créer une nouvelle catégorie
                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    IsActive = true
                };

                // Ajouter la catégorie au repository
                await _categoryRepository.AddAsync(category);

                // Mapper la réponse
                var categoryResponse = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };

                return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category is null)
                {
                    return new ApiResponse<CategoryResponseDTO>(404, "Category not found.");
                }

                var categoryResponse = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive
                };

                return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);
                if (category is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");
                }

                // Vérifier si le nom de la catégorie existe déjà (sauf pour la catégorie actuelle)
                if (category.Name != categoryDto.Name)
                {
                    var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
                    if (existingCategory is not null)
                    {
                        return new ApiResponse<ConfirmationResponseDTO>(400, "Another category with the same name already exists.");
                    }
                }

                // Mise à jour des propriétés de la catégorie
                category.Name = categoryDto.Name;

                // Si la description n'est pas vide, on la met à jour, sinon on garde l'ancienne description
                if (!string.IsNullOrEmpty(categoryDto.Description))
                {
                    category.Description = categoryDto.Description;
                }

                // Mettre à jour la catégorie dans la base de données
                await _categoryRepository.UpdateAsync(category);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Category with Id {categoryDto.Id} updated successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }


        public async Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Category not found.");
                }

                // Soft delete (désactivation de la catégorie)
                category.IsActive = false;
                await _categoryRepository.UpdateAsync(category);

                var confirmationMessage = new ConfirmationResponseDTO
                {
                    Message = $"Category with Id {id} deleted successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmationMessage);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoryList = categories.Select(c => new CategoryResponseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive
                }).ToList();

                return new ApiResponse<List<CategoryResponseDTO>>(200, categoryList);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<CategoryResponseDTO>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
