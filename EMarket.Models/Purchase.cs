using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models;

public class Purchase
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public string OwnerId { get; set; } = default!;

    [DefaultValue(0)]
    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = default!;

    public virtual IdentityUser Owner { get; set; } = default!;

    public static double GetSumPurchase(IEnumerable<Purchase> carts)
    {
        return carts.Sum(c => c.Quantity * c.Product.UnitPrice);
    }

    public static int GetCountOfPurchases(IEnumerable<Purchase> carts)
    {
        return carts.Sum(c => c.Quantity);
    }
}
