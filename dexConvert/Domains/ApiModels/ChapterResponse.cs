using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ChapterResponse : ApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }
    
    [JsonPropertyName("data")]
    public Chapter? Data { get; set; }
}

