using System.Text.Json.Serialization;

namespace dexConvert.Domains.ApiModels;

public class ChapterData
{

    [JsonPropertyName("result")]
    public string? Result { get; set; }
    
    [JsonPropertyName("baseUrl")]
    public string? BaseUrl { get; set; }
    
    [JsonPropertyName("chapter")]
    public Data? Chapter { get; set; }

    public class Data
    {
        [JsonPropertyName("hash")]
        public string? Hash { get; set; }
        
        [JsonPropertyName("data")]
        public List<string> ImageData { get; set; }  = new List<string>();
        
        [JsonPropertyName("dataSaver")]
        public List<string> ImageDataSaver { get; set; } = new List<string>();
    }
    
}
