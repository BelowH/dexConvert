using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class PageReport
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("bytes")]
    public int Bytes { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }

    [JsonPropertyName("cached")]
    public bool Cached { get; set; }
}