using System.Globalization;

namespace dexConvert.Services;

public class PreferenceService : IPreferenceService
{
    
    
    private static readonly Dictionary<string, List<string>> Alternatives = new Dictionary<string, List<string>>()
    {
        { "zh", new List<string>() { "zh-hk", "zh-ro", "zh" } },
        { "es", new List<string>() { "es", "es-la" } },
        { "jp", new List<string>() { "ja-ro", "ja", "jp" } },
        { "kr", new List<string>() { "ko-ro", "ko" } },
        { "en", new List<string>() { "gb" } },
    };


    private readonly Dictionary<string, string> _cultures = new Dictionary<string, string>()
    {
        {"zh", "Chinese"},
        {"es", "Spanish"},
        {"en", "English"},
        {"hi", "Hindi"},
        {"bn", "Bengali"},
        {"pt", "Portuguese"},
        {"ru", "Russian"},
        {"ja", "Japanese"},
        {"pa", "Punjabi"},
        {"mr", "Marathi"},
        {"te", "Telugu"},
        {"tr", "Turkish"},
        {"ko", "Korean"},
        {"fr", "French"},
        {"de", "German"},
    };
    
    private string _preferenceKey = "en";
    
    private bool _deepSearch = false;
    public EventHandler<string>? OnPreferenceChanged { get; set; }

    public Dictionary<string, string> GetAvailableLanguages() => _cultures;

    public void SetCulturePreference(string key)
    {
        _preferenceKey = key;
        OnPreferenceChanged?.Invoke(this, key);
    }

    public List<string> GetLangPreferenceForSearch()
    {
        List<string> langs = new List<string> { _preferenceKey };
        if (Alternatives.TryGetValue(_preferenceKey, out List<string>? alternative))
        {
            langs.AddRange(alternative);
        }
        return langs;
    }

    public void SetDeepSearch(bool deepSearch)
    {
        _deepSearch = deepSearch;
    }
    
    public bool GetDeepSearch()
    {
        return _deepSearch;
    }
    
    public List<string> GetSelectedLanguage()
    {
        List<string> list = new List<string> { _cultures.TryGetValue(_preferenceKey, out string? lang) ? lang : "English" };
        return list;
    }
}