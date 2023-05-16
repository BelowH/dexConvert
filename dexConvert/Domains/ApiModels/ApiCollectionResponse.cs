using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ApiCollectionResponse
{
    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonPropertyName("response")]
    public string Response { get; set; }

    [JsonPropertyName("data")]
    public IList<ApiData> Data { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}