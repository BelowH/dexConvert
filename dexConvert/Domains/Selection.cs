using dexConvert.Domains.ApiModels;

namespace dexConvert.Domains;

public class Selection
{

    public Selection(Guid mangaId, HashSet<Chapter> selectedChapters)
    {
        Dictionary<string, List<Guid>> chapterDictionary = new Dictionary<string, List<Guid>>();
        foreach (Chapter chapter in selectedChapters)
        {
            if (chapterDictionary.TryGetValue(chapter.Attributes.Volume ?? "unknown", out List<Guid>? chapterList))
            {
                chapterList!.Add(chapter.Id);
            }
            else
            {
                chapterDictionary.Add(chapter.Attributes.Volume ?? "unknown", new List<Guid>{chapter.Id});
            }
        }
        foreach (KeyValuePair<string, List<Guid>> keyValuePair in chapterDictionary)
        {
            Chapters.AddLast((keyValuePair.Key, keyValuePair.Value));
        }
        DataSaver = false;
        MangaId = mangaId;

    }
    
    
    public Guid MangaId { get; set; }
    
    public LinkedList<(string volume, List<Guid> chapters)> Chapters { get; set; } = new LinkedList<(string volume, List<Guid> chapters)>();

    public bool DataSaver { get; set; }
}