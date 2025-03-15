using System.ComponentModel.DataAnnotations;
using StoreNet.Domain.Layer.Interfaces;


namespace StoreNet.Domain.Layer.Entities;

public class Status : BaseEntity
{
    // Represents the status master
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public int Rank { get; set; }
}
