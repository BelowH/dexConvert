using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ApiErrorResponse
{
    [JsonPropertyName("result")]
    public string Result { get; set; } = "error";

    [JsonPropertyName("error")]
    public Error Error { get; set; }
    
}

public class Error
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("detail")]
    public string? Detail { get; set; }
    
}