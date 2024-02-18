using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
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

    public async Task<FeedResponse> GetFeed(Guid mangaId,List<string> langs, int offset = 0, bool deepSearch = false)
    {
        try
        {
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!deepSearch)
            {
                foreach (string lang in langs)
                {
                    query.Add("translatedLanguage[]",lang);    
                }
            }
            query.Add("offset",offset.ToString());
            string endpoint = MangaEndpoint + "/" + mangaId.ToString("D") + "/feed?" + query;
            HttpResponseMessage response = await _client.GetAsync(endpoint);
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
        Stopwatch sw = new Stopwatch();
        string endpoint = "/data/" + hash + "/" + page;
        try
        {
            using HttpClient pageClient = new HttpClient();
            pageClient.BaseAddress = new Uri(baseUrl);
            
            sw.Start();
            HttpResponseMessage imageResponse  = await pageClient.GetAsync("/data/" + hash + "/" + page, cancellationToken);
            imageResponse.EnsureSuccessStatusCode();
            byte[] image = await imageResponse.Content.ReadAsByteArrayAsync(cancellationToken);
            sw.Stop();
            imageResponse.Headers.TryGetValues("X-Cache", out  IEnumerable<string>? cacheHeaders); 
            bool isCached  = cacheHeaders?.FirstOrDefault(s => s.Contains("HIT")) != null;
            ReportDownload(baseUrl + endpoint, sw.ElapsedMilliseconds, true, isCached, image.Length, cancellationToken);
            return image;
        }
        catch (Exception)
        {
            sw.Stop();
            ReportDownload(baseUrl + endpoint, sw.ElapsedMilliseconds, false, false, 0,
                cancellationToken);
            
            return null;
        }
    }

    public async Task<ScanlationGroupCollectionResponse> GetScanlationGroups(List<string> ids, int offset = 0, int limit = 100)
    {
        try
        {
            
            NameValueCollection query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query.Add("limit",limit.ToString());
            query.Add("offset",offset.ToString());
            foreach (string id in ids)
            {
                query.Add("ids[]",id);
            }
            HttpResponseMessage response = await _client.GetAsync("/group?" + query);
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ScanlationGroupCollectionResponse>(responseContent)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<AuthorResponse> GetAuthorById(Guid id)
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync($"/author/{id}");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AuthorResponse>(responseContent)!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async void ReportDownload(string url ,long timeInMs, bool success, bool isCached, int size, CancellationToken cancellationToken)
    {
        try
        {
            if (url.Contains("mangadex.org", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            
            Uri uri = new Uri(url);
            PageReport pageReport = new PageReport()
            {
                Url = url,
                Success = success,
                Bytes = size,
                Duration = timeInMs,
                Cached = isCached
            };
            
            using HttpClient reportClient = new HttpClient();
            Uri reportUri = new Uri(Constants.ReportBaseUrl);
            reportClient.BaseAddress = reportUri;
            HttpContent content = new StringContent(JsonSerializer.Serialize(pageReport));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await reportClient.PostAsync("/report", content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            Console.WriteLine(responseBody);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error while reporting page download" + e);
            //ignore
        }
    }
    
    
    public void Dispose()
    {
        _client.Dispose();
    }
}