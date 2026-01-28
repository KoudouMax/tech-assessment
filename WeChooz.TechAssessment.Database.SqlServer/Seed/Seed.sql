DECLARE @Course1 UNIQUEIDENTIFIER;
DECLARE @Course2 UNIQUEIDENTIFIER;
DECLARE @Course3 UNIQUEIDENTIFIER;

IF NOT EXISTS (SELECT 1 FROM [dbo].[Course] WHERE [Name] = N'Formation Dialogue Social – Bases')
BEGIN
    SET @Course1 = NEWID();
    INSERT INTO [dbo].[Course] (
        [Id],
        [Name],
        [ShortDescription],
        [LongDescriptionMarkdown],
        [DurationDays],
        [TargetAudience],
        [MaxCapacity],
        [TrainerFirstName],
        [TrainerLastName]
    )
    VALUES
    (
        @Course1,
        N'Formation Dialogue Social – Bases',
        N'Comprendre les fondamentaux du dialogue social.',
        N'# Objectifs

- Comprendre le rôle du CSE
- Maîtriser les bases de la communication sociale

## Programme

- Introduction
- Cas pratiques',
        2,
        N'ELU',
        12,
        N'Claire',
        N'Durand'
    );
END
ELSE
BEGIN
    SELECT @Course1 = [Id] FROM [dbo].[Course] WHERE [Name] = N'Formation Dialogue Social – Bases';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Course] WHERE [Name] = N'Présider un CSE efficacement')
BEGIN
    SET @Course2 = NEWID();
    INSERT INTO [dbo].[Course] (
        [Id],
        [Name],
        [ShortDescription],
        [LongDescriptionMarkdown],
        [DurationDays],
        [TargetAudience],
        [MaxCapacity],
        [TrainerFirstName],
        [TrainerLastName]
    )
    VALUES
    (
        @Course2,
        N'Présider un CSE efficacement',
        N'Piloter un CSE et faciliter les échanges.',
        N'# Objectifs

- Organiser les réunions
- Anticiper les conflits

## Programme

- Gouvernance
- Médiation',
        3,
        N'PRESIDENT',
        10,
        N'Romain',
        N'Petit'
    );
END
ELSE
BEGIN
    SELECT @Course2 = [Id] FROM [dbo].[Course] WHERE [Name] = N'Présider un CSE efficacement';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Course] WHERE [Name] = N'Atelier Négociation')
BEGIN
    SET @Course3 = NEWID();
    INSERT INTO [dbo].[Course] (
        [Id],
        [Name],
        [ShortDescription],
        [LongDescriptionMarkdown],
        [DurationDays],
        [TargetAudience],
        [MaxCapacity],
        [TrainerFirstName],
        [TrainerLastName]
    )
    VALUES
    (
        @Course3,
        N'Atelier Négociation',
        N'Exercices pratiques de négociation.',
        N'# Objectifs

- Préparer une négociation
- Conduire une discussion

## Programme

- Techniques
- Jeux de rôle',
        1,
        N'ELU',
        8,
        N'Alice',
        N'Nguyen'
    );
END
ELSE
BEGIN
    SELECT @Course3 = [Id] FROM [dbo].[Course] WHERE [Name] = N'Atelier Négociation';
END

DECLARE @Session1 UNIQUEIDENTIFIER;
DECLARE @Session2 UNIQUEIDENTIFIER;
DECLARE @Session3 UNIQUEIDENTIFIER;

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[TrainingSession]
    WHERE [CourseId] = @Course1
      AND [StartDate] = DATEADD(day, 10, CAST(GETDATE() AS DATE))
      AND [DeliveryMode] = N'PRESENTIEL'
)
BEGIN
    SET @Session1 = NEWID();
    INSERT INTO [dbo].[TrainingSession] (
        [Id],
        [CourseId],
        [StartDate],
        [DeliveryMode],
        [Location]
    )
    VALUES
    (
        @Session1,
        @Course1,
        DATEADD(day, 10, CAST(GETDATE() AS DATE)),
        N'PRESENTIEL',
        N'Paris'
    );
END
ELSE
BEGIN
    SELECT @Session1 = [Id]
    FROM [dbo].[TrainingSession]
    WHERE [CourseId] = @Course1
      AND [StartDate] = DATEADD(day, 10, CAST(GETDATE() AS DATE))
      AND [DeliveryMode] = N'PRESENTIEL';
