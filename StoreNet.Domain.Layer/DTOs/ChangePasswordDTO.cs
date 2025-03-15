using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs;

public class ChangePasswordDTO
{
    // DTO for changing password

    [Required(ErrorMessage = "CustomerId is required.")]
    public string CustomerId { get; set; }

    [Required(ErrorMessage = "Current Password is required.")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New Password is required.")]
    [MinLength(8, ErrorMessage = "New Password must be at least 8 characters.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm New Password is required.")]
    [Compare("NewPassword", ErrorMessage = "New Password and Confirm New Password do not match.")]
    public string ConfirmNewPassword { get; set; }
}
