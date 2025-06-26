# Company Registry Identity Server

OAuth 2.0/OpenID Connect authentication server providing secure token-based authentication for the Company Registry system.

## Prerequisites

- .NET 8 SDK
- PostgreSQL 15+

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
dotnet ef database update --project Identity.API
```
*Note: In Development mode, migrations will be executed automatically on startup.*

4. **Start the Identity Server:**
```bash
cd Identity.API
dotnet run
```

The Identity Server will be available at `http://localhost:5133`

 