# AIChatCore

A full-stack AI chat application built with **.NET 8 Web API** and **React (Vite)**, featuring OpenAI-powered conversations, session-based chat history, and a complete observability stack with **Serilog**, **Elasticsearch**, and **Kibana**. The entire stack is containerized with **Docker Compose** for one-command setup.

## Features

- 💬 AI-powered chat using the OpenAI API
- 🗂️ Session-based conversations with auto-generated titles
- 🔐 API key authentication middleware
- 🚦 Rate limiting on chat endpoints
- 📝 Centralized exception handling with structured logging
- 📊 Log aggregation via Elasticsearch, visualized in Kibana
- 🗄️ PostgreSQL persistence with EF Core (auto-applied migrations)
- 🐳 Fully containerized with Docker Compose

## Tech Stack

**Backend**
- .NET 8 Web API
- Entity Framework Core (PostgreSQL / Npgsql)
- Serilog (Console, File, Elasticsearch sinks)
- OpenAI .NET SDK
- Custom lightweight AutoMapper wrapper
- Layered architecture: `API` → `Business` → `DataAccess` → `Entities`, with a shared `Utilities` layer

**Frontend**
- React + Vite
- Axios
- Material UI

**Infrastructure**
- PostgreSQL 16
- Elasticsearch 8.13
- Kibana 8.13
- Nginx (serving the production React build)
- Docker Compose

## Project Structure

```
AIChatCore/
├── back-net-apı/
│   ├── API/                # Controllers, Program.cs, Kestrel & middleware config
│   ├── Business/           # Business logic (Managers)
│   ├── DataAccess/         # EF Core DbContext & Repositories
│   ├── DTO/                # Data transfer objects
│   ├── Entities/            # Domain models
│   ├── Utilities/           # Logging, exception handling, mapper, API key middleware
│   └── Dockerfile
├── front-react/
│   ├── src/
│   │   ├── api/             # Axios client configuration
│   │   ├── models/          # Session / message factory functions
│   │   └── page/            # React pages/components
│   └── Dockerfile
└── docker-compose.yml
```

## Architecture Overview

- **API layer** exposes REST endpoints and hosts cross-cutting middleware (exception handling, API key auth, CORS, rate limiting).
- **Business layer** contains the chat logic: building OpenAI requests, generating session titles, orchestrating repositories.
- **DataAccess layer** wraps EF Core `DbContext` and repository implementations.
- **Utilities layer** hosts shared, dependency-light infrastructure: the custom exception middleware, Serilog configuration, the mapper, and the API key middleware. It has no dependency on the Business layer to keep the dependency graph one-directional.
- Dependency injection is wired automatically via [Scrutor](https://github.com/khellang/Scrutor) assembly scanning, with explicit registrations for the mapper (singleton) and exception types (excluded from scanning).

## Getting Started

### Prerequisites

- [Docker](https://www.docker.com/) and Docker Compose
- An [OpenAI API key](https://platform.openai.com/api-keys)

### Environment Configuration

**Backend** — create `back-net-apı/API/appsettings.Development.json` (git-ignored) with your local secrets:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=AIChatCore;Username=postgres;Password=yourpassword;Port=5432"
  },
  "SecurityKey": {
    "ApiKey": "your-api-key-header-value"
  },
  "OpenAIKey": {
    "AIKey": "your-openai-api-key"
  }
}
```

For Docker, create `back-net-apı/API/appsettings.Docker.json` with container-network addresses (e.g. `http://elasticsearch:9200` instead of `localhost`).

**Frontend** — create `front-react/.env` (for local dev) and `front-react/.env.production` (for Docker/build):

```dotenv
VITE_API_BASE_URL=http://localhost:7077/api
VITE_API_KEY=your-api-key-header-value
```

### Run with Docker Compose

From the repository root:

```bash
docker compose up --build
```

This spins up:

| Service       | URL                          |
|---------------|-------------------------------|
| API           | `http://localhost:7077`       |
| Frontend      | `http://localhost:5173`       |
| PostgreSQL    | `localhost:5432`              |
| Elasticsearch | `http://localhost:9200`       |
| Kibana        | `http://localhost:5601`       |

Database migrations are applied automatically on API startup — no manual `dotnet ef database update` step required.

### Run locally (without Docker)

**Backend**
```bash
cd back-net-apı
dotnet restore
dotnet run --project API
```

**Frontend**
```bash
cd front-react
npm install
npm run dev
```

## Logging & Observability

All unhandled exceptions are caught by a global `ExceptionMiddleware`, logged via Serilog (console, file, and Elasticsearch sinks), and returned to the client as a consistent JSON error response — without leaking stack traces to the frontend.

Logs shipped to Elasticsearch can be explored in Kibana at `http://localhost:5601` under **Discover**, using the index pattern configured in `appsettings.json` (`Serilog:WriteTo:Elasticsearch:Args:indexFormat`).

## Security Notes

- API keys and connection strings are kept out of source control via `appsettings.Development.json` / `appsettings.Docker.json` and `.env` files (all git-ignored).
- Requests to `/api/*` require a valid `X-API-KEY` header, enforced by `ApiKeyMiddleware`.
- Chat endpoints are protected by fixed-window rate limiting to control OpenAI usage costs.
