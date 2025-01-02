create table Tags
(
    Id   int identity
        primary key,
    Name varchar(255) not null
        unique
)
go

INSERT INTO StudwebDB.dbo.Tags (Name) VALUES (N'string');
INSERT INTO StudwebDB.dbo.Tags (Name) VALUES (N'string1');
INSERT INTO StudwebDB.dbo.Tags (Name) VALUES (N'string2');
