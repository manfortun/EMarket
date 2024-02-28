using EMarket.Models;

namespace EMarketWeb.Services.Interfaces;

public interface ICategoryFilterService : ISessionService
{
    List<Category> Categories { get; set; }
    bool IsCategoriesSet { get; }

    void SetCategories(List<Category> categories);

    /// <summary>
    /// Adds or removes a <paramref name="categoryId"/> from the selected categories list.
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    void Toggle(int categoryId);

    /// <summary>
    /// Obtains selected categories hash set
    /// </summary>
    /// <returns></returns>
    HashSet<int> GetSelectedCategories();

    /// <summary>
    /// Checks if the <paramref name="categoryId"/> is selected.
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns><c>true</c> if selected. Otherwise, <c>false</c>.</returns>
    bool IsSelected(int categoryId);
}
