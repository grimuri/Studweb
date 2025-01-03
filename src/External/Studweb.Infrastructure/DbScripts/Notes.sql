create table Notes
(
    Id                int identity
        primary key,
    Title             varchar(255) not null,
    Content           varchar(max) not null,
    CreatedOnUtc      datetime     not null,
    LastModifiedOnUtc datetime     not null,
    UserId            int          not null
        references Users
)
go

INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-26 10:13:56.720', N'2024-12-26 10:13:56.720', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-26 10:15:35.033', N'2024-12-26 10:15:35.033', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-26 10:21:00.440', N'2024-12-26 10:21:00.440', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-26 10:22:32.617', N'2024-12-26 10:22:32.617', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-26 10:23:36.413', N'2024-12-26 10:23:36.413', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-27 13:57:28.020', N'2024-12-27 13:57:28.020', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-27 13:58:23.233', N'2024-12-27 13:58:23.233', 10);
INSERT INTO StudwebDB.dbo.Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-27 14:04:29.110', N'2024-12-27 14:04:29.110', 10);
