using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ApiResponse
{
    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }
    
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}