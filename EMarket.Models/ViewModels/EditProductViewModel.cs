using Newtonsoft.Json;

namespace EMarket.Models.ViewModels;

public class EditProductViewModel : Product
{
    /// <summary>
    /// ID of the selected category used as argument from the view layer
    /// </summary>
    public int? NumberParameter { get; set; }

    /// <summary>
    /// Serialized array of selected IDs
    /// </summary>
    public string? CategoriesStringed { get; set; }

    /// <summary>
    /// Sets a <paramref name="categoryId"/> as either selected or unselected
    /// </summary>
    /// <param name="categoryId">The category ID to toggle</param>
    public void ToggleCategory(int categoryId)
    {
        HashSet<int> tempCategoryHashSet = [.. GetCategoryIdsArray()];

        if (!tempCategoryHashSet.Add(categoryId))
        {
            tempCategoryHashSet.Remove(categoryId);
        }

        CategoriesStringed = JsonConvert.SerializeObject(tempCategoryHashSet.ToArray());
    }

    /// <summary>
    /// Checks if the <paramref name="categoryId"/> is selected.
    /// </summary>
    /// <param name="categoryId">The category ID to evaluate</param>
    /// <returns></returns>
    public bool HasCategory(int categoryId)
    {
        HashSet<int> tempCategoryHashSet = [.. GetCategoryIdsArray()];

        return tempCategoryHashSet.Contains(categoryId);
    }

    /// <summary>
    /// Serializes the <paramref name="selectedCategories"/> to CategoriesStringed as JSON string
    /// </summary>
    /// <param name="selectedCategories">The array of IDs to serialize</param>
    public void SetCategories(int[] selectedCategories)
    {
        CategoriesStringed = JsonConvert.SerializeObject(selectedCategories);
    }

    /// <summary>
    /// Deserializes CategoriesStringed into array of IDs
    /// </summary>
    /// <returns></returns>
    public int[] GetCategoryIdsArray()
    {
        if (string.IsNullOrEmpty(CategoriesStringed))
        {
            return [];
        }

        return JsonConvert.DeserializeObject<int[]>(CategoriesStringed);
    }

    /// <summary>
    /// Converts <paramref name="product"/> to <see cref="EditProductViewModel"/>.
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public static EditProductViewModel Convert(Product product)
    {
        var viewModel = new EditProductViewModel
        {
            Name = product.Name,
            UnitPrice = product.UnitPrice,
            ImageSource = product.ImageSource,
            Id = product.Id,
            Description = product.Description,
            DateCreated = product.DateCreated
        };

        viewModel.SetCategories(product.Category?.Select(c => c.CategoryId).ToArray() ?? []);

        return viewModel;
    }
}
