using System.ComponentModel.DataAnnotations;

namespace StoreNet.Domain.Layer.DTOs
{
    public class ProductStatusUpdateDTO
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public string ProductId { get; set; }
        [Required(ErrorMessage = "IsAvailable is required.")]
        public bool IsAvailable { get; set; }
    }
}
