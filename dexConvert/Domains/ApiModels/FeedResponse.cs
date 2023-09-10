using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class FeedResponse : ApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }
    
    [JsonPropertyName("data")]
    public IList<Chapter>? Data { get; set; }
}