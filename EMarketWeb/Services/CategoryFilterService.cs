﻿using EMarket.Models;
using EMarketWeb.Services.Interfaces;
using Newtonsoft.Json;

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

        if (!_selectedCategories.Add(categoryId))
        {
            _selectedCategories.Remove(categoryId);
        }
    }

    public void Add(int categoryId)
    {
        _selectedCategories = GetSelectedCategories();

        _selectedCategories.Add(categoryId);
    }

    public HashSet<int> GetSelectedCategories()
    {
        return _selectedCategories ??= [];
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
        return _selectedCategories is not null &&
            _selectedCategories.Contains(categoryId);
    }
}
