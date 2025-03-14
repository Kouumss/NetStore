namespace StoreNet.Domain.Layer.DTOs;

// DTO for returning category details.
public class CategoryResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
