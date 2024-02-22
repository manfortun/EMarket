using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class Cart
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public string OwnerId { get; set; } = default!;

    [DefaultValue(0)]
    public int Quantity { get; set; }

    public virtual Product Product { get; set; }

    public virtual IdentityUser Owner { get; set; }
}
