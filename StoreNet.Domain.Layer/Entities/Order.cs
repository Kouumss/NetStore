using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StoreNet.Domain.Layer.Interfaces;

namespace StoreNet.Domain.Layer.Entities
{
    public class Order : BaseEntity
    {
        // Represents an order placed by a customer
        [Required]
        [StringLength(30, ErrorMessage = "Order Number cannot exceed 30 characters.")]
        public string OrderNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        // Foreign key to Customer
        [Required(ErrorMessage = "Customer ID is required.")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        // Foreign keys to Addresses
        [Required(ErrorMessage = "Billing Address ID is required.")]
        public string BillingAddressId { get; set; }

        [ForeignKey("BillingAddressId")]
        public Address BillingAddress { get; set; }

        [Required(ErrorMessage = "Shipping Address ID is required.")]
        public string ShippingAddressId { get; set; }

        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddress { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, 100000.00, ErrorMessage = "Total Base Amount must be between $0.00 and $100,000.00.")]
        public decimal TotalBaseAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, 100000.00, ErrorMessage = "Total Discount Amount must be between $0.00 and $100,000.00.")]
        public decimal TotalDiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, 10000.00, ErrorMessage = "Shipping Cost must be between $0.00 and $10,000.00.")]
        public decimal ShippingCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, 110000.00, ErrorMessage = "Total Amount must be between $0.00 and $110,000.00.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid Order Status.")]
        public OrderStatus OrderStatus { get; set; }

        // Navigation properties
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
        public Cancellation Cancellation { get; set; }

        // Constructor to initialize collections
        public Order()
        {
            OrderItems = new List<OrderItem>();

        }
    }
}
