create table Notes_Tags
(
    NoteId int not null
        references Notes,
    TagId  int not null
        references Tags,
    primary key (NoteId, TagId)
)
go

INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (4, 2);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (5, 2);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (5, 3);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (1002, 2);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (1003, 2);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (1003, 1002);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (1004, 2);
INSERT INTO StudwebDB.dbo.Notes_Tags (NoteId, TagId) VALUES (1004, 1002);
