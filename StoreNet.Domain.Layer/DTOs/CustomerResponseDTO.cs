namespace StoreNet.Domain.Layer.DTOs;
public class CustomerResponseDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<AddressResponseDTO> Addresses { get; set; }
    public List<OrderResponseDTO> Orders { get; set; }
}

