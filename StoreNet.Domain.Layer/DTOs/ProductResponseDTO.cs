

namespace StoreNet.Domain.Layer.DTOs
{
    // DTO for returning product details.
    public class ProductResponseDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public int DiscountPercentage { get; set; }
        public string CategoryId { get; set; }
        public bool IsAvailable { get; set; }
    }
}
