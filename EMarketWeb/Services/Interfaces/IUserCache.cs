namespace EMarketWeb.Services.Interfaces;

public interface IUserCache
{

    /// <summary>
    /// Obtains instance of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Service type</typeparam>
    /// <returns>Instance of type <typeparamref name="T"/></returns>
    T Get<T>() where T : ISessionService, new();

    /// <summary>
    /// Clears the values from user cache and the user cache itself
    /// </summary>
    void ClearUserCache();
}
