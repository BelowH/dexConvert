using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using dexConvert.Domains;
using dexConvert.Domains.ApiModels;

namespace dexConvert.Repository;

public class ApiRepository : IApiRepository
{

    private const string MangaEndpoint = "/manga";
    
    private readonly HttpClient _client;

    public ApiRepository()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Constants.BaseUrl);

    }
    
    public async Task<MangaSearchResponse> GetManga(string title, int offset = 0, int limit = 9)
    {
        try
        {
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query.Add("limit",limit.ToString());
            query.Add("offset",offset.ToString());
            query.Add("title",title);
            query.Add("includes[]", "cover_art");
            HttpResponseMessage response = await _client.GetAsync( MangaEndpoint +"?"+ query.ToString());
            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            MangaSearchResponse result = JsonSerializer.Deserialize<MangaSearchResponse>(responseContent)!;
            return result;
        }
        catch (HttpRequestException httpRequestException)
        {
            HttpStatusCode? statusCode = httpRequestException.StatusCode;
            throw;
        }
    }

    public async Task<FeedResponse> GetFeed(Guid mangaId,string lang, int offset = 0, bool deepSearch = false)
    {
        try
        {
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!deepSearch)
            {
                query.Add("translatedLanguage[]",lang); 
            }
            query.Add("offset",offset.ToString());
            HttpResponseMessage response = await _client.GetAsync(MangaEndpoint + "/" + mangaId + "/feed?" + query);
            response.EnsureSuccessStatusCode();
            
            string responseContent = await response.Content.ReadAsStringAsync();
            FeedResponse result = JsonSerializer.Deserialize<FeedResponse>(responseContent)!;
            return result;
        }
        catch (HttpRequestException httpRequestException)
        {
            HttpStatusCode? statusCode = httpRequestException.StatusCode;
            throw;
        }
    }
    

    public async Task<ChapterData> GetChapter(Guid chapterId, CancellationToken cancellationToken)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync("/at-home/server/" + chapterId, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            ChapterData result = JsonSerializer.Deserialize<ChapterData>(responseContent)!;
            return result;
        }
        catch (HttpRequestException httpRequestException)
        {
            HttpStatusCode? statusCode = httpRequestException.StatusCode;
            throw;
        }
    }

    public async Task<byte[]?> GetPage(string baseUrl, string hash, string page, CancellationToken cancellationToken)
    {
        try
        {
            using HttpClient pageClient = new HttpClient();
            pageClient.BaseAddress = new Uri(baseUrl);
            byte[] image = await pageClient.GetByteArrayAsync("/data/" + hash + "/" + page, cancellationToken);
            return image;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}