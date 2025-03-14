using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for returning complete order details.
public class OrderResponseDTO
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BillingAddressId { get; set; }
    public Guid ShippingAddressId { get; set; }
    public decimal TotalBaseAmount { get; set; }
    public decimal TotalDiscountAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public List<OrderItemResponseDTO> OrderItems { get; set; }
}
