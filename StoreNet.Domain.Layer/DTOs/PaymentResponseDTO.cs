using StoreNet.Domain.Layer.Entities;

namespace StoreNet.Domain.Layer.DTOs;

// DTO for payment response
public class PaymentResponseDTO
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
}
