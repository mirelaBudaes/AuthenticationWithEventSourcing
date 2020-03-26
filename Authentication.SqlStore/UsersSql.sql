
if not exists (select * from sysobjects where name='Users' and xtype='U')

CREATE TABLE [dbo].[Users] (
    [UserId]          UNIQUEIDENTIFIER NOT NULL,
    [Email]           NVARCHAR (MAX)   NOT NULL,
    [EmailIsVerified] BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC)
);

ALTER TABLE Users
ADD CreatedDate DATETIME NOT NULL DEFAULT (GETDATE());

ALTER TABLE Users
ADD LastUpdatedDate DATETIME NULL;

