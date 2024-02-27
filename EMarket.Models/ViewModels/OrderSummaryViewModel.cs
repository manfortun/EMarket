namespace EMarket.Models.ViewModels;

public class OrderSummaryViewModel
{
    public List<Purchase> Carts { get; set; } = default!;
    public bool IsEditMode { get; set; }
}
