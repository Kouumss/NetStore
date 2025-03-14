using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<Cart> GetActiveCartByCustomerIdAsync(Guid customerId);
        Task<Cart> CreateCartAsync(Guid customerId);
        Task<CartItem> GetCartItemAsync(Guid cartId, Guid productId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task ClearCartItemsAsync(Guid cartId);
        Task SaveChangesAsync();
    }
}
