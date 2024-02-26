namespace EMarket.Utility;

public static class PaginationExtension
{
    public static IEnumerable<T> GoToPage<T>(this IEnumerable<T> items, int pageNumber, int noOfItemsInPage, out int activePage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(pageNumber);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(noOfItemsInPage);
        
        IEnumerable<T> paginatedItems;
        activePage = pageNumber;

        if (!items.Any())
        {
            return [];
        }

        int maxNoOfPages = items.GetMaxNoOfPages(noOfItemsInPage);
        activePage = Math.Min(maxNoOfPages, pageNumber);

        paginatedItems = items.Skip((activePage - 1) * noOfItemsInPage).Take(noOfItemsInPage);

        return paginatedItems;
    }

    public static int GetMaxNoOfPages<T>(this IEnumerable<T> items, int noOfItemsInPage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(noOfItemsInPage);

        return (int)Math.Ceiling((double)items.Count() / noOfItemsInPage);
    }
}
