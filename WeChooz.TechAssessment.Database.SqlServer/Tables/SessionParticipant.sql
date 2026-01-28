CREATE TABLE [dbo].[SessionParticipant] (
    [TrainingSessionId] UNIQUEIDENTIFIER NOT NULL,
    [ParticipantId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT [PK_SessionParticipant] PRIMARY KEY CLUSTERED ([TrainingSessionId], [ParticipantId]),
    CONSTRAINT [FK_SessionParticipant_TrainingSession] FOREIGN KEY ([TrainingSessionId]) REFERENCES [dbo].[TrainingSession]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SessionParticipant_Participant] FOREIGN KEY ([ParticipantId]) REFERENCES [dbo].[Participant]([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_SessionParticipant_ParticipantId] ON [dbo].[SessionParticipant] ([ParticipantId]);
GO
