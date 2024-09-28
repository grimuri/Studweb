-- Creating Database StudwebDB

CREATE DATABASE StudwebDB;

USE StudwebDB;

-- Creating Table Roles
Create TABLE Roles (
                       Id integer  NOT NULL IDENTITY primary key,
                       Name varchar(100) NOT NULL unique
);

-- Creating Table User
Create TABLE Users (
                       Id integer  NOT NULL IDENTITY primary key,
                       FirstName varchar(100) NOT NULL,
                       LastName varchar(100) NOT NULL,
                       Email varchar(250) NOT NULL unique,
                       Password varchar(max) NOT NULL,
                       Birthday datetime NULL,
                       CreatedOnUtc datetime NOT NULL,
                       VerifiedOnUtc datetime NULL,                       
                       LastModifiedPasswordOnUtc datetime NULL,
                       BanTime datetime NULL,
                       VerificationTokenId integer NULL FOREIGN KEY REFERENCES Tokens(Id),
                       ResetPasswordTokenId integer NULL FOREIGN KEY REFERENCES Tokens(Id),
                       RoleId integer NOT NULL FOREIGN KEY REFERENCES Roles(Id),
);

Create TABLE Tokens (
                       Id integer NOT NULL IDENTITY primary key,
                       Value uniqueidentifier NOT NULL unique,
                       CreatedOnUtc datetime NOT NULL,
                       ExpiresOnUtc datetime NOT NULL,
                       Type varchar(max) NOT NULL,
);

-- Creating Table Outbox_Message
Create TABLE OutboxMessage (
                       Id integer NOT NULL IDENTITY primary key,
                       Type varchar(255) NOT NULL,
                       Content varchar(max) NOT NULL,
                       OccuredOnUtc datetime NOT NULL,
                       ProcessedOnUtc datetime NULL,
                       Error varchar(max) NULL,
);

-- Insert example data to table Roles
Insert into Roles values (1, 'Administrator');
