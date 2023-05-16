using dexConvert.Domains.ApiModels;

namespace dexConvert.Repository;

public interface IApiRepository
{
    public Task<ApiCollectionResponse> GetManga(string title);
}