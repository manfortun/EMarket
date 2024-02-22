using Microsoft.AspNetCore.Identity;

namespace EMarket.Models;

public class Checkout
{
    public Receiver Receiver { get; set; }
    public IEnumerable<Cart> Purchases { get; set; }
    public string UserId { get; set; }

    public Checkout(
        IEnumerable<Cart> purchases,
        IdentityUser user)
    {
        Receiver = new Receiver()
        {
            OwnerId = user.Id,
        };

        Purchases = purchases;
        UserId = user.Id;
    }

    public double GetSumPurchase()
    {
        return Purchases.Sum(p => p.Quantity * p.Product.UnitPrice);
    }

    public int GetCountOfPurchases()
    {
        return Purchases.Sum(p => p.Quantity);
    }

    public bool HasPurchases()
    {
        return Purchases is not null && Purchases.Any();
    }
}
