﻿namespace StoreNet.Domain.Layer.DTOs;

// DTO for returning individual order item details.
public class OrderItemResponseDTO
{
    public string Id { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}
