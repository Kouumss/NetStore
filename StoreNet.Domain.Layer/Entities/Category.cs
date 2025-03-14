﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using StoreNet.Domain.Layer.Interfaces;


namespace StoreNet.Domain.Layer.Entities;

[Index(nameof(Name), Name = "IX_Name_Unique", IsUnique = true)]
public class Category : BaseEntity
{
    // Represents a product category
    [Required(ErrorMessage = "Category Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Category Name must be between 3 and 100 characters.")]
    public string Name { get; set; }
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Product> Products { get; set; }
}
