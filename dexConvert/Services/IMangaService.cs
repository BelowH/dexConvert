using dexConvert.Domains;
using dexConvert.Domains.ApiModels;

namespace dexConvert.Services;

public interface IMangaService
{
    public Task<(IList<Manga> manga, int total)> SearchMangaByTitle(string title, int offset);
    
    public Manga? GetFromCache(Guid id);

    public Task<List<Chapter>> GetChapters(Guid mangaId, string lang, bool deepSearch = false);

    public Guid AddSelection(Selection selection);

    public Selection? GetSelection(Guid id);

}