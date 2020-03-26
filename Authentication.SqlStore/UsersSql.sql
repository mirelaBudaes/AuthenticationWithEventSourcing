CREATE TABLE [dbo].[Users] (
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [Email]           NVARCHAR (MAX)   NOT NULL,
    [EmailIsVerified] BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);