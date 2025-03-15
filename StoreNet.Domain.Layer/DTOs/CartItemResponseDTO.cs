namespace StoreNet.Domain.Layer.DTOs;

// DTO for returning cart item details
public class CartItemResponseDTO
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}
