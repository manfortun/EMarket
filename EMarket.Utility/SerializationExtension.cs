using Newtonsoft.Json;

namespace EMarket.Utility;

public static class SerializationExtension
{
    public static T FromJson<T>(this string value)
    {
        return JsonConvert.DeserializeObject<T>(value);
    }

    public static string ToJson<T>(this T value)
    {
        return JsonConvert.SerializeObject(value);
    }
}
