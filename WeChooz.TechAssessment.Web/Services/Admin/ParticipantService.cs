using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using WeChooz.TechAssessment.Web.Api.Admin;
using WeChooz.TechAssessment.Web.Api.Common;
using WeChooz.TechAssessment.Web.Data;

namespace WeChooz.TechAssessment.Web.Services.Admin;

public sealed class ParticipantService : IParticipantService
{
    private readonly FormationDbContext _dbContext;
    private readonly ILogger<ParticipantService> _logger;

    public ParticipantService(FormationDbContext dbContext, ILogger<ParticipantService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PagedResult<ParticipantSummaryDto>?> ListBySessionAsync(Guid sessionId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.TrainingSessions
            .AsNoTracking()
            .AnyAsync(s => s.Id == sessionId, cancellationToken);

        if (!exists)
        {
            return null;
        }

        var query = _dbContext.SessionParticipants
            .AsNoTracking()
            .Where(sp => sp.TrainingSessionId == sessionId)
            .OrderBy(sp => sp.Participant!.LastName)
            .ThenBy(sp => sp.Participant!.FirstName);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(sp => new ParticipantSummaryDto(
                sp.Participant!.Id,
                sp.Participant.FirstName,
                sp.Participant.LastName,
                sp.Participant.Email,
                sp.Participant.CompanyName
            ))
            .ToListAsync(cancellationToken);

        return new PagedResult<ParticipantSummaryDto>(items, totalCount, page, pageSize);
    }

    public async Task<ParticipantSummaryDto?> AddToSessionAsync(Guid sessionId, AddParticipantRequest request, CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var session = await _dbContext.TrainingSessions
            .Include(s => s.Course)
            .SingleOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

        if (session is null || session.Course is null)
        {
            return null;
        }

        var participant = await _dbContext.Participants
            .SingleOrDefaultAsync(p => p.Email == request.Email, cancellationToken);

        if (participant is null)
        {
            participant = new Participant
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CompanyName = request.CompanyName,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            _dbContext.Participants.Add(participant);
        }
        else
        {
            participant.FirstName = request.FirstName;
            participant.LastName = request.LastName;
            participant.CompanyName = request.CompanyName;
            participant.UpdatedAtUtc = DateTime.UtcNow;
        }

        var alreadyAdded = await _dbContext.SessionParticipants
            .AnyAsync(sp => sp.TrainingSessionId == sessionId && sp.ParticipantId == participant.Id, cancellationToken);

        if (!alreadyAdded)
        {
            var currentCount = await _dbContext.SessionParticipants
                .CountAsync(sp => sp.TrainingSessionId == sessionId, cancellationToken);

            if (currentCount >= session.Course.MaxCapacity)
            {
                return null;
            }

            _dbContext.SessionParticipants.Add(new SessionParticipant
            {
                TrainingSessionId = sessionId,
                ParticipantId = participant.Id,
                CreatedAtUtc = DateTime.UtcNow
            });
        }

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Failed to add participant {Email} to session {SessionId}.", request.Email, sessionId);
            await transaction.RollbackAsync(cancellationToken);
            return null;
        }

        return new ParticipantSummaryDto(
            participant.Id,
            participant.FirstName,
            participant.LastName,
            participant.Email,
            participant.CompanyName
        );
    }

    public async Task<bool> RemoveFromSessionAsync(Guid sessionId, Guid participantId, CancellationToken cancellationToken)
    {
        var entry = await _dbContext.SessionParticipants
            .SingleOrDefaultAsync(sp => sp.TrainingSessionId == sessionId && sp.ParticipantId == participantId, cancellationToken);

        if (entry is null)
        {
            return false;
        }

        _dbContext.SessionParticipants.Remove(entry);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
