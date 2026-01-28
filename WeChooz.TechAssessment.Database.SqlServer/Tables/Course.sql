CREATE TABLE [dbo].[Course] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [Name] NVARCHAR(200) NOT NULL,
    [ShortDescription] NVARCHAR(500) NOT NULL,
    [LongDescriptionMarkdown] NVARCHAR(MAX) NOT NULL,
    [DurationDays] INT NOT NULL,
    [TargetAudience] NVARCHAR(20) NOT NULL, -- 'ELU' | 'PRESIDENT'
    [MaxCapacity] INT NOT NULL,
    [TrainerFirstName] NVARCHAR(100) NOT NULL,
    [TrainerLastName] NVARCHAR(100) NOT NULL,
    [CreatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [CK_Course_DurationDays] CHECK ([DurationDays] > 0),
    CONSTRAINT [CK_Course_MaxCapacity] CHECK ([MaxCapacity] > 0),
    CONSTRAINT [CK_Course_TargetAudience] CHECK ([TargetAudience] IN ('ELU', 'PRESIDENT'))
);
GO

CREATE INDEX [IX_Course_TargetAudience] ON [dbo].[Course] ([TargetAudience]);
GO
