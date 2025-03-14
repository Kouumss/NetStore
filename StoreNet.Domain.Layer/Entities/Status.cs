using System.ComponentModel.DataAnnotations;


namespace StoreNet.Domain.Layer.Entities;

public class Status : BaseEntity
{
    // Represents the status master
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
}
