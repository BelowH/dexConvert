using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class FeedResponse : ApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }
    
    [JsonPropertyName("data")]
    public List<Chapter>? Data { get; set; }

    [JsonIgnore]
    public int Filtered { get; set; } = 0;
    
    [JsonIgnore]
    public int Duplicate { get; set; } = 0;
}