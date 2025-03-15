using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using StoreNet.Domain.Layer.Interfaces;


namespace StoreNet.Domain.Layer.Entities;

public class Cart : BaseEntity
{
    // Represents a shopping cart
    public string CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
    public bool IsCheckedOut { get; set; } = false;
    [Required]
    public DateTime CreatedAt { get; set; }
    [Required]
    public DateTime UpdatedAt { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
}
