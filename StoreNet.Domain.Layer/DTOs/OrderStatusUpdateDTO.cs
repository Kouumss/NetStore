using StoreNet.Domain.Layer.Entities;
using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for updating order status
public class OrderStatusUpdateDTO
{
    [Required(ErrorMessage = "OrderId is Required")]
    public string OrderId { get; set; }
    [Required]
    [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid Order Status.")]
    public OrderStatus OrderStatus { get; set; }
}
