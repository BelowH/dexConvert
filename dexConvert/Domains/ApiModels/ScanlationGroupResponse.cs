using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;


public class ScanlationGroupCollectionResponse : ApiCollectionResponse
{
    [JsonPropertyName("data")]
    public new List<ScanlationGroupResponse>? Data { get; set; }
}


public class ScanlationGroupResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("attributes")]
    public ScanlationGroupAttributes? Attributes { get; set; }
}

public class ScanlationGroupAttributes
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("website")]
    public string? Website { get; set; }

    [JsonPropertyName("discord")]
    public string? Discord { get; set; }
    
}