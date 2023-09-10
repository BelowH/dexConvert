using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class Relationships
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("related")]
    public string? Related { get; set; }
    
    [JsonPropertyName("attributes")] 
    public ApiAttributes? Attributes { get; set; }
}