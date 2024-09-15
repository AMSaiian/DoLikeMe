# MyCutePet
--------
## To run with docker (recommended)
You have to clarify that your machine has got next software:
- Docker
- Docker Compose
- Git
### Steps:
1. To clone this repository, run the following command:
```bash
git clone https://github.com/AMSaiian/MyCutePet.git
```
2. Navigate to the repository backend source code folder
``` bash
cd MyCutePet/backend
```
3. Copy .env.example as .env at the same folder - it must be exactly near *docker.compose.yml*. All generic environment variables have been configured, however you can change it with your own
4. Run predefined console script basing on your OS:
``` bash
# Windows (using Powershell at MyCutePet/backend):
.\run-with-docker.ps1
```
``` bash
# Linux (using sh at MyCutePet/backend):
chmod +x ./run_with_docker.sh # use this line if you encountered issues with permissions
./run-with-docker.sh
```

## To run locally
You have to clarify that your machine has got next software:
- .NET Core 8
- PostgreSQL 16
- Git
### Steps:
1. To clone this repository, run the following command:
```bash
git clone https://github.com/AMSaiian/MyCutePet.git
```
2. Navigate to the repository backend source code folder
``` bash
cd MyCutePet/backend
```
3. Fill next placeholders in configuration using appsetting.json or .NET user secrets:
- *TokenProvider -> Secret*
- *ConnectionStrings -> Auth* 
- *ConnectionString -> Application* (can be similar with Auth)
4. Run next commands using Powershell or Command line:
```bash
dotnet restore Task.io.sln
dotnet build Task.io.sln --configuration Release --no-restore
dotnet run --project src/App/Taskio/Taskio.csproj --no-build
```

## Since now you can access backend using next URIs (if configuration hasn't been changed):
- Swagger UI (HTTPS): https://localhost:9081/swagger/index.html 
- Swagger UI (HTTP): http://localhost:9080/swagger/index.html !!! due to Swagger UI can't handle redirect, use HTTPS
- The PostgreSQL database is accessible on port 5433 (using Docker run) or
The PostgreSQL database is accessible on port 5432 (using local run)
------------
## Generic user flow:
0. Use pre-created user with next credentials:
- Identifier: WellKnownUser
- Password: 12345678Ab!
1. Register a user providing a valid email and username e.g. *NewUser*. Password must have length from 8 to 20 symbols and contain one digit, one uppercase letter and one special symbol e.g. *!@$*. Save returned user id, which you have to use to query tasks and etc.
2. Obtain a token at the token endpoint using credentials from 0 or 1 step. Add the token as bearer token e.g. at *Swagger UI*
3. If you start with pre-created credentials and don't have user id - access api/users *GET* endpoint with token
4. When you have *user id* you can create, update, delete and query your task. 
5. Create, Update, Delete actions are trivial so I would like tell about *api/user/{userId}/tasks* endpoint:
- By default it paginated with amount provided in configuration. Using query params you can define by what property tasks will be sorted; amount of tasks; direction of order. You can order, filter and rangie(by providing start and end value) tasks simultaneously.
- Available order properties: 
-- title
-- priority
-- status
-- dueDate
-- createdDate
-- updatedDate
- Available filter properties: 
-- priority
-- status
-- dueDate
-- createdDate
-- updatedDate
- Available ranger properties: 
-- dueDate
-- createdDate
-- updatedDate
------------
## Solution Setup
### Architecture - Onion Architecture
### Storage - EF Core (Code First) via PostgreSQL
### Application - CQRS with MediatoR
### Auth - Custom Identity provider using Microsoft ASP .NET Core Identity
### Unit and Integration testing - xUnit using Moq, Fluent Assertions, Bogus and TestContainers.
------------
## Implemented:
- #### Cross-cuttings:
1. Structured logging using Serilog with sensitive data masking
2. Validation pipelines
3. Errors and exceptions handlers/filters
4. Enhanced solution structure divided on necessary assemblies
- #### Testing
1. Integration testing using docker containers for database per every test class and seeding.
2. Unit testing using InMemory DbContext and Moq Mocks.
