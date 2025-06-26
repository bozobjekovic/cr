# Company Registry Web API

The main REST API for company operations, built with Clean Architecture principles using CQRS pattern and MediatR.

## Prerequisites

- .NET 8 SDK
- PostgreSQL 15+
- Identity Server running (see `Company.Registry.Identity` project)

## Quick Start

### Development Setup

1. **Install dependencies:**
```bash
dotnet restore
```

2. **Start database:**
From the root directory, start the PostgreSQL database:
```bash
docker compose up database -d
```

3. **Run database migrations:**
```bash
dotnet ef database update --project Infrastructure --startup-project Web.API
```
*Note: In Development mode, migrations will be executed automatically on startup.*

4. **Start the API:**
```bash
cd Web.API
dotnet run
```

The API will be available at `http://localhost:5165`

## Testing

### Unit Tests
```bash
dotnet test Application.UnitTests/
```

### Integration Tests
Integration tests use TestContainers and require Docker to be running:
```bash
dotnet test Application.IntegrationTests/
```

## Architecture

The project follows Clean Architecture with:

- **Domain Layer**: Core business entities and rules
- **Application Layer**: Business logic, CQRS handlers, validation
- **Infrastructure Layer**: Data persistence and external services
- **Web.API Layer**: REST endpoints and middleware 