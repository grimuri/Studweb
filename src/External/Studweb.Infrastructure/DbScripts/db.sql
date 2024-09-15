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
                       Name varchar(100) NOT NULL,
                       Surname varchar(100) NOT NULL,
                       Email varchar(250) NOT NULL unique,
                       Password text NOT NULL,
                       Birthday datetime NULL,
                       CreatedAt datetime NOT NULL,
                       VerifiedAt datetime NULL,
                       VerificationToken text NULL,
                       VerificationTokenExpires datetime NULL,
                       ResetPasswordToken text NULL,
                       ResetPasswordTokenExpires datetime NULL,
                       BanTime datetime NULL,
                       RoleId integer NOT NULL FOREIGN KEY REFERENCES Roles(Id),
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
