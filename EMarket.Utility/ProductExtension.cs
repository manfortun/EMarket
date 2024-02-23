using EMarket.Models;

namespace EMarket.Utility;

public static class ProductExtension
{
    public static string GetCategoriesFormattedString(this Product product)
    {
        string[] categoryNames = product
            .GetCategoriesArray()
            .Select(c => c.Name)
            .ToArray();

        if (categoryNames.Length < 1 )
        {
            return "Uncategorized";
        }
        else
        {
            return string.Join(", ", product.Category.Select(c => c.Category.Name).ToArray());
        }
    }

    public static Category[] GetCategoriesArray(this Product product)
    {
        return product.Category.Select(c => c.Category).ToArray();
    }
}
