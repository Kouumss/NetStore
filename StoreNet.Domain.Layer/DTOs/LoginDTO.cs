using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;
public class LoginDTO
{
    // DTO for customer login
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; }
}
