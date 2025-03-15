using System.ComponentModel.DataAnnotations;


namespace StoreNet.Domain.Layer.DTOs;

// DTO for adding an item to the cart
public class AddToCartDTO
{
    [Required(ErrorMessage = "CustomerId is required.")]
    public string CustomerId { get; set; }
    [Required(ErrorMessage = "ProductId is required.")]
    public string ProductId { get; set; }
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; }
}
