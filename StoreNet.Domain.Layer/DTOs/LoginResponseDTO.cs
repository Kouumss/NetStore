namespace StoreNet.Domain.Layer.DTOs;
public class LoginResponseDTO
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string Message { get; set; }
}
