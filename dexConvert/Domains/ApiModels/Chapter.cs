using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class Chapter
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("attributes")]
    public ChapterAttributes Attributes { get; set; }
}

public class ChapterAttributes
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("volume")]
    public string? Volume { get; set; }

    [JsonPropertyName("chapter")]
    public string? Chapter { get; set; }
    
    [JsonPropertyName("translatedLanguage")]
    public string? TranslatedLanguage { get; set; }
    
    [JsonPropertyName("uploader")] 
    public Guid Uploader { get; set; }

    [JsonPropertyName("externalUrl")]
    public string ExternalUrl { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("uploadedAt")]
    public string? UploadedAt { get; set; }

    [JsonPropertyName("publishedAt")]
    public string? PublishedAt { get; set; }

    [JsonPropertyName("readableAt")]
    public string? ReadableAt { get; set; }
    
}