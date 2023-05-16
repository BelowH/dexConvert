namespace dexConvert.Domains.ApiModels;

public static class ApiEnumCollection
{
    public enum MediaType
    {
        manga,
    }

    public enum PublicationDemographic
    {
        shounen,
        shoujo,
        josei,
        seinen
    }

    public enum Status
    {
        completed,
        ongoing,
        cancelled,
        hiatus
    }

    public enum ContentRating
    {
        safe,
        suggestive,
        erotica,
        pornographic
    }

    public enum State
    {
        draft,
        submitted,
        published,
        rejected
    }

}