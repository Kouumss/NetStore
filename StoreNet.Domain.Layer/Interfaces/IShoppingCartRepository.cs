using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<Cart> GetActiveCartByCustomerIdAsync(string customerId);
        Task<Cart> CreateCartAsync(string customerId);
        Task<CartItem> GetCartItemAsync(string cartId, string productId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task ClearCartItemsAsync(string cartId);
        Task SaveChangesAsync();
    }
}
