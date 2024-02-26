using EMarketWeb.Services.Interfaces;

namespace EMarketWeb.Services;

public class UserCache : IUserCache
{
    private readonly Dictionary<Type, ISessionService> _userCache = new Dictionary<Type, ISessionService>();

    public void ClearUserCache()
    {
        foreach (var cache in _userCache.Values)
        {
            cache.Clear();
        }

        _userCache.Clear();
    }

    public ISessionService? Get(Type type)
    {
        return _userCache.ContainsKey(type) ? _userCache[type] : null;
    }

    public T Get<T>() where T : ISessionService, new()
    {
        if (!_userCache.ContainsKey(typeof(T)))
        {
            _userCache[typeof(T)] = new T();
        }
        return (T)_userCache[typeof(T)];
    }
}
