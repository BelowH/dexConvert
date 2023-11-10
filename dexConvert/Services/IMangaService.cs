using dexConvert.Domains;
using dexConvert.Domains.ApiModels;

namespace dexConvert.Services;

public interface IMangaService
{
    public Task<(IList<Manga> manga, int total)> SearchMangaByTitle(string title, int offset);
    
    public Manga? GetFromCache(Guid id);

    public Task<FeedResponse> GetChapters(Guid mangaId, List<string> langs, bool deepSearch = false);

    public Guid AddSelection(Selection selection);

    public Selection? GetSelection(Guid id);

}