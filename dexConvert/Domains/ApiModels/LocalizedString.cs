using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class LocalizedString
{
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? Values { get; set; }


    public string GetLocalizedTitle(List<string> langs)
    {
        if (Values == null)
        {
            return string.Empty;
        }

        try
        {
            foreach (string lang in langs)
            {
                if (Values.TryGetValue(lang,out JsonElement jsonElement))
                {
                    return jsonElement.GetString() ?? string.Empty;
                }
            }
            return Values.First().Value.GetString() ?? string.Empty;
        }
        catch (Exception e)
        {
            // ignore
            return "";
        }
    }
}
