using dexConvert.Domains.ApiModels;

namespace dexConvert.Domains;

public class ChapterCollection
{
    
    public Guid TranslatorId { get; set; }
    
    public List<Chapter> Chapters { get; set; } = new List<Chapter>();
    
}