using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class LocalizedString
{
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? Values { get; set; }


    public string GetLocalizedTitle(CultureInfo cultureInfo)
    {
        if (Values == null)
        {
            return string.Empty;
        }
        
        if (Values.TryGetValue(cultureInfo.TwoLetterISOLanguageName,out JsonElement jsonElement))
        {
            return jsonElement.GetString() ?? string.Empty;
        }
        return Values.First().Value.GetString() ?? string.Empty;
    }
}
