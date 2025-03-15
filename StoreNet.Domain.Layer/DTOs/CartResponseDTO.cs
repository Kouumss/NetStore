namespace StoreNet.Domain.Layer.DTOs;

// DTO for returning cart details
public class CartResponseDTO
{
    public string Id { get; set; }
    public string? CustomerId { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal TotalBasePrice { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CartItemResponseDTO>? CartItems { get; set; }
}
