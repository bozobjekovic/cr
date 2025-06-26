# Company Registry Application

The Company Registry is a system for company information management. This application enables users to create, view, update, and search company records with advanced ISIN (International Securities Identification Number) validation to ensure data accuracy and uniqueness. The system provides secure access through user authentication, making it ideal for organizations that need to maintain accurate and validated company databases.

## 🚀 Quick Start

### Prerequisites

- Docker and Docker Compose
- .NET 8 SDK (for local development)
- Node.js 18+ (for frontend development)

### Using Docker Compose

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd company-registry
   ```

2. **Start all services**

   ```bash
   docker compose up --build
   ```

3. **Access the application**
   - **Frontend**: http://localhost:3000
   - **API**: http://localhost:8080
   - **Identity Server**: http://localhost:8081
   - **Database**: localhost:5432

### Running Components Separately

Each component can be run independently without Docker. Detailed setup instructions are available in each project's README:

- **Identity Server**: See `Company.Registry.Identity/README.md`
- **Web API**: See `Company.Registry/README.md`
- **Frontend Client**: See `Company.Registry.Client/README.md`

This approach is useful for development, debugging, or when you prefer to manage services individually.

### Services Architecture

The application consists of three main components:

- **Web API**: REST API for company operations using Clean Architecture with CQRS pattern and MediatR
- **Identity Server**: Authentication service providing OAuth 2.0/OpenID Connect using IdentityServer4
- **Client**: React application built with Vite and TypeScript, integrated with IdentityServer4 for authentication

## 🧪 Testing

The application includes both unit and integration tests using TestContainers for database testing.

### Test Structure

- **Unit Tests**: `Company.Registry/Application.UnitTests/`
- **Integration Tests**: `Company.Registry/Application.IntegrationTests/`

### TestContainers Integration

Integration tests use TestContainers to spin up real PostgreSQL containers for testing, ensuring tests run against actual database instances rather than mocks.

### Running Tests

#### Run All Tests

```bash
# Navigate to the solution directory
cd Company.Registry

# Run all tests
dotnet test
```

#### Run Unit Tests Only

```bash
cd Company.Registry
dotnet test Application.UnitTests/
```

#### Run Integration Tests Only

```bash
cd Company.Registry
dotnet test Application.IntegrationTests/
```

### Test Categories

**Unit Tests:**

- Command/Query handlers
- Validation logic
- Domain models
- Business rules

**Integration Tests:**

- End-to-end API workflows
- Database operations
- Authentication flows
- ISIN validation with database constraints
- Company CRUD operations

### TestContainers Requirements

TestContainers requires Docker to be running. Tests automatically manage PostgreSQL containers - starting them before execution and cleaning up afterward.

## 🏗️ Architecture Notes

**Main Web API Project**
The main Web API represents the core backend project, implementing Clean Architecture principles with CQRS pattern and MediatR for handling business operations.

**Identity Server Configuration**
The Identity Server implementation doesn't follow production-ready practices, as the focus of this project was on demonstrating the company registry functionality rather than building a production-grade identity solution. For easier setup and demonstration purposes, the identity provider uses **in-memory configuration** with IdentityServer4.

Key in-memory configurations (see `Company.Registry.Identity/Identity.API/Program.cs`):
- API Scopes and Resources
- Client configurations  
- Identity Resources
- Development signing credentials

**Database Architecture**
For simplicity and resource optimization, both the main Web API and Identity Server currently **share the same PostgreSQL database instance**. In a real-world scenario, these services would typically use separate databases for better isolation, security, and scalability.
