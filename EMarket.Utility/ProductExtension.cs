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
        if (product.Category is null ||
            !product.Category.Any())
        {
            return [CategoryExtension.GetUncategorizedCategory()];
        }

        return product.Category.Select(c => c.Category).ToArray();
    }

    public static int[] GetCategoryIdsArray(this Product product)
    {
        return product.GetCategoriesArray().Select(c => c.Id).ToArray();
    }

    public static IEnumerable<Product> AddFilter(this IEnumerable<Product> items, string? searchKey)
    {
        // create a local copy in case the argument came from database
        List<Product> itemsList = [.. items];

        if (!itemsList.Any() ||
            string.IsNullOrEmpty(searchKey))
        {
            return itemsList;
        }

        // filter based on search key
        var filteredProducts = itemsList.Where(item =>
        {
            if (item.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            List<string> categoryNames = item.Category?.Select(c => c.Category.Name).ToList() ?? [];

            if (categoryNames.Exists(c => c.Contains(searchKey, StringComparison.OrdinalIgnoreCase))) return true;

            return false;
        }).ToList();

        return filteredProducts;
    }

    public static IEnumerable<Product> AddFilter(this IEnumerable<Product> items, int[] categories)
    {
        // create a local copy in case the argument came from database
        List<Product> itemsList = [.. items];

        if (!itemsList.Any() ||
            categories.Length == 0)
        {
            return itemsList;
        }

        var filteredProducts = itemsList.Where(item =>
        {
            int[] itemCatIds = item.GetCategoryIdsArray();
            return categories.Intersect(itemCatIds).Any();
        }).ToList();

        return filteredProducts;
    }
}
