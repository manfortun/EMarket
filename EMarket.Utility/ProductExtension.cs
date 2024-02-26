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

        return string.Join(", ", categoryNames);
    }

    public static Category[] GetCategoriesArray(this Product product)
    {
        if (product.Category is null || product.Category.Count < 1)
        {
            return [ CategoryExtension.GetUncategorizedCategory() ]; 
        }

        return product.Category.Select(c => c.Category).ToArray();
    }

    public static int[] GetCategoryIdsArray(this Product product)
    {
        return product.GetCategoriesArray().Select(c => c.Id).ToArray();
    }
}
