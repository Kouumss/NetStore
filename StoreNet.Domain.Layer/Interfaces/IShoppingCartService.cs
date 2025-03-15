using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(string customerId);
        Task<ApiResponse<CartResponseDTO>> AddToCartAsync(AddToCartDTO addToCartDTO);
        Task<ApiResponse<CartResponseDTO>> UpdateCartItemAsync(UpdateCartItemDTO updateCartItemDTO);
        Task<ApiResponse<CartResponseDTO>> RemoveCartItemAsync(RemoveCartItemDTO removeCartItemDTO);
        Task<ApiResponse<ConfirmationResponseDTO>> ClearCartAsync(string customerId);
    }

}
