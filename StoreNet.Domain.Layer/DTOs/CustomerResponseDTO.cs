namespace StoreNet.Domain.Layer.DTOs;
public class CustomerResponseDTO
{
    // DTO for returning customer details.
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}
