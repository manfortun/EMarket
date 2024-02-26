namespace EMarketWeb.Services.Interfaces;

public interface IUserCache
{
    T Get<T>() where T : ISessionService, new();
    void ClearUserCache();
}
