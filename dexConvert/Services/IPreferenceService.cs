using System.Globalization;

namespace dexConvert.Services;

public interface IPreferenceService
{
    
    public EventHandler<CultureInfo> OnPreferenceChanged { get; set; }

    public CultureInfo GetCulturePreference();
    
    public List<CultureInfo> GetAvailableLanguages();

    public void SetCulturePreference(CultureInfo lang);
    
    public void SetDeepSearch(bool deepSearch);
    
    public bool GetDeepSearch();
}