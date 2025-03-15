using System.ComponentModel.DataAnnotations;


namespace StoreNet.Domain.Layer.DTOs;

public class AddressDeleteDTO
{
    [Required(ErrorMessage = "CustomerId is Required")]
    public string CustomerId { get; set; }
    [Required(ErrorMessage = "AddressId is Required")]
    public string AddressId { get; set; }
}
