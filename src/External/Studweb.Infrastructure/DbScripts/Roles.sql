create table Roles
(
    Id   int identity
        primary key,
    Name varchar(100) not null
        unique
)
go

INSERT INTO StudwebDB.dbo.Roles (Name) VALUES (N'User');
