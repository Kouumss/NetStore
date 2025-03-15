using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for creating a new order
public class OrderCreateDTO
{
    [Required(ErrorMessage = "Customer ID is required.")]
    public string CustomerId { get; set; }


    [Required(ErrorMessage = "At least one order item is required.")]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<OrderItemCreateDTO> OrderItems { get; set; }
}
