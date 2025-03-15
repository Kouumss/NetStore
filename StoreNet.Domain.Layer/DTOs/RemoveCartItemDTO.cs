using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for removing an item from the cart
public class RemoveCartItemDTO
{
    [Required(ErrorMessage = "CustomerId is required.")]
    public string CustomerId { get; set; }
    [Required(ErrorMessage = "CartItemId is required.")]
    public string CartItemId { get; set; }
}
