using EMarket.Models;

namespace EMarket.Utility;

public class CategoryExtension
{
    public static Category GetUncategorizedCategory()
    {
        return new Category
        {
            Id = -1,
            Name = "Uncategorized"
        };
    }
}
