using System.Globalization;

namespace dexConvert.Services;

public class PreferenceService : IPreferenceService
{

    private static Dictionary<string, string> _supportedLangs = new Dictionary<string, string>()
    {
        {"gb", "English" },
        {"tw", "Simplified Chinese"},
        {"es", "Spanish"},
        {"jp", "Japanese"},
        {"kr", "Korean"},
        {"de", "German"}
    };

    private static Dictionary<string, List<string>> _alternatives = new Dictionary<string, List<string>>()
    {
        { "tw", new List<string>() { "zh-hk", "zh-ro", "zh" } },
        { "es", new List<string>() { "es", "es-la" } },
        { "jp", new List<string>() { "ja-ro", "ja", "jp" } },
        { "kr", new List<string>() { "ko-ro", "ko" } },
        { "gb", new List<string>() { "en" } },
    };

    
    
    
    private string _preferenceKey = "gb";
    
    private bool _deepSearch = false;

    public EventHandler<string>? OnPreferenceChanged { get; set; }

    public string GetPreferenceName() => _supportedLangs[_preferenceKey];

    public Dictionary<string, string> GetAvailableLanguages() => _supportedLangs;

    public void SetCulturePreference(string key)
    {
        _preferenceKey = key;
        OnPreferenceChanged?.Invoke(this, key);
    }

    public List<string> GetLangPreferenceForSearch()
    {
        List<string> langs = new List<string>();
        langs.AddRange(_alternatives[_preferenceKey]);
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
}