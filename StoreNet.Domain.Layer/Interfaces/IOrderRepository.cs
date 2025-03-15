using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IOrderRepository
{
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(string orderId);
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetOrdersByCustomerAsync(string customerId);
    Task UpdateOrderAsync(Order order);
}

