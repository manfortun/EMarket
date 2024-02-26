using EMarket.Models;
using Microsoft.EntityFrameworkCore;

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
            return [CategoryExtension.GetUncategorizedCategory()];
        }

        return product.Category.Select(c => c.Category).ToArray();
    }

    public static int[] GetCategoryIdsArray(this Product product)
    {
        return product.GetCategoriesArray().Select(c => c.Id).ToArray();
    }

    public static List<Product> AddFilter(this List<Product> items, string? searchKey)
    {
        if (items.Count == 0 || string.IsNullOrEmpty(searchKey))
        {
            return items;
        }

        // filter based on search key
        var filteredProducts = items.Where(item =>
        {
            if (item.Name.Contains(searchKey))
            {
                return true;
            }

            string[] categoryNames = item.Category?.Select(c => c.Category.Name).ToArray() ?? [];

            if (categoryNames.Contains(searchKey)) return true;

            return false;
        }).ToList();

        return filteredProducts;
    }

    public static List<Product> AddFilter(this List<Product> items, int[] categories)
    {
        if (items.Count == 0 || categories.Length == 0)
        {
            return items;
        }

        var filteredProducts = items.Where(item =>
        {
            int[] itemCatIds = item.GetCategoryIdsArray();
            return categories.Intersect(itemCatIds).Any();
        }).ToList();

        return filteredProducts;
    }
}
