using System.Globalization;

namespace dexConvert.Services;

public interface IPreferenceService
{
    
    public EventHandler<string> OnPreferenceChanged { get; set; }
    
    public Dictionary<string, string> GetAvailableLanguages();

    public void SetCulturePreference(string lang);

    public List<string> GetLangPreferenceForSearch();
    
    public void SetDeepSearch(bool deepSearch);
    
    public bool GetDeepSearch();
}