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
    public double UnitPrice { get; set; }

    public string ImageSource { get; set; } = default!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
}
