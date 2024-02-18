using System.Collections.ObjectModel;
using dexConvert.Domains;
using dexConvert.Domains.ApiModels;
using dexConvert.Repository;
using MudBlazor.Utilities;

namespace dexConvert.Services;

public class MangaService : IMangaService
{
    private readonly IApiRepository _apiRepository;
    
    private readonly Dictionary<Guid,Manga> _mangas = new Dictionary<Guid, Manga>();
    
    private readonly Dictionary<Guid,Selection> _selection = new Dictionary<Guid, Selection>();

    public MangaService(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }


    public async Task<(IList<Manga> manga, int total)> SearchMangaByTitle(string title, int offset)
    {
        _mangas.Clear();
        MangaSearchResponse searchResult = await _apiRepository.GetManga(title, offset);
        foreach (Manga manga in searchResult.Data)
        {
            string? coverFileName = manga.Relationships?.FirstOrDefault( r => r.Type!.Equals("cover_art"))?.Attributes?.GetElementValue<string>("fileName") ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(coverFileName))
            {
                manga.CoverLink = Constants.CoverBaseUrl + "/covers/" + manga.Id + "/" + coverFileName + ".512.jpg";
            }
            _mangas.TryAdd(manga.Id, manga);
        }
        return (searchResult.Data, searchResult.Total);
    }

    public Manga? GetFromCache(Guid id)
    {
        return _mangas.GetValueOrDefault(id);
    }
    
    public async Task<FeedResponse> GetChapters(Guid mangaId, List<string> langs, bool deepSearch = false)
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
        chapters = FilterChapters(chapters, out int numFiltered, out int duplicateCount);
        FeedResponse result = new FeedResponse
        {
            Data = chapters,
            Total = chapters.Count,
            Filtered = numFiltered,
            Duplicate = duplicateCount
        };
        return result;
    }

    public List<Manga>? GetSearch()
    {
        return !_mangas.Any() ? null : _mangas.Values.ToList();
    }

    public Guid AddSelection( Selection selection)
    {
        Guid id = Guid.NewGuid();
        _selection.TryAdd(id, selection);
        return id;
    }
    

    private List<Chapter> FilterChapters(List<Chapter> chapters, out int numFiltered, out int duplicateCount)
    {
        numFiltered = 0;
        duplicateCount = 0;
        Dictionary<string,Chapter> filteredChapters = new Dictionary<string, Chapter>();
        foreach (Chapter chapter in chapters)
        {
            if (chapter.Attributes == null || chapter.Attributes.Pages == 0 || !string.IsNullOrWhiteSpace(chapter.Attributes.ExternalUrl))
            {
                numFiltered++;
                continue;
            }
            string? id = chapter.Attributes?.Chapter;
            if (string.IsNullOrWhiteSpace(id))
            {
                id = Guid.NewGuid().ToString();
            }
            if (filteredChapters.ContainsKey(id))
            {
                id = Guid.NewGuid().ToString();
                duplicateCount++;
            }
            filteredChapters.Add(id,chapter);
           
        }
        return filteredChapters.Values.ToList();
    }
    
} 