using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;


public class AuthorResponse : ApiResponse
{
    [JsonPropertyName("data")]
    public Author Data { get; set; }
    
}

public class Author
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("attributes")]
    public AuthorAttributes Attributes { get; set; }
    
}

public class AuthorAttributes
{
    
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("biography")]
    public LocalizedString Biography { get; set; }

    [JsonPropertyName("twitter")]
    public string? Twitter { get; set; }

    [JsonPropertyName("pixiv")]
    public string? Pixiv { get; set; }

    [JsonPropertyName("melonBook")]
    public string? MelonBook { get; set; }

    [JsonPropertyName("fanBox")]
    public string? FanBox { get; set; }

    [JsonPropertyName("booth")]
    public string? Booth { get; set; }

    [JsonPropertyName("nicoVideo")]
    public string? NicoVideo { get; set; }

    [JsonPropertyName("skeb")]
    public string? Sekb { get; set; }

    [JsonPropertyName("fanita")]
    public string? Fanita { get; set; }

    [JsonPropertyName("tumblr")]
    public string? Tumblr { get; set; }

    [JsonPropertyName("youtube")]
    public string? Youtube { get; set; }

    [JsonPropertyName("weibo")]
    public string? Weibo { get; set; }

    [JsonPropertyName("naver")]
    public string? Naver { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }
    
}