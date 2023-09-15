using System.Collections.ObjectModel;
using dexConvert.Domains;
using dexConvert.Domains.ApiModels;
using dexConvert.Repository;

namespace dexConvert.Services;

public class MangaService : IMangaService
{
    private readonly IApiRepository _apiRepository;
    
    private readonly Dictionary<Guid,Manga> _mangaCache = new Dictionary<Guid, Manga>();
    
    private readonly Dictionary<Guid,Selection> _selectionCache = new Dictionary<Guid, Selection>();

    public MangaService(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }


    public async Task<(IList<Manga> manga, int total)> SearchMangaByTitle(string title, int offset)
    {
        MangaSearchResponse searchResult = await _apiRepository.GetManga(title, offset);
        foreach (Manga manga in searchResult.Data)
        {
            string? coverFileName = manga.Relationships?.First( r => r.Type!.Equals("cover_art")).Attributes?.GetElementValue<string>("fileName") ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(coverFileName))
            {
                manga.CoverLink = Constants.CoverBaseUrl + "/covers/" + manga.Id + "/" + coverFileName + ".512.jpg";
            }
            _mangaCache.TryAdd(manga.Id, manga);
        }
        return (searchResult.Data, searchResult.Total);
    }

    public Manga? GetFromCache(Guid id)
    {
        return _mangaCache.TryGetValue(id, out Manga? cache) ? cache : null;
    }
    
    public async Task<List<Chapter>> GetChapters(Guid mangaId, List<string> langs, bool deepSearch = false)
    {
        List<Chapter>? chapters = new List<Chapter>();
        int total = 1, offset = 0;
        while (offset <= total)
        {
            if (deepSearch)
            {
                await Task.Delay(250);
            }
            FeedResponse feedResponse = await _apiRepository.GetFeed(mangaId, langs, offset, deepSearch);
            total = feedResponse.Total;
            chapters.AddRange(feedResponse.Data ?? new List<Chapter>());
            offset += 100;
        }
        return chapters;
    }
    
    public Guid AddSelection( Selection selection)
    {
        Guid id = Guid.NewGuid();
        _selectionCache.TryAdd(id, selection);
        return id;
    }
    
    public Selection? GetSelection(Guid id)
    {
        return _selectionCache.TryGetValue(id, out Selection? selection) ? selection : null;
    }
    
    
} 