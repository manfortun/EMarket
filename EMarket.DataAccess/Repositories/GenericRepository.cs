using EMarket.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EMarket.DataAccess.Repositories;

public class GenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProp in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProp);
        }

        return query.ToList();
    }

    public virtual TEntity? FirstOrDefault(
        Expression<Func<TEntity, bool>> predicate,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet;

        foreach (var includeProp in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProp);
        }

        return query.FirstOrDefault(predicate);
    }

    public virtual int GetCount(Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return query.Count();
    }

    public virtual TEntity? ElementAtOrDefault(int index)
    {
        return _dbSet.ElementAtOrDefault(index);
    }

    public virtual TEntity? GetById(object id)
    {
        return _dbSet.Find(id);
    }

    public virtual void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Delete(object id)
    {
        TEntity? entity = GetById(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public virtual void Delete(TEntity entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}
