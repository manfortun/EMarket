using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;

    [DisplayName("Price")]
    [Range(1, double.MaxValue, ErrorMessage = "Price cannot be 0 or negative.")]
    public double UnitPrice { get; set; }

    public string? ImageSource { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public virtual List<ProductCategory>? Category { get; set; }
}
