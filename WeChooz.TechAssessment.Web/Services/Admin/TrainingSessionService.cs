using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Data;

namespace WeChooz.TechAssessment.Web.Services.Admin;

public sealed class TrainingSessionService : ITrainingSessionService
{
    private readonly FormationDbContext _dbContext;

    public TrainingSessionService(FormationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<TrainingSessionSummaryDto>> ListAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.TrainingSessions.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.StartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapSessionExpr)
            .ToListAsync(cancellationToken);

        return new PagedResult<TrainingSessionSummaryDto>(items, totalCount, page, pageSize);
    }

    public async Task<TrainingSessionSummaryDto?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.TrainingSessions
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(MapSessionExpr)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<TrainingSessionSummaryDto?> CreateAsync(TrainingSessionCreateRequest request, CancellationToken cancellationToken)
    {
        var courseExists = await _dbContext.Courses
            .AnyAsync(c => c.Id == request.CourseId, cancellationToken);

        if (!courseExists)
        {
            return null;
        }

        var session = new TrainingSession
        {
            CourseId = request.CourseId,
            StartDate = request.StartDate,
            DeliveryMode = Enum.Parse<DeliveryMode>(request.DeliveryMode, true),
            Location = request.Location,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        _dbContext.TrainingSessions.Add(session);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetAsync(session.Id, cancellationToken);
    }

    public async Task<TrainingSessionSummaryDto?> UpdateAsync(Guid id, TrainingSessionUpdateRequest request, CancellationToken cancellationToken)
    {
        var session = await _dbContext.TrainingSessions
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (session is null)
        {
            return null;
        }

        session.StartDate = request.StartDate;
        session.DeliveryMode = Enum.Parse<DeliveryMode>(request.DeliveryMode, true);
        session.Location = request.Location;
        session.UpdatedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetAsync(session.Id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var session = await _dbContext.TrainingSessions
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (session is null)
        {
            return false;
        }

        _dbContext.TrainingSessions.Remove(session);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static readonly Expression<Func<TrainingSession, TrainingSessionSummaryDto>> MapSessionExpr =
        session => new TrainingSessionSummaryDto(
            session.Id,
            session.CourseId,
            session.Course!.Name,
            session.StartDate,
            session.DeliveryMode.ToString(),
            session.Location,
            session.SessionParticipants.Count);

}
