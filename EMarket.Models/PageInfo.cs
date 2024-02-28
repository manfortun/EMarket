namespace EMarket.Models;

public class PageInfo<T>
{
    private IEnumerable<T> _items;
    private int _noOfPages = 0;
    private int _activePage = 0;

    /// <summary>
    /// Desired number of items to be displayed on the page.
    /// </summary>
    public readonly int NoOfItemsPerPage;

    /// <summary>
    /// The current active page. Cannot be less than 1 or more than <c>NoOfPages</c>
    /// </summary>
    public int ActivePage
    {
        get
        {
            return _activePage;
        }
        set
        {
            if (value > NoOfPages)
            {
                _activePage = NoOfPages;
            }
            else if (value < 1)
            {
                _activePage = 1;
            }
            else
            {
                _activePage = value;
            }
        }
    }

    /// <summary>
    /// Original list of items
    /// </summary>
    public IEnumerable<T> Items => _items;

    /// <summary>
    /// Items currently in display
    /// </summary>
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

    /// <summary>
    /// Total number of pages. If set, the <c>ActivePage</c> is also automatically calculated
    /// </summary>
    public int NoOfPages
    {
        get => _noOfPages;
        set
        {
            _noOfPages = value;
            ActivePage = _activePage;
        }
    }

    public PageInfo(int noOfItemsPerPage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(noOfItemsPerPage);

        _items = [];
        NoOfItemsPerPage = noOfItemsPerPage;
    }

    /// <summary>
    /// Sets the original list of items, and calculates the <c>NoOfPages</c>
    /// </summary>
    /// <param name="items"></param>
    public void SetItems(IEnumerable<T> items)
    {
        double count = items.Count();

        _items = items;
        NoOfPages = (int)Math.Ceiling(count / NoOfItemsPerPage);
    }
}
