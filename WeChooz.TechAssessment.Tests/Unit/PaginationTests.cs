using WeChooz.TechAssessment.Web.Api.Common;

namespace WeChooz.TechAssessment.Tests.Unit;

public class PaginationTests
{
    [Fact]
    public void NormalizeUsesDefaultsWhenNull()
    {
        var (page, pageSize) = Pagination.Normalize(null, null);
        Assert.Equal(1, page);
        Assert.Equal(20, pageSize);
    }

    [Fact]
    public void NormalizeClampsToMinimum()
    {
        var (page, pageSize) = Pagination.Normalize(0, 0);
        Assert.Equal(1, page);
        Assert.Equal(20, pageSize);
    }

    [Fact]
    public void NormalizeClampsToMaximum()
    {
        var (page, pageSize) = Pagination.Normalize(1, 1000);
        Assert.Equal(1, page);
        Assert.Equal(100, pageSize);
    }
}
