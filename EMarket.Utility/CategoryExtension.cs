using EMarket.Models;

namespace EMarket.Utility;

public static class CategoryExtension
{
    /// <summary>
    /// Returns the <c>Uncategorized</c> category
    /// </summary>
    /// <returns></returns>
    public static Category GetUncategorizedCategory()
    {
        return new Category
        {
            Id = -1,
            Name = "Uncategorized"
        };
    }
}
