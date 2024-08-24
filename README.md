## Running locally

This project utilizes the devcontainers project.

### Prerequisites

- Docker
- Docker compose
- Your user using uid/gid 1000
- Vs code
- Devcontainers extension in vs code

### Running the application

1. Open the repo in vs code.
1. Re-open the the container.
1. The database starts automatically and you can use the vs tasks to run the application in debug mode.
1. The swagger UI is available on [https://0.0.0.0:5001](https://0.0.0.0:5001)

## EF migrations setup

**Adjust these for devcontainer**

dotnet ef migrations add <name>
dotnet ef database update
