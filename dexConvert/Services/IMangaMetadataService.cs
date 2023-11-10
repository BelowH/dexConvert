using dexConvert.Domains.ApiModels;

namespace dexConvert.Services;

public interface IMangaMetadataService
{
    
    public Task<List<ScanlationGroupResponse>> GetScanlationGroupsForFeed(FeedResponse feedResponse);
    
    
}