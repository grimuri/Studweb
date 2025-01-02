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

INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'771a4b0a-e347-47e7-a794-b2347d8bfe6f', N'2024-09-28 14:51:37.667', N'2024-09-29 14:51:37.667', N'0');
INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'f7e86e55-9a8b-4271-8307-1d09129a3e9d', N'2024-09-28 14:52:47.623', N'2024-09-29 14:52:47.623', N'0');
INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'd7f73c22-b71c-4747-8d82-409f01dbdee6', N'2024-09-28 14:55:27.627', N'2024-09-29 14:55:27.627', N'0');
INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'6308abf9-4935-4085-9f1a-2b4f395087c8', N'2024-09-28 15:22:54.420', N'2024-09-29 15:22:54.420', N'0');
INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'a6416911-7915-41cb-b06d-1c6836aefcae', N'2024-09-28 15:26:25.540', N'2024-09-29 15:26:25.540', N'0');
INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'564926a1-adf5-4ac5-bfcd-94c61f03fd9e', N'2024-11-10 19:29:16.560', N'2024-11-11 19:29:16.560', N'0');
INSERT INTO StudwebDB.dbo.Tokens (Value, CreatedOnUtc, ExpiresOnUtc, Type) VALUES (N'3f5be13e-9449-4bca-8a49-bcb666c9dd5a', N'2024-12-25 13:15:50.367', N'2024-12-26 13:15:50.367', N'0');
