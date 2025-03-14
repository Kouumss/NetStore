﻿using System.ComponentModel.DataAnnotations;

public class CustomerRegistrationDTO
{
    // DTO for customer registration

    [Required(ErrorMessage = "First Name is required.")]
    [MinLength(2, ErrorMessage = "First Name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "PhoneNumber is required.")]
    [Phone(ErrorMessage = "Invalid Phone Number.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "DateOfBirth is required.")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; }

    // Adress information (obligatory for registration)
    [Required(ErrorMessage = "Address Line 1 is required.")]
    [MaxLength(100, ErrorMessage = "Address Line 1 cannot exceed 100 characters.")]
    public string AddressLine1 { get; set; }

    [MaxLength(100, ErrorMessage = "Address Line 2 cannot exceed 100 characters.")]
    public string AddressLine2 { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
    public string City { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [MaxLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
    public string State { get; set; }

    [Required(ErrorMessage = "Postal Code is required.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
    public string Country { get; set; }
}
