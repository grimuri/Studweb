create table Users
(
    Id                        int identity
        primary key,
    FirstName                 varchar(100) not null,
    LastName                  varchar(100) not null,
    Email                     varchar(250) not null
        unique,
    Password                  varchar(max) not null,
    Birthday                  datetime,
    CreatedOnUtc              datetime     not null,
    VerifiedOnUtc             datetime,
    LastModifiedPasswordOnUtc datetime,
    BanTime                   datetime,
    VerificationTokenId       int
        references Tokens,
    ResetPasswordTokenId      int
        references Tokens,
    RoleId                    int          not null
        references Roles
)
go

INSERT INTO StudwebDB.dbo.Users (FirstName, LastName, Email, Password, Birthday, CreatedOnUtc, VerifiedOnUtc, LastModifiedPasswordOnUtc, BanTime, VerificationTokenId, ResetPasswordTokenId, RoleId) VALUES (N'string', N'string', N'xowex78127@abatido.com', N'$2a$13$dvM1jMiGcyL6hV22ahEAputpx5PbryYGIh7WtcwZcTE14ACynetc.', N'2000-09-28 13:27:41.807', N'2024-09-28 14:51:25.967', null, null, null, null, null, 5);
INSERT INTO StudwebDB.dbo.Users (FirstName, LastName, Email, Password, Birthday, CreatedOnUtc, VerifiedOnUtc, LastModifiedPasswordOnUtc, BanTime, VerificationTokenId, ResetPasswordTokenId, RoleId) VALUES (N'string', N'string', N'jodavij921@abatido.com', N'$2a$13$TxQE/UQb.oj.zmEyooB/NOGlWMB9t2dmhUVpwT/mkT1nOupFcP73e', N'2000-09-28 13:27:41.807', N'2024-09-28 15:26:18.497', null, null, null, 5, null, 5);
INSERT INTO StudwebDB.dbo.Users (FirstName, LastName, Email, Password, Birthday, CreatedOnUtc, VerifiedOnUtc, LastModifiedPasswordOnUtc, BanTime, VerificationTokenId, ResetPasswordTokenId, RoleId) VALUES (N'Artur', N'Nowak', N'wefohec810@edectus.com', N'02414B6E8DC97015B6F1F099C6DCAC19C4094473E3CDB0A0CA1382F565547236-42F378195862BFB572CEEC20D5B35529', N'2000-11-11 00:00:00.000', N'2024-11-10 19:29:09.447', N'2024-11-10 19:29:09.447', null, null, 6, null, 5);
INSERT INTO StudwebDB.dbo.Users (FirstName, LastName, Email, Password, Birthday, CreatedOnUtc, VerifiedOnUtc, LastModifiedPasswordOnUtc, BanTime, VerificationTokenId, ResetPasswordTokenId, RoleId) VALUES (N'string', N'string', N'rofiv96855@kelenson.com', N'4FD0A85C30EF1859137F758DB8CF94FF1E7D8A8684CCCC1517F38263A43906CF-920A7B1F7472068920036343A684C003', N'2000-12-25 13:15:33.203', N'2024-12-25 13:15:49.377', N'2024-12-25 13:15:49.377', null, null, 7, null, 5);
