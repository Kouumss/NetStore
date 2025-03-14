﻿using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for updating a cart item's quantity
public class UpdateCartItemDTO
{
    [Required(ErrorMessage = "CustomerId is required.")]
    public string CustomerId { get; set; }
    [Required(ErrorMessage = "CartItemId is required.")]
    public string CartItemId { get; set; }
    [Required(ErrorMessage = "Quantity is required.")]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
    public int Quantity { get; set; }
}
