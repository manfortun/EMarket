namespace EMarket.Models;

public class PageInfo<T>
{
    private IEnumerable<T> _items;

    public readonly int NoOfItemsPerPage;
    public int ActivePage { get; set; } = 0;
    public int NoOfPages { get; set; } = 0;

    public IEnumerable<T> Items => _items;

    public IEnumerable<T> ActiveItems
    {
        get
        {
            IEnumerable<T> paginatedItems;

            if (!_items.Any())
            {
                return [];
            }

            paginatedItems = _items
                .Skip((ActivePage - 1) * NoOfItemsPerPage)
                .Take(NoOfItemsPerPage);

            return paginatedItems;
        }
    }

    public PageInfo(int noOfItemsPerPage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(noOfItemsPerPage);

        _items = [];
        NoOfItemsPerPage = noOfItemsPerPage;
    }

    public void RefreshNoOfPages(IEnumerable<T> items)
    {
        _items = items;
        NoOfPages = (int)Math.Ceiling((double)items.Count() / NoOfItemsPerPage);
        GoToPage(ActivePage);
    }

    public void GoToPage(int pageNo)
    {
        if (pageNo > NoOfPages)
        {
            pageNo = NoOfPages;
        }
        else if (pageNo < 1)
        {
            pageNo = 1;
        }

        ActivePage = pageNo;
    }
}
