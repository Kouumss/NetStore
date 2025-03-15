using StoreNet.Domain.Layer.DTOs;

namespace StoreNet.Domain.Layer.Interfaces;

public interface IOrderService
{
    Task<ApiResponse<OrderResponseDTO>> CreateOrderAsync(OrderCreateDTO orderDto);
    Task<ApiResponse<OrderResponseDTO>> GetOrderByIdAsync(string orderId);
    Task<ApiResponse<List<OrderResponseDTO>>> GetAllOrdersAsync();
    Task<ApiResponse<List<OrderResponseDTO>>> GetOrdersByCustomerAsync(string customerId);
    Task<ApiResponse<ConfirmationResponseDTO>> UpdateOrderStatusAsync(OrderStatusUpdateDTO statusDto);
}
