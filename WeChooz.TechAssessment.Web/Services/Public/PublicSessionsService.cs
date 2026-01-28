using Markdig;
using Microsoft.EntityFrameworkCore;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Api.Public;
using WeChooz.TechAssessment.Web.Data;

namespace WeChooz.TechAssessment.Web.Services.Public;

public sealed class PublicSessionsService : IPublicSessionsService
{
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .DisableHtml()
        .Build();


    private readonly FormationDbContext _dbContext;

    public PublicSessionsService(FormationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<PublicSessionSummaryDto>> ListAsync(
        string? targetAudience,
        string? deliveryMode,
        DateOnly? startDateFrom,
        DateOnly? startDateTo,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.TrainingSessions.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(targetAudience) && Enum.TryParse<TargetAudience>(targetAudience, true, out var audienceValue))
        {
            query = query.Where(s => s.Course!.TargetAudience == audienceValue);
        }

        if (!string.IsNullOrWhiteSpace(deliveryMode) && Enum.TryParse<DeliveryMode>(deliveryMode, true, out var deliveryValue))
        {
            query = query.Where(s => s.DeliveryMode == deliveryValue);
        }

        if (startDateFrom.HasValue)
        {
            query = query.Where(s => s.StartDate >= startDateFrom.Value);
        }

        if (startDateTo.HasValue)
        {
            query = query.Where(s => s.StartDate <= startDateTo.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.StartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new PublicSessionSummaryDto(
                s.Id,
                s.Course!.Name,
                s.Course.ShortDescription,
                s.Course.TargetAudience.ToString(),
                s.StartDate,
                s.Course.DurationDays,
                s.DeliveryMode.ToString(),
                s.Course.MaxCapacity - s.SessionParticipants.Count,
                s.Course.TrainerFirstName,
                s.Course.TrainerLastName
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<PublicSessionSummaryDto>(items, totalCount, page, pageSize);
    }

    public async Task<PublicSessionDetailDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var session = await _dbContext.TrainingSessions
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new
            {
                s.Id,
                s.StartDate,
                DeliveryMode = s.DeliveryMode.ToString(),
                s.Course!.Name,
                s.Course.ShortDescription,
                s.Course.LongDescriptionMarkdown,
                TargetAudience = s.Course.TargetAudience.ToString(),
                s.Course.DurationDays,
                s.Course.MaxCapacity,
                ParticipantsCount = s.SessionParticipants.Count,
                s.Course.TrainerFirstName,
                s.Course.TrainerLastName
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (session is null)
        {
            return null;
        }

        var html = Markdown.ToHtml(session.LongDescriptionMarkdown, MarkdownPipeline);

        return new PublicSessionDetailDto(
            session.Id,
            session.Name,
            session.ShortDescription,
            session.TargetAudience,
            session.StartDate,
            session.DurationDays,
            session.DeliveryMode,
            session.MaxCapacity - session.ParticipantsCount,
            session.TrainerFirstName,
            session.TrainerLastName,
            session.LongDescriptionMarkdown,
            html
        );
    }
}
