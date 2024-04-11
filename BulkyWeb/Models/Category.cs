﻿using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(30)]
    [Display(Name = "Category Name")]
    public string Name { get; set; }

    [Display(Name = "Display Order")]
    [Range(1,100,ErrorMessage="Diplay Order must be between 1-100")]
    public int DisplayOrder { get; set; }
}
