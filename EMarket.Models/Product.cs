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
    [Range(1, 1000000.00, ErrorMessage = $"Price cannot be less than 1 or greater than 1, 000, 000.00")]
    public double UnitPrice { get; set; }

    public string? ImageSource { get; set; }

    [MaxLength(100)]
    public string? Description { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public virtual List<ProductCategory>? Category { get; set; }
}
