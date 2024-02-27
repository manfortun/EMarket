namespace EMarket.Models.ViewModels;

public class OrderSummaryViewModel
{
    public List<Cart> Carts { get; set; } = default!;
    public bool IsEditMode { get; set; }
}
