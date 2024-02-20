using System.ComponentModel.DataAnnotations;

namespace EMarketWeb.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = default!;
    public int DisplayOrder { get; set; }
}
