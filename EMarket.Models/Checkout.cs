using Newtonsoft.Json;

namespace EMarket.Models;

public class Checkout : Receiver, ICloneable
{
    public string? PurchasesStringed { get; set; }

    public Checkout(
        IEnumerable<Cart> purchases,
        string ownerId)
    {
        OwnerId = ownerId;
        SetPurchases(purchases);
    }

    public Checkout() { }

    public bool HasPurchases()
    {
        int[] purchases = GetPurchases();
        return purchases.Any();
    }

    public int[] GetPurchases()
    {
        if (string.IsNullOrEmpty(PurchasesStringed))
        {
            return [];
        }

        return JsonConvert.DeserializeObject<int[]>(PurchasesStringed);
    }

    public void SetPurchases(IEnumerable<Cart> purchases)
    {
        int[] purchaseIds = purchases is null || !purchases.Any() ? []
            : purchases.Select(p => p.Id).ToArray();

        PurchasesStringed = JsonConvert.SerializeObject(purchaseIds);
    }

    public static double GetSumPurchase(IEnumerable<Cart> carts)
    {
        return carts.Sum(c => c.Quantity * c.Product.UnitPrice);
    }

    public static int GetCountOfPurchases(IEnumerable<Cart> carts)
    {
        return carts.Sum(c => c.Quantity);
    }

    public object Clone()
    {
        return new Checkout
        {
            Id = Id,
            OwnerId = OwnerId,
            FirstName = FirstName,
            LastName = LastName,
            ContactNo = ContactNo,
            Address = Address,
            PurchasesStringed = PurchasesStringed,
        };
    }
}
