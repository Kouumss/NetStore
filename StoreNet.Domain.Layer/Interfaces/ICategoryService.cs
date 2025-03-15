using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryCreateDTO categoryDto);
        Task<ApiResponse<CategoryResponseDTO>> GetCategoryByIdAsync(string id);
        Task<ApiResponse<ConfirmationResponseDTO>> UpdateCategoryAsync(CategoryUpdateDTO categoryDto);
        Task<ApiResponse<ConfirmationResponseDTO>> DeleteCategoryAsync(string id);
        Task<ApiResponse<List<CategoryResponseDTO>>> GetAllCategoriesAsync();
    }
}
