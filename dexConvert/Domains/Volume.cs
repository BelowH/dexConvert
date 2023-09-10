using dexConvert.Domains.ApiModels;

namespace dexConvert.Domains;

public class Volume
{

    public Volume(string volumeId)
    {
        VolumeId = volumeId;
        ChapterCollection = new Dictionary<Guid, ChapterCollection?>();
    }
    
    public void Add(Chapter chapter)
    {
        if (ChapterCollection.TryGetValue(chapter.Attributes.Uploader, out ChapterCollection? chapterCollection))
        {
            chapterCollection!.Chapters.Add(chapter);
        }else
        {
            ChapterCollection.Add(chapter.Attributes.Uploader, new ChapterCollection{TranslatorId = chapter.Attributes.Uploader, Chapters = new List<Chapter>{chapter}});
        }
    }
    
    public string VolumeId { get; set; }

    public Dictionary<Guid, ChapterCollection?> ChapterCollection { get; set; } 
    
}