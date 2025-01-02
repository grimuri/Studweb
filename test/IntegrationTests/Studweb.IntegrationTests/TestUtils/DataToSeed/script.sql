CREATE DATABASE StudwebDB;

-- Table Roles

create table Roles
(
    Id   int identity
        primary key,
    Name varchar(100) not null
        unique
)
go

INSERT INTO Roles (Name) VALUES (N'User');


-- Table Tokens

create table Tokens
(
    Id           int identity
        primary key,
    Value        uniqueidentifier not null
        unique,
    CreatedOnUtc datetime         not null,
    ExpiresOnUtc datetime         not null,
    Type         varchar(max)     not null
)
go

INSERT INTO Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) 
    VALUES (N'771a4b0a-e347-47e7-a794-b2347d8bfe6f', N'2024-09-28 14:51:37.667', N'2024-09-29 14:51:37.667', N'0');

-- Table Users

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

INSERT INTO Users (FirstName, LastName, Email, Password, Birthday, CreatedOnUtc, VerifiedOnUtc, LastModifiedPasswordOnUtc, BanTime, VerificationTokenId, ResetPasswordTokenId, RoleId) 
    VALUES (N'FirstName', N'LastName', N'user@gmail.com', N'0D99D7B5C1D248F958B1F53A3C9F01AB59ED81EE12AB08CFCDC302F3E4FD9E08-374C5D26A1235817664E4626643EC48A', N'2000-09-28 13:27:41.807', N'2024-09-28 14:51:25.967', N'2024-09-28 14:51:25.967', null, null, 1, null, 1);


-- Table OutboxMessage

create table OutboxMessage
(
    Id             int identity
        primary key,
    Type           varchar(255) not null,
    Content        varchar(max) not null,
    OccuredOnUtc   datetime     not null,
    ProcessedOnUtc datetime,
    Error          varchar(max)
)
go


-- Table Tags

create table Tags
(
    Id   int identity
        primary key,
    Name varchar(255) not null
        unique
)
go

INSERT INTO Tags (Name) VALUES (N'tag1');
INSERT INTO Tags (Name) VALUES (N'tag2');
INSERT INTO Tags (Name) VALUES (N'tag3');


-- Table Notes
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

INSERT INTO Notes (Title, Content, CreatedOnUtc, LastModifiedOnUtc, UserId) VALUES (N'string', N'string', N'2024-12-26 10:13:56.720', N'2024-12-26 10:13:56.720', 1);


-- Table Notes_Tags

create table Notes_Tags
(
    NoteId int not null
        references Notes,
    TagId  int not null
        references Tags,
    primary key (NoteId, TagId)
)
go

INSERT INTO Notes_Tags (NoteId, TagId) VALUES (1, 1);
INSERT INTO Notes_Tags (NoteId, TagId) VALUES (1, 2);
INSERT INTO Notes_Tags (NoteId, TagId) VALUES (1, 3);