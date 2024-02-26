using EMarket.Models;
using EMarketWeb.Services.Interfaces;
using Newtonsoft.Json;

namespace EMarketWeb.Services;

public class CategoryFilterService : ICategoryFilterService
{
    public List<Category> Categories { get; set; } = default!;
    public bool IsCategoriesSet => Categories?.Count > 0;
    private HashSet<int>? _selectedCategories = default!;

    public void SetCategories(List<Category> categories)
    {
        Categories = categories;
    }

    public void Toggle(int categoryId)
    {
        InitSelectedCategories();

        ArgumentNullException.ThrowIfNull(_selectedCategories);

        if (!_selectedCategories.Add(categoryId))
        {
            _selectedCategories.Remove(categoryId);
        }
    }

    public int[] GetSelectedCategories()
    {
        if (_selectedCategories is null)
        {
            _selectedCategories = Categories.Select(c => c.Id).ToHashSet();
        }
        return [.. _selectedCategories];
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(_selectedCategories);
    }

    public void Clear()
    {
        Categories?.Clear();
        _selectedCategories?.Clear();
    }

    public bool IsDisplayed(int categoryId)
    {
        return _selectedCategories is null ? false : _selectedCategories.Contains(categoryId);
    }

    private void InitSelectedCategories()
    {
        if (_selectedCategories is null)
        {
            _selectedCategories = Categories.Select(c => c.Id).ToHashSet();
        }
    }
}
