using dexConvert.Domains.ApiModels;

namespace dexConvert.Repository;

public interface IApiRepository : IDisposable
{
    public Task<MangaSearchResponse> GetManga(string title, int offset = 0, int limit = 9);
    
    public Task<FeedResponse> GetFeed(Guid mangaId, string lang, int offset = 0, bool deepSearch = false);
    
    public Task<ChapterData> GetChapter(Guid chapterId, CancellationToken cancellationToken);
    
    public Task<byte[]?> GetPage(string baseUrl, string hash, string page, CancellationToken cancellationToken);

}