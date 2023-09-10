using System.Text.Json.Serialization;
using Microsoft.VisualBasic;

namespace dexConvert.Domains.ApiModels;



public class MangaSearchResponse : ApiResponse
{

    [JsonPropertyName("result")]
    public string? Result { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }
    
    [JsonPropertyName("data")]
    public IList<Manga> Data { get; set; } = new List<Manga>();
}

public class Manga 
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("attributes")]
    public MangaAttributes? Attributes { get; set; }
    
    [JsonPropertyName("relationships")]
    public List<Relationships>? Relationships { get; set; }

    [JsonIgnore]
    public string? CoverLink { get; set; }
    
}

public class MangaAttributes
{
    [JsonPropertyName("title")]
    public LocalizedString? Title { get; set; }

    [JsonPropertyName("altTitles")]
    public List<LocalizedString>? AltTitles { get; set; }
    
    [JsonPropertyName("description")]
    public LocalizedString? Description { get; set; } 

    [JsonPropertyName("isLocked")]
    public bool IsLocked { get; set; }

    [JsonPropertyName("originalLanguage")] 
    public string OriginalLanguage { get; set; } = string.Empty;

    [JsonPropertyName("lastVolume")]
    public string? LastVolume { get; set; }
    
    [JsonPropertyName("lastChapter")]
    public string? LastChapter { get; set; }

    [JsonPropertyName("publicationDemographic")]
    public string? PublicationDemographic { get; set; }
    
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("year")]
    public int? Year { get; set; }

    [JsonPropertyName("contentRating")]
    public string? ContentRating { get; set; }

    [JsonPropertyName("chapterNumbersResetOnNewVolume")]
    public bool ChapterNumbersResetOnNewVolume { get; set; }

    [JsonPropertyName("latestUploadedChapter")]
    public Guid? LatestUploadedChapter { get; set; }
    
    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("version")]
    public int Version { get; set; }

    [JsonPropertyName("createdAt")] 
    public string CreatedAt { get; set; } = string.Empty;

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; } = string.Empty;
    
    

}
