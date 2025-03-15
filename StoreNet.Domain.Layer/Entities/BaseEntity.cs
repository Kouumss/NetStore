namespace StoreNet.Domain.Layer.Entities;

public class BaseEntity
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }


    // Méthode pour définir la date de mise à jour
    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
