using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(100)]
    [DisplayName("Category Name")]
    public string Name { get; set; } = default!;

    [Range(1, 100, ErrorMessage = "Display Order must be between 1-100.")]
    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }
}
