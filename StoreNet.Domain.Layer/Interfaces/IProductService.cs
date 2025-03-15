using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IProductService
{
    Task<ApiResponse<ProductResponseDTO>> CreateProductAsync(ProductCreateDTO productDto);
    Task<ApiResponse<ProductResponseDTO>> GetProductByIdAsync(string id);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductAsync(ProductUpdateDTO productDto);
    Task<ApiResponse<ConfirmationResponseDTO>> DeleteProductAsync(string id);
    Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsAsync();
    Task<ApiResponse<List<ProductResponseDTO>>> GetAllProductsByCategoryAsync(string categoryId);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateProductStatusAsync(ProductStatusUpdateDTO productStatusUpdateDTO);
}