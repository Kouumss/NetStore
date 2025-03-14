using System.ComponentModel.DataAnnotations;


namespace StoreNet.Domain.Layer.DTOs;

public class AddressDeleteDTO
{
    [Required(ErrorMessage = "CustomerId is Required")]
    public Guid CustomerId { get; set; }
    [Required(ErrorMessage = "AddressId is Required")]
    public Guid AddressId { get; set; }
}
