using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ScanlationGroupRequest
{
    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 10;

    [JsonPropertyName("offset")]
    public int Offset { get; set; } = 0;
    
    [JsonPropertyName("ids")]
    public List<string> Ids { get; set; } = new List<string>();
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("focusedLanguage")]
    public string? FocusedLanguage { get; set; }
    
    
}