using Newtonsoft.Json;

namespace EMarket.Utility;

public static class SerializationExtension
{
    /// <summary>
    /// Converts a JSON string into object of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="value">JSON string</param>
    /// <returns>Object of type <typeparamref name="T"/></returns>
    public static T FromJson<T>(this string value)
    {
        return JsonConvert.DeserializeObject<T>(value);
    }

    /// <summary>
    /// Converts an object into JSON string
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="value">Object of type <typeparamref name="T"/></param>
    /// <returns>JSON string</returns>
    public static string ToJson<T>(this T value)
    {
        return JsonConvert.SerializeObject(value);
    }
}
