using dexConvert.Domains.ApiModels;

namespace dexConvert.Helper;

public static class ChapterExtensions
{
    public static Dictionary<string, List<Chapter>> GetChaptersByLanguage(this List<Chapter> chapters)
    {
        Dictionary<string, List<Chapter>> chapterDictionary = new Dictionary<string, List<Chapter>>();
        foreach (Chapter chapter in chapters)
        {
            string lang = chapter.Attributes.TranslatedLanguage ?? "Unknown";
            if (chapterDictionary.TryGetValue(lang, out List<Chapter>? value))
            {
                value.Add(chapter);
            }
            else
            {
                chapterDictionary.Add(lang, new List<Chapter>{chapter});
            }
        }

        return chapterDictionary;   
    }
}