using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for creating a new order
public class OrderCreateDTO
{
    [Required(ErrorMessage = "Customer ID is required.")]
    public Guid CustomerId { get; set; }
    [Required(ErrorMessage = "Billing Address ID is required.")]
    public Guid BillingAddressId { get; set; }
    [Required(ErrorMessage = "Shipping Address ID is required.")]
    public Guid ShippingAddressId { get; set; }
    [Required(ErrorMessage = "At least one order item is required.")]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<OrderItemCreateDTO> OrderItems { get; set; }
}
