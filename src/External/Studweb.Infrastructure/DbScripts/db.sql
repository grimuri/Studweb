-- Creating Database StudwebDB

CREATE DATABASE StudwebDB;

USE StudwebDB;

-- Creating Table Roles
Create TABLE Roles (
                       Id integer  NOT NULL primary key,
                       Name varchar(100) NOT NULL unique
);

-- Insert example data to table Roles
Insert into Roles values (1, 'Administrator');