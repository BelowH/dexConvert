using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace dexConvert.Domains.ApiModels;

public class Manga : ApiData
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public ApiEnumCollection.MediaType Type { get; set; }

    [JsonPropertyName("attributes")]
    public MangaAttributes? Attributes { get; set; }


}

public class MangaAttributes
{
    [JsonPropertyName("title")]
    [JsonExtensionData]
    public IDictionary<string, string> Title { get; set; } = new Dictionary<string, string>();

    [JsonPropertyName("altTitles")]
    [JsonExtensionData]
    public IDictionary<string, string> AltTitles { get; set; } = new Dictionary<string, string>();
    
    [JsonPropertyName("description")]
    [JsonExtensionData]
    public IDictionary<string, string> Description { get; set; } = new Dictionary<string, string>();

    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; set; }

    [JsonPropertyName("originalLanguage")] 
    public string OriginalLanguage { get; set; } = string.Empty;

    [JsonPropertyName("lastVolume")]
    public string? LastVolume { get; set; }
    
    [JsonPropertyName("lastChapter")]
    public string? LastChapter { get; set; }

    [JsonPropertyName("publicationDemographic")]
    public ApiEnumCollection.PublicationDemographic PublicationDemographic { get; set; }
    
    [JsonPropertyName("status")]
    public ApiEnumCollection.Status Status { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("contentRating")]
    public ApiEnumCollection.ContentRating ContentRating { get; set; }

    [JsonPropertyName("chapterNumbersResetOnNewVolume")]
    public bool ChapterNumbersResetOnNewVolume { get; set; }

    [JsonPropertyName("latestUploadedChapter")]
    public Guid LatestUploadedChapter { get; set; }
    
    [JsonPropertyName("state")]
    public ApiEnumCollection.State State { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("createdAt")] 
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; } = string.Empty;

}