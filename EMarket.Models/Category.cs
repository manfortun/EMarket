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
}