END

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[TrainingSession]
    WHERE [CourseId] = @Course2
      AND [StartDate] = DATEADD(day, 25, CAST(GETDATE() AS DATE))
      AND [DeliveryMode] = N'DISTANCIEL'
)
BEGIN
    SET @Session2 = NEWID();
    INSERT INTO [dbo].[TrainingSession] (
        [Id],
        [CourseId],
        [StartDate],
        [DeliveryMode],
        [Location]
    )
    VALUES
    (
        @Session2,
        @Course2,
        DATEADD(day, 25, CAST(GETDATE() AS DATE)),
        N'DISTANCIEL',
        NULL
    );
END
ELSE
BEGIN
    SELECT @Session2 = [Id]
    FROM [dbo].[TrainingSession]
    WHERE [CourseId] = @Course2
      AND [StartDate] = DATEADD(day, 25, CAST(GETDATE() AS DATE))
      AND [DeliveryMode] = N'DISTANCIEL';
END

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[TrainingSession]
    WHERE [CourseId] = @Course3
      AND [StartDate] = DATEADD(day, 40, CAST(GETDATE() AS DATE))
      AND [DeliveryMode] = N'PRESENTIEL'
)
BEGIN
    SET @Session3 = NEWID();
    INSERT INTO [dbo].[TrainingSession] (
        [Id],
        [CourseId],
        [StartDate],
        [DeliveryMode],
        [Location]
    )
    VALUES
    (
        @Session3,
        @Course3,
        DATEADD(day, 40, CAST(GETDATE() AS DATE)),
        N'PRESENTIEL',
        N'Lyon'
    );
END
ELSE
BEGIN
    SELECT @Session3 = [Id]
    FROM [dbo].[TrainingSession]
    WHERE [CourseId] = @Course3
      AND [StartDate] = DATEADD(day, 40, CAST(GETDATE() AS DATE))
      AND [DeliveryMode] = N'PRESENTIEL';
END

DECLARE @Participant1 UNIQUEIDENTIFIER;
DECLARE @Participant2 UNIQUEIDENTIFIER;
DECLARE @Participant3 UNIQUEIDENTIFIER;

IF NOT EXISTS (SELECT 1 FROM [dbo].[Participant] WHERE [Email] = N'julien.martin@example.com')
BEGIN
    SET @Participant1 = NEWID();
    INSERT INTO [dbo].[Participant] (
        [Id],
        [FirstName],
        [LastName],
        [Email],
        [CompanyName]
    )
    VALUES
    (
        @Participant1,
        N'Julien',
        N'Martin',
        N'julien.martin@example.com',
        N'Acme'
    );
END
ELSE
BEGIN
    SELECT @Participant1 = [Id] FROM [dbo].[Participant] WHERE [Email] = N'julien.martin@example.com';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Participant] WHERE [Email] = N'marie.dupont@example.com')
BEGIN
    SET @Participant2 = NEWID();
    INSERT INTO [dbo].[Participant] (
        [Id],
        [FirstName],
        [LastName],
        [Email],
        [CompanyName]
    )
    VALUES
    (
        @Participant2,
        N'Marie',
        N'Dupont',
        N'marie.dupont@example.com',
        N'Globex'
    );
END
ELSE
BEGIN
    SELECT @Participant2 = [Id] FROM [dbo].[Participant] WHERE [Email] = N'marie.dupont@example.com';
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Participant] WHERE [Email] = N'sarah.lopez@example.com')
BEGIN
    SET @Participant3 = NEWID();
    INSERT INTO [dbo].[Participant] (
        [Id],
        [FirstName],
        [LastName],
        [Email],
        [CompanyName]
    )
    VALUES
    (
        @Participant3,
        N'Sarah',
        N'Lopez',
        N'sarah.lopez@example.com',
        N'Initech'
    );
END
ELSE
BEGIN
    SELECT @Participant3 = [Id] FROM [dbo].[Participant] WHERE [Email] = N'sarah.lopez@example.com';
END

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[SessionParticipant]
    WHERE [TrainingSessionId] = @Session1 AND [ParticipantId] = @Participant1
)
BEGIN
    INSERT INTO [dbo].[SessionParticipant] ([TrainingSessionId], [ParticipantId])
    VALUES (@Session1, @Participant1);
END

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[SessionParticipant]
    WHERE [TrainingSessionId] = @Session1 AND [ParticipantId] = @Participant2
)
BEGIN
    INSERT INTO [dbo].[SessionParticipant] ([TrainingSessionId], [ParticipantId])
    VALUES (@Session1, @Participant2);
END

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[SessionParticipant]
    WHERE [TrainingSessionId] = @Session2 AND [ParticipantId] = @Participant3
)
BEGIN
    INSERT INTO [dbo].[SessionParticipant] ([TrainingSessionId], [ParticipantId])
    VALUES (@Session2, @Participant3);
END
