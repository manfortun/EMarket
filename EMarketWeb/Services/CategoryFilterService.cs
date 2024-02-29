using EMarket.Models;
using EMarketWeb.Services.Interfaces;

namespace EMarketWeb.Services;

public class CategoryFilterService : ICategoryFilterService
{
    private List<Category> _categories = default!;

    public List<Category> Categories
    {
        get => _categories is null ? [] : _categories;
        set => _categories = value;
    }

    public bool IsCategoriesSet => Categories?.Count > 0;
    private HashSet<int>? _selectedCategories = default!;

    public void SetCategories(List<Category> categories)
    {
        Categories = categories;

        // remove any obsolete categories
        if (_selectedCategories != null)
        {
            IEnumerable<int> obsoleteCategories = _selectedCategories.Except(Categories.Select(c => c.Id));
            foreach (var c in obsoleteCategories)
            {
                _selectedCategories.Remove(c);
            }
        }
    }
    public void Toggle(int categoryId)
    {
        _selectedCategories = GetSelectedCategories();

        // when ID is already existing...
        if (!_selectedCategories.Add(categoryId))
        {
            //...remove
            _selectedCategories.Remove(categoryId);
        }
    }

    /// <summary>
    /// Adds a <paramref name="categoryId"/> to selected categories list
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    public void Add(int categoryId)
    {
        _selectedCategories = GetSelectedCategories();

        _selectedCategories.Add(categoryId);
    }

    public HashSet<int> GetSelectedCategories()
    {
        return _selectedCategories ??= [];
    }

    public void Clear()
    {
        Categories?.Clear();
        _selectedCategories?.Clear();
    }

    public bool IsSelected(int categoryId)
    {
        return _selectedCategories is not null &&
            _selectedCategories.Contains(categoryId);
    }
}
