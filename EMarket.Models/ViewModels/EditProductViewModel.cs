using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EMarket.Models.ViewModels;

public class EditProductViewModel
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public double UnitPrice { get; set; }

    public string ImageSource { get; set; } = default!;

    public int? NumberParameter { get; set; }

    public string CategoriesStringed { get; set; } = default!;

    public void ToggleCategory(int categoryId)
    {
        HashSet<int> tempCategoryHashSet = JsonConvert.DeserializeObject<int[]>(CategoriesStringed).ToHashSet();

        if (tempCategoryHashSet.Add(categoryId) == false)
        {
            tempCategoryHashSet.Remove(categoryId);
        }

        CategoriesStringed = JsonConvert.SerializeObject(tempCategoryHashSet.ToArray());
    }

    public bool HasCategory(int categoryId)
    {
        HashSet<int> tempCategoryHashSet = JsonConvert.DeserializeObject<int[]>(CategoriesStringed).ToHashSet();

        return tempCategoryHashSet.Contains(categoryId);
    }

    public void SetCategories(int[] selectedCategories)
    {
        CategoriesStringed = JsonConvert.SerializeObject(selectedCategories);
    }

    public int[] GetCategories()
    {
        return JsonConvert.DeserializeObject<int[]>(CategoriesStringed);
    }
}
