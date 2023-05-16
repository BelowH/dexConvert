using System.Net;
using System.Text.Json;
using dexConvert.Domains;
using dexConvert.Domains.ApiModels;

namespace dexConvert.Repository;

public class ApiRepository : IApiRepository, IDisposable
{

    private const string MANGA_ENDPOINT = "/manga";
    
    private readonly HttpClient _client;

    public ApiRepository()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Constants.BASE_URL);

    }
    
    public async Task<ApiCollectionResponse> GetManga(string title)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(MANGA_ENDPOINT + $"&title={title}");
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiCollectionResponse>(responseContent)!;
        }
        catch (HttpRequestException httpRequestException)
        {
            HttpStatusCode? statusCode = httpRequestException.StatusCode;
            throw;
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}