namespace EMarket.Models.ViewModels;

public class OrderSummaryViewModel
{
    public List<Purchase> Purchases { get; set; } = default!;
    public bool IsEditMode { get; set; }
}
