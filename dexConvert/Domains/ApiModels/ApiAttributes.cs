using System.Text.Json;
using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ApiAttributes
{
    [JsonExtensionData]
    public Dictionary<string, JsonElement> Elements { get; set; } = new Dictionary<string, JsonElement>();
    
    
    public T? GetElementValue<T>(string key)
    {
        return Elements.TryGetValue(key, out JsonElement element) ? element.Deserialize<T>() : default;
    }
    
    
}