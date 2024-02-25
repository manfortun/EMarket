using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models.ViewModels;

public class EditProductViewModel : Product
{
    public int? NumberParameter { get; set; }

    public string? CategoriesStringed { get; set; }

    public void ToggleCategory(int categoryId)
    {
        HashSet<int> tempCategoryHashSet = GetCategories().ToHashSet();

        if (tempCategoryHashSet.Add(categoryId) == false)
        {
            tempCategoryHashSet.Remove(categoryId);
        }

        CategoriesStringed = JsonConvert.SerializeObject(tempCategoryHashSet.ToArray());
    }

    public bool HasCategory(int categoryId)
    {
        HashSet<int> tempCategoryHashSet = GetCategories().ToHashSet();

        return tempCategoryHashSet.Contains(categoryId);
    }

    public void SetCategories(int[] selectedCategories)
    {
        CategoriesStringed = JsonConvert.SerializeObject(selectedCategories);
    }

    public int[] GetCategories()
    {
        if (string.IsNullOrEmpty(CategoriesStringed))
        {
            return [];
        }

        return JsonConvert.DeserializeObject<int[]>(CategoriesStringed);
    }

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

        viewModel.SetCategories(product.Category.Select(c => c.CategoryId).ToArray());

        return viewModel;
    }
}
