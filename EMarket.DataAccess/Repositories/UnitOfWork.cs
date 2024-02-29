using EMarket.DataAccess.Data;
using EMarket.Models;

namespace EMarket.DataAccess.Repositories;

public class UnitOfWork : IDisposable
{
    private bool _disposed = false;
    private readonly ApplicationDbContext _context;
    private GenericRepository<Product> _productRepository = default!;
    private GenericRepository<Category> _categoryRepository = default!;
    private GenericRepository<Purchase> _purchaseRepository = default!;
    private GenericRepository<Receiver> _receiverRepository = default!;
    private GenericRepository<ProductCategory> _productCategoryRepository = default!;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public GenericRepository<Product> ProductRepository
    {
        get
        {
            if (this._productRepository == null)
            {
                this._productRepository = new GenericRepository<Product>(_context);
            }

            return this._productRepository;
        }
    }

    public GenericRepository<Category> CategoryRepository
    {
        get
        {
            if (this._categoryRepository == null)
            {
                this._categoryRepository = new GenericRepository<Category>(_context);
            }

            return this._categoryRepository;
        }
    }

    public GenericRepository<Purchase> PurchaseRepository
    {
        get
        {
            if (this._purchaseRepository == null)
            {
                this._purchaseRepository = new GenericRepository<Purchase>(_context);
            }

            return this._purchaseRepository;
        }
    }

    public GenericRepository<Receiver> ReceiverRepository
    {
        get
        {
            if (this._receiverRepository == null)
            {
                this._receiverRepository = new GenericRepository<Receiver>(_context);
            }

            return this._receiverRepository;
        }
    }

    public GenericRepository<ProductCategory> ProductCategoryRepository
    {
        get
        {
            if (this._productCategoryRepository == null)
            {
                this._productCategoryRepository = new GenericRepository<ProductCategory>(_context);
            }

            return this._productCategoryRepository;
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
