using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Domain.Layer.Entities
{
    public class Address : BaseEntity
    {
        // Represents a customer's address

        [Required(ErrorMessage = "Address Line 1 is required.")]
        [StringLength(100, ErrorMessage = "Address Line 1 cannot exceed 100 characters.")]
        public string AddressLine1 { get; set; }

        // Permettre les valeurs nulles pour AddressLine2
        [StringLength(100, ErrorMessage = "Address Line 2 cannot exceed 100 characters.")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is required.")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public string Country { get; set; }

        // Foreign key to Customer
        public string CustomerId { get; set; }

        // Navigation property to Customer
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public AddressType AddressType { get; set; } = AddressType.Billing;
    }
}
