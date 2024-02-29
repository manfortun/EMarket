using EMarket.Models;

namespace EMarket.Utility;

public static class ProductExtension
{
    /// <summary>
    /// Obtains the category names in a comma-separated format
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static string GetCategoriesFormattedString(this Product product)
    {
        string[] categoryNames = product
            .GetCategoriesArray()
            .Select(c => c.Name)
            .ToArray();

        return string.Join(", ", categoryNames);
    }

    /// <summary>
    /// Obtains the categories of a <paramref name="product"/>. When the <paramref name="product"/> does not belong to a category, returns the <c>Uncategorized</c> category
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static Category[] GetCategoriesArray(this Product product)
    {
        if (product.Category is null ||
            !product.Category.Any())
        {
            return [CategoryExtension.GetUncategorizedCategory()];
        }

        return product.Category.Select(c => c.Category).ToArray();
    }

    /// <summary>
    /// Obtains the array of IDs of the <paramref name="product"/> categories.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static int[] GetCategoryIdsArray(this Product product)
    {
        return product.GetCategoriesArray().Select(c => c.Id).ToArray();
    }

    /// <summary>
    /// Applies a filter to a collection of <paramref name="items"/> based on a <paramref name="searchKey"/>.
    /// </summary>
    /// <param name="items">The items to filter.</param>
    /// <param name="searchKey">Filter search key.</param>
    /// <returns>Items that passed the filter.</returns>
    public static IEnumerable<Product> AddFilter(this IEnumerable<Product> items, string? searchKey)
    {
        // when the searchKey is null or empty, all items passed the filtration
        if (!items.Any() ||
            string.IsNullOrEmpty(searchKey))
        {
            return items;
        }

        // filter based on search key
        var filteredProducts = items.Where(item =>
        {
            // check name
            if (item.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string categoryNames = item.GetCategoriesFormattedString();

            // check categories
            if (categoryNames.Contains(searchKey, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }).ToList();

        return filteredProducts;
    }

    /// <summary>
    /// Applies a filter to a collection of <paramref name="items"/> based on <paramref name="categories"/>.
    /// </summary>
    /// <param name="items">The items to filter.</param>
    /// <param name="categories">Filter categories</param>
    /// <returns></returns>
    public static IEnumerable<Product> AddFilter(this IEnumerable<Product> items, int[] categories)
    {
        // when there are no categories, all items are filtered out
        if (!items.Any() ||
            categories.Length == 0)
        {
            return [];
        }

        IEnumerable<Product> filteredProducts = [.. items
            .Where(item =>
            {
                int[] itemCatIds = item.GetCategoryIdsArray();
                return categories.Intersect(itemCatIds).Any();
            })];

        return filteredProducts;
    }
}
