# Studweb

<!-- TOC -->
- [Description](#description)
- [Features](#features)
- [Project Structure](#project-structure)
- [Database](#database)
  - [Tables](#tables)
  - [Relationships](#relationships)
- [Technologies](#technologies)
- [Requirements](#requirements)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
  - [Register User](#register-user)
  - [Login User](#login-user)
  - [Create Note](#create-note)
  - [Get All Notes](#get-all-notes)
  - [Get Note](#get-note)
  - [Update Note](#update-note)
  - [Delete Note](#delete-note)
- [Roadmap](#roadmap)
<!-- /TOC -->

## Description
Studweb is a social platform designed specifically for students, enabling them to connect, collaborate, and share knowledge. The application provides tools for managing academic resources, creating and sharing notes, and engaging in discussions. With features tailored to the needs of students, Studweb fosters a collaborative environment that supports learning and community building.

**SWAGGER:** [LINK](https://studwebapi.kindsky-22e1b9a5.polandcentral.azurecontainerapps.io/swagger/index.html)

***Please note that the application may take a moment to start in the cloud environment. This is due to the initialization of services and resources required for proper operation. Thank you for your patience!***

## Features
- **User Management**:
  - Register and authenticate users.
  - Send confirmation emails for account verification.
- **Note Management**:
  - Create, read, update, and delete notes.

## Project Structure

<p align="center">
  <img src="./assets/project_structure.png" alt="Logo firmy" />
</p>

- `Studweb.Domain` - Contains domain logic, including aggregates, value objects, and primitives.
- `Studweb.Application` - Handles application logic, including use case processing.
- `Studweb.Infrastructure` - Implements data access, including SQL scripts and database configuration.
- `Studweb.Api` - Exposes APIs for the user interface.
- `Studweb.IntegrationTests` - Contains integration tests for the application.
- `Studweb.Application.UnitTests` - Contains unit tests for the application layer.
- `Studweb.Domain.UnitTests` - Contains unit tests for the domain layer.
- `Studweb.Infrastructure.UnitTests` - Contains unit tests for the infrastructure layer.


## Database

![Database diagram](./assets/db_diagram.png)

The application uses a MSSQL Server database to store its data. Below is an overview of the main tables and their purposes:

### Tables

- **`Roles`**:
  - Stores user roles in the application.
  - **Columns**: `Id`, `Name`.

- **`Tokens`**:
  - Stores tokens for user verification and password reset.
  - **Columns**: `Id`, `Value`, `CreatedOnUtc`, `ExpiresOnUtc`, `Type`.

- **`Users`**:
  - Stores user information and credentials.
  - **Columns**: `Id`, `FirstName`, `LastName`, `Email`, `Password`, `Birthday`, `CreatedOnUtc`, `VerifiedOnUtc`, `LastModifiedPasswordOnUtc`, `BanTime`, `VerificationTokenId`, `ResetPasswordTokenId`, `RoleId`.

- **`OutboxMessage`**:
  - Stores messages for domain events.
  - **Columns**: `Id`, `Type`, `Content`, `OccuredOnUtc`, `ProcessedOnUtc`, `Error`.

- **`Tags`**:
  - Stores tags that can be associated with notes.
  - **Columns**: `Id`, `Name`.

- **`Notes`**:
  - Stores user-created notes.
  - **Columns**: `Id`, `Title`, `Content`, `CreatedOnUtc`, `LastModifiedOnUtc`, `UserId`.

- **`Notes_Tags`**:
  - Stores relationships between notes and tags.
  - **Columns**: `NoteId`, `TagId`.

### Relationships
- `Users` table references `Roles` and `Tokens` tables for user roles and token management.
- `Notes` table references `Users` table to associate notes with their creators.
- `Notes_Tags` table establishes a many-to-many relationship between `Notes` and `Tags`.

## Technologies
- **.NET 7.0**
- **MSSQL Server**
- **Dapper**
- **Newtonsoft.Json**
- **MediatR**
- **ErrorOr**
- **FluentValidation**
- **Docker** (for containerization)
- **xUnit** (for unit testing)
- **Moq** (for mocking dependencies)
- **FluentAssertions** (for assertions in tests)

## Requirements
- **.NET SDK 7.0**
- **MSSQL Server** for database operations.
- **Docker** for containerization (optional, but recommended for development).

## Getting Started
To set up and run the project using the provided configuration, follow these steps:

1. **Clone the repository to your local machine:**
    
```bash
git clone https://github.com/grimuri/Studweb.git
cd Studweb
```

2. **Configure appsettings.json**

   Update the appsettings.json file located in src/Api/Studweb.Api with the required values:

```json
{
  "ConnectionStrings": {
    "Default": "your-connection-string"
  },
  "Email": {
    "SenderEmail": "your-email@example.com",
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "your-smtp-username",
    "Password": "your-smtp-password"
  }
}
```

3. **Prepare the database:**
   - Open SQL Server Management Studio (SSMS) and create a new database named `Studweb`.
   - Run the SQL scripts located in `src/Infrastructure/Studweb.Infrastructure/Database/SQL` to create the necessary tables and relationships.
    - Alternatively, you can use the provided docker-compose file to run the database in a container. Make sure to adjust the connection string in `appsettings.json` accordingly.
   

4. **Build and run the application:**

   Use the following commands to restore dependencies, build, and run the application:

```bash
dotnet restore ./Studweb.sln
dotnet build ./Studweb.sln --configuration Release
dotnet run --project ./src/Api/Studweb.Api/Studweb.Api.csproj
```

5. **Access the API:**
   - The API will be available at `http://localhost:5028/api/`.


6. **Test the API**

    You can use tools like Postman or curl to test the endpoints.

## Api Documentation

### Register User

#### Register User Request

```http
POST /api/register
```

```json
{
  "firstName": "Joe",
  "lastName": "Doe",
  "email": "joe.doe@gmai.com",
  "password": "Qwerty1234!",
  "confirmPassword": "Qwerty1234!",
  "birthday": "2000-01-01"
}
```

#### Register User Response

```http
200 OK
```

```json
{
  "id": 1,
  "message": "Successfully registered! Check your email!"
}
```

### Login User

#### Login User Request

```http
POST /api/login
```

```json
{
  "email": "joe.doe@gmail.com",
  "password": "Qwerty1234!"
}
```

#### Login User Response

```http
200 OK
```

```json
{
  "id": 1,
  "firstName": "Joe",
  "lastName": "Doe",
  "email": "joe.doe@gmail.com",
  "birthday": "2000-01-01T00:00:00.0",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjgiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJzdHJpbmcxMjNzQGdtYWlsLmNvbSIsImV4cCI6MTc0NDU2MzE4OSwiaXNzIjoiaXNzdWVyIiwiYXVkIjoiYXVkaWVuY2UifQ.cQJ35PJZIUZlBN35tNaTJ3KSGQpMU0xD7-hsOxzmTOE"
}
```

### Create Note

#### Create Note Request

```http
POST /api/note
```

```json
{
  "title": "Fibonacci equation",
  "content": "Fn = a1bn1+a2bn2",
  "tags": [
    "Math", "Fibonacci"
  ]
}
```

#### Create Note Response

```http
200 OK
```

```json
{
  "id": 1,
  "message": "Successfully added note!"
}
```

### Get All Notes

#### Get All Notes Request

```http
GET /api/note
```

#### Get All Notes Response

```http
200 OK
```

```json
{
  "notes": [
    {
      "id": 1,
      "title": "Fibonacci equation",
      "content": "Fn = a1bn1+a2bn2",
      "createdOnUtc": "2025-04-13T15:57:03.317",
      "lastModifiedOnUtc": "2025-04-13T15:57:03.317"
    },
    {
      "id": 2,
      "title": "Fibonacci equation2",
      "content": "Fn = a1bn1+a2bn2",
      "createdOnUtc": "2025-03-13T15:57:03.317",
      "lastModifiedOnUtc": "2025-03-13T15:57:03.317"
    }
  ]
}
```

### Get Note 

#### Get Note Request

```http
GET /api/note/{noteId}
```

#### Get Note Response

```http
200 OK
```

```json
{
  "id": 1,
  "title": "Fibonacci equation",
  "content": "Fn = a1bn1+a2bn2",
  "createdOnUtc": "2025-04-13T15:57:03.317",
  "lastModifiedOnUtc": "2025-04-13T15:57:03.317",
  "tags": [
    "Math",
    "Fibonacci"
  ]
}
```

### Update Note

#### Update Note Request

```http
PUT /api/note/{noteId}
```

```json
{
  "title": "Equation Fibonacci",
  "content": "Fn = a1bn1+a2bn2",
  "tags": [
    "Maths",
    "Fibo"
  ]
}
```

#### Update Note Response

```http
200 OK
```

```json
{
  "id": 1,
  "title": "Equation Fibonacci",
  "content": "Fn = a1bn1+a2bn2",
  "createdOnUtc": "2025-04-13T15:57:03.317",
  "lastModifiedOnUtc": "2025-04-13T16:06:51.4611863Z",
  "tags": [
    "Maths",
    "Fibo"
  ]
}
```

### Delete Note

#### Delete Note Request

```http
DELETE /api/note/{noteId}
```

#### Delete Note Response

```http
200 OK
```

```json
{
  "id": 1,
  "message": "Note successfully deleted"
}
```


## Roadmap

- **Authentication and Authorization**:
  - ✔️ JWT-based authentication for secure API access.
  - ✔️ Role-based authorization for different user roles (e.g., admin, user).
  - ✔️ Login and registration endpoints


- **Note Management**:
  - ✔️ Create, read, update, and delete notes.
  - ✔️ Tagging system for notes (many-to-many relationship).
  - ❌ Search functionality for notes by title or content.
  - ❌ Pagination for note retrieval.


- **User Management**:
  - ❌ Edit user profiles (profile picture, contact details, description).
  - ❌ Password reset functionality.
  - ❌ Email verification.
  - ❌ User permissions management (e.g., administrators, moderators).


- **Social Features**:
  - ❌ Create thematic groups (e.g., subjects, projects).
  - ❌ Discussion forums for groups and general topics.
  - ❌ Private messaging system between users.
  - ❌ Notification system (e.g., for new messages, comments, group invitations).
  - ❌ Add friends and build a network of contacts. 


- **Content Management**:
  - ❌ Share notes with other users or groups.
  - ❌ Commenting and rating notes.
  - ❌ Versioning system for notes (change history).
  - ❌ Search functionality for notes and tags.


- **Educational Features**:
  - ❌ Create and manage events (e.g., meetings, workshops, exams).
  - ❌ Calendar integrated with events and deadlines.
  - ❌ Quiz and test system for groups.
  - ❌ Repository for educational materials (e.g., PDF files, presentations).


- **Integrations**:
  - ❌ Integration with external platforms (e.g., Google Calendar, Microsoft Teams).

