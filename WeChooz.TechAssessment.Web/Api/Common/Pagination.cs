namespace WeChooz.TechAssessment.Web.Api.Common;

public static class Pagination
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    public static (int Page, int PageSize) Normalize(int? page, int? pageSize)
    {
        var normalizedPage = page.GetValueOrDefault(DefaultPage);
        if (normalizedPage < 1)
        {
            normalizedPage = DefaultPage;
        }

        var normalizedSize = pageSize.GetValueOrDefault(DefaultPageSize);
        if (normalizedSize < 1)
        {
            normalizedSize = DefaultPageSize;
        }

        normalizedSize = Math.Min(normalizedSize, MaxPageSize);
        return (normalizedPage, normalizedSize);
    }
}
