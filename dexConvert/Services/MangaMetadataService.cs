using dexConvert.Domains.ApiModels;
using dexConvert.Repository;

namespace dexConvert.Services;

public class MangaMetadataService : IMangaMetadataService
{

    private readonly IApiRepository _apiRepository;

    public MangaMetadataService(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }


    public async Task<List<ScanlationGroupResponse>> GetScanlationGroupsForFeed(FeedResponse feedResponse)
    {
        List<ScanlationGroupResponse> scanlationGroups = new List<ScanlationGroupResponse>();
        try
        {
            if (feedResponse.Data == null)
            {
                return scanlationGroups;
            }
            //for each chapter in feed get the relationship with type scanlation_group and get the id then filter out duplicates
            IEnumerable<string> scanlationGroupIdsEnumerable = feedResponse.Data
                .SelectMany(chapter => chapter.Relationships)
                .Where(relationship => relationship.Type == "scanlation_group")
                .Select(relationship => relationship.Id)
                .Distinct();
            Stack<string> scanlationGroupIds = new Stack<string>(scanlationGroupIdsEnumerable);

            int offset = 0;
            while (scanlationGroupIds.Count > 0)
            {
                List<string> ids = scanlationGroupIds.TakeWhile(
                        (_, index) => index <= 100 && index <= scanlationGroupIds.Count)
                    .ToList();
                ScanlationGroupCollectionResponse scanlationGroupCollectionResponse = await _apiRepository.GetScanlationGroups(ids, offset);
                if (scanlationGroupCollectionResponse.Data == null)
                {
                    continue;
                }
                scanlationGroups.AddRange(scanlationGroupCollectionResponse.Data);
                if (offset > scanlationGroupCollectionResponse.Total)
                {
                    break;
                }
                offset += 100;
                
            }
            return scanlationGroups;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return scanlationGroups;
        }
    }

    public async Task<Author?> GetAuthorByManga(Manga manga)
    {
        try
        {
            
            Guid? id = manga.Relationships?.FirstOrDefault(r => r.Type is "author")?.Id;
            if (id == null)
            {
                return null;
            }
            AuthorResponse authorResponse = await _apiRepository.GetAuthorById(id.Value);
            return authorResponse.Data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}