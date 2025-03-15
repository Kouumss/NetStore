using StoreNet.Domain.Layer.DTOs;
using StoreNet.Domain.Layer.Entities;
using StoreNet.Domain.Layer.Interfaces;
using AutoMapper;

namespace StoreNet.Application.Layer.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShoppingCartService(IShoppingCartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CartResponseDTO>> GetCartByCustomerIdAsync(string customerId)
        {
            try
            {
                var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(customerId);
                if (cart is null)
                {
                    var emptyCartDTO = new CartResponseDTO
                    {
                        CustomerId = customerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItemResponseDTO>(),
                        TotalBasePrice = 0,
                        TotalDiscount = 0,
                        TotalAmount = 0
                    };
                    return new ApiResponse<CartResponseDTO>(200, emptyCartDTO);
                }

                // Utilisation d'AutoMapper pour transformer le modèle Cart en DTO
                var cartDTO = _mapper.Map<CartResponseDTO>(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CartResponseDTO>> AddToCartAsync(AddToCartDTO addToCartDTO)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(addToCartDTO.ProductId);
                if (product is null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Product not found.");
                }

                if (addToCartDTO.Quantity > product.StockQuantity)
                {
                    return new ApiResponse<CartResponseDTO>(400, $"Only {product.StockQuantity} units of {product.Name} are available.");
                }

                var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(addToCartDTO.CustomerId);
                if (cart is null)
                {
                    cart = await _cartRepository.CreateCartAsync(addToCartDTO.CustomerId);
                }

                var existingCartItem = await _cartRepository.GetCartItemAsync(cart.Id, addToCartDTO.ProductId);
                if (existingCartItem is not null)
                {
                    if (existingCartItem.Quantity + addToCartDTO.Quantity > product.StockQuantity)
                    {
                        return new ApiResponse<CartResponseDTO>(400, $"Adding {addToCartDTO.Quantity} exceeds available stock.");
                    }

                    existingCartItem.Quantity += addToCartDTO.Quantity;
                    existingCartItem.TotalPrice = (existingCartItem.UnitPrice - existingCartItem.Discount) * existingCartItem.Quantity;
                    existingCartItem.UpdatedAt = DateTime.UtcNow;

                    await _cartRepository.UpdateCartItemAsync(existingCartItem);
                }
                else
                {
                    var discount = product.DiscountPercentage > 0 ? product.Price * product.DiscountPercentage / 100 : 0;
                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = product.Id,
                        Quantity = addToCartDTO.Quantity,
                        UnitPrice = product.Price,
                        Discount = discount,
                        TotalPrice = (product.Price - discount) * addToCartDTO.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _cartRepository.AddCartItemAsync(cartItem);
                }

                cart.UpdatedAt = DateTime.UtcNow;
                await _cartRepository.SaveChangesAsync();

                var cartDTO = _mapper.Map<CartResponseDTO>(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CartResponseDTO>> UpdateCartItemAsync(UpdateCartItemDTO updateCartItemDTO)
        {
            try
            {
                var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(updateCartItemDTO.CustomerId);
                if (cart is null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Active cart not found.");
                }

                var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, updateCartItemDTO.CartItemId);
                if (cartItem is null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Cart item not found.");
                }

                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product is null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Product not found.");
                }

                if (updateCartItemDTO.Quantity > product.StockQuantity)
                {
                    return new ApiResponse<CartResponseDTO>(400, $"Only {product.StockQuantity} units of {product.Name} are available.");
                }

                cartItem.Quantity = updateCartItemDTO.Quantity;
                cartItem.TotalPrice = (cartItem.UnitPrice - cartItem.Discount) * cartItem.Quantity;
                cartItem.UpdatedAt = DateTime.UtcNow;

                await _cartRepository.UpdateCartItemAsync(cartItem);

                cart.UpdatedAt = DateTime.UtcNow;
                await _cartRepository.SaveChangesAsync();

                var cartDTO = _mapper.Map<CartResponseDTO>(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CartResponseDTO>> RemoveCartItemAsync(RemoveCartItemDTO removeCartItemDTO)
        {
            try
            {
                var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(removeCartItemDTO.CustomerId);
                if (cart is null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Active cart not found.");
                }

                var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, removeCartItemDTO.CartItemId);
                if (cartItem is null)
                {
                    return new ApiResponse<CartResponseDTO>(404, "Cart item not found.");
                }

                await _cartRepository.RemoveCartItemAsync(cartItem);

                cart.UpdatedAt = DateTime.UtcNow;
                await _cartRepository.SaveChangesAsync();

                var cartDTO = _mapper.Map<CartResponseDTO>(cart);
                return new ApiResponse<CartResponseDTO>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<CartResponseDTO>(500, $"An unexpected error occurred, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDTO>> ClearCartAsync(string customerId)
        {
            try
            {
                var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(customerId);
                if (cart is null)
                {
                    return new ApiResponse<ConfirmationResponseDTO>(404, "Active cart not found.");
                }

                await _cartRepository.ClearCartItemsAsync(cart.Id);

                var confirmation = new ConfirmationResponseDTO
                {
                    Message = "Cart has been cleared successfully."
                };

                return new ApiResponse<ConfirmationResponseDTO>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDTO>(500, $"An unexpected error occurred, Error: {ex.Message}");
            }
        }
    }
}
