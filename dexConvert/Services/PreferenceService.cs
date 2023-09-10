using System.Globalization;

namespace dexConvert.Services;

public class PreferenceService : IPreferenceService
{
    
    private CultureInfo _preference = CultureInfo.CurrentCulture;
    
    private bool _deepSearch = false;

    public EventHandler<CultureInfo>? OnPreferenceChanged { get; set; }

    public CultureInfo GetCulturePreference()
    {
        return _preference;
    }

    public List<CultureInfo> GetAvailableLanguages()
    {
        List<CultureInfo> cultureList = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        cultureList.Remove(CultureInfo.InvariantCulture);
        return cultureList;
    }

    public void SetCulturePreference(CultureInfo lang)
    {
        _preference = lang;
        OnPreferenceChanged?.Invoke(this, lang);
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