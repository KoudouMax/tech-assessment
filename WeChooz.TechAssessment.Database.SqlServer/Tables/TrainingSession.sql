CREATE TABLE [dbo].[TrainingSession] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [CourseId] UNIQUEIDENTIFIER NOT NULL,
    [StartDate] DATE NOT NULL,
    [DeliveryMode] NVARCHAR(20) NOT NULL, -- 'PRESENTIEL' | 'DISTANCIEL'
    [Location] NVARCHAR(200) NULL,
    [CreatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT [PK_TrainingSession] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_TrainingSession_Course] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Course]([Id]) ON DELETE CASCADE,
    CONSTRAINT [CK_TrainingSession_DeliveryMode] CHECK ([DeliveryMode] IN ('PRESENTIEL', 'DISTANCIEL'))
);
GO

CREATE INDEX [IX_TrainingSession_CourseId] ON [dbo].[TrainingSession] ([CourseId]);
GO

CREATE INDEX [IX_TrainingSession_StartDate] ON [dbo].[TrainingSession] ([StartDate]);
GO
