using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.Entities;

public class Payment : BaseEntity
{
    // Represents a payment transaction
    [Required]
    public Guid OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order Order { get; set; }
    [Required]
    [StringLength(50)]
    public string PaymentMethod { get; set; } // e.g., "DebitCard", "CreditCard", "PayPal", "COD"
    [StringLength(50)]
    public string? TransactionId { get; set; } // From payment gateway
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    [Required]
    public DateTime PaymentDate { get; set; }
    [Required]
    [StringLength(20)]
    public PaymentStatus Status { get; set; } // "Completed", "Pending", "Failed", "Refunded"
    public Refund Refund { get; set; } // Navigational property to Refund
}
