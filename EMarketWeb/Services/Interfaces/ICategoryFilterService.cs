using EMarket.Models;

namespace EMarketWeb.Services.Interfaces;

public interface ICategoryFilterService : ISessionService
{
    List<Category> Categories { get; set; }
    bool IsCategoriesSet { get; }

    void SetCategories(List<Category> categories);

    void Toggle(int categoryId);

    int[] GetSelectedCategories();

    bool IsDisplayed(int categoryId);
}
