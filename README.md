# Task Manager — Workspace Overview & Quick Start

This workspace contains two small full-stack apps (each with an API and a frontend client):

- Task Manager (minimal .NET minimal API + React client)
  - API: [TaskManagerApi/Program.cs](TaskManagerApi/Program.cs)
  - Client: [TaskManagerClient/src/App.tsx](TaskManagerClient/src/App.tsx) — config in [TaskManagerClient/package.json](TaskManagerClient/package.json)

- Mini Project Manager (EF Core + JWT auth API + React + Tailwind client)
  - API: [MiniProjectManagerAPI/Program.cs](MiniProjectManagerAPI/Program.cs)
    - Auth implementation: [`MiniProjectManagerAPI.Services.AuthService`](MiniProjectManagerAPI/Services/AuthService.cs)
    - Controllers: [MiniProjectManagerAPI/Controllers/AuthController.cs](MiniProjectManagerAPI/Controllers/AuthController.cs), [MiniProjectManagerAPI/Controllers/ProjectsController.cs](MiniProjectManagerAPI/Controllers/ProjectsController.cs), [MiniProjectManagerAPI/Controllers/TasksController.cs](MiniProjectManagerAPI/Controllers/TasksController.cs)
  - Client: [MiniProjectManagerClient/src/App.tsx](MiniProjectManagerClient/src/App.tsx), config in [MiniProjectManagerClient/package.json](MiniProjectManagerClient/package.json)

This README provides succinct quick-start instructions for each folder.

Prerequisites
- .NET SDK (9.0 recommended for these projects)
- Node.js + npm
- Git (optional)
- From the dev container you can open browser with: "$BROWSER <url>"

Quick start — TaskManagerApi (minimal .NET API)
1. Open a terminal and run:
   cd TaskManagerApi
   dotnet restore
   dotnet run
2. The API will listen on the URL(s) configured in [TaskManagerApi/Properties/launchSettings.json](TaskManagerApi/Properties/launchSettings.json). Default (development) HTTP port: 5013.
3. Inspect key server code:
   - Routes and startup: [TaskManagerApi/Program.cs](TaskManagerApi/Program.cs)
   - In-memory repository: [TaskManagerApi/Repositories/TaskRepository.cs](TaskManagerApi/Repositories/TaskRepository.cs)
   - Task model: [TaskManagerApi/Models/TaskItem.cs](TaskManagerApi/Models/TaskItem.cs)

Quick start — TaskManagerClient (React + Vite)
1. Open a new terminal:
   cd TaskManagerClient
   npm install
   npm run dev
2. Vite dev server will print a local URL (commonly `http://localhost:5173`). Open it:
   $BROWSER http://localhost:5173
3. Client uses default API URL fallback in [TaskManagerClient/src/App.tsx](TaskManagerClient/src/App.tsx) (http://localhost:5013/api/tasks). Adjust if needed.

Quick start — MiniProjectManagerAPI (EF Core + JWT)
1. Open a terminal:
   cd MiniProjectManagerAPI
   dotnet restore
   dotnet run
2. The API auto-applies migrations at startup (see `db.Database.Migrate()` in [MiniProjectManagerAPI/Program.cs](MiniProjectManagerAPI/Program.cs)). Default ports are in [MiniProjectManagerAPI/Properties/launchSettings.json](MiniProjectManagerAPI/Properties/launchSettings.json) (http://localhost:5183 / https://localhost:7180 etc).
3. Important files:
   - Startup & auth config: [MiniProjectManagerAPI/Program.cs](MiniProjectManagerAPI/Program.cs)
   - JWT settings: [MiniProjectManagerAPI/JwtSettings.cs](MiniProjectManagerAPI/JwtSettings.cs) and [MiniProjectManagerAPI/appsettings.json](MiniProjectManagerAPI/appsettings.json)
   - Auth service: [`MiniProjectManagerAPI.Services.AuthService`](MiniProjectManagerAPI/Services/AuthService.cs)
   - Controllers: [MiniProjectManagerAPI/Controllers/AuthController.cs](MiniProjectManagerAPI/Controllers/AuthController.cs), [MiniProjectManagerAPI/Controllers/ProjectsController.cs](MiniProjectManagerAPI/Controllers/ProjectsController.cs), [MiniProjectManagerAPI/Controllers/TasksController.cs](MiniProjectManagerAPI/Controllers/TasksController.cs)
   - EF DbContext & models: [MiniProjectManagerAPI/Data/AppDbContext.cs](MiniProjectManagerAPI/Data/AppDbContext.cs), [MiniProjectManagerAPI/Models/Project.cs](MiniProjectManagerAPI/Models/Project.cs)

Quick start — MiniProjectManagerClient (React + Vite + Tailwind)
1. Open a new terminal:
   cd MiniProjectManagerClient
   npm install
   npm run dev
2. Open the dev URL (Vite output), e.g.:
   $BROWSER http://localhost:5173
3. Key client pieces:
   - API client: [MiniProjectManagerClient/src/api/axios.ts](MiniProjectManagerClient/src/api/axios.ts)
   - Routing & auth hooks: [MiniProjectManagerClient/src/App.tsx](MiniProjectManagerClient/src/App.tsx), [MiniProjectManagerClient/src/hooks/useAuth.ts](MiniProjectManagerClient/src/hooks/useAuth.ts)
   - Pages/components: [MiniProjectManagerClient/src/pages/Dashboard.tsx](MiniProjectManagerClient/src/pages/Dashboard.tsx), [MiniProjectManagerClient/src/pages/ProjectDetails.tsx](MiniProjectManagerClient/src/pages/ProjectDetails.tsx)

Tips & Notes
- MiniProjectManagerAPI requires a writable SQLite file (connection string in [MiniProjectManagerAPI/appsettings.json](MiniProjectManagerAPI/appsettings.json)). Migrations are included in [MiniProjectManagerAPI/Migrations](MiniProjectManagerAPI/Migrations).
- JWT config is in [MiniProjectManagerAPI/appsettings.json](MiniProjectManagerAPI/appsettings.json). Replace the secret for any public deployment.
- TaskManagerClient uses fetch directly — its fallback API URL is defined in [TaskManagerClient/src/App.tsx](TaskManagerClient/src/App.tsx).
- If ports collide, adjust launch settings or pass environment overrides.

Useful quick links
- [TaskManagerApi/Program.cs](TaskManagerApi/Program.cs)
- [TaskManagerApi/Repositories/TaskRepository.cs](TaskManagerApi/Repositories/TaskRepository.cs)
- [TaskManagerClient/src/App.tsx](TaskManagerClient/src/App.tsx)
- [TaskManagerClient/package.json](TaskManagerClient/package.json)
- [MiniProjectManagerAPI/Program.cs](MiniProjectManagerAPI/Program.cs)
- [`MiniProjectManagerAPI.Services.AuthService`](MiniProjectManagerAPI/Services/AuthService.cs)
- [MiniProjectManagerAPI/Controllers/AuthController.cs](MiniProjectManagerAPI/Controllers/AuthController.cs)
- [MiniProjectManagerAPI/Controllers/ProjectsController.cs](MiniProjectManagerAPI/Controllers/ProjectsController.cs)
- [MiniProjectManagerAPI/Controllers/TasksController.cs](MiniProjectManagerAPI/Controllers/TasksController.cs)
- [MiniProjectManagerClient/src/App.tsx](MiniProjectManagerClient/src/App.tsx)
- [MiniProjectManagerClient/package.json](MiniProjectManagerClient/package.json)

If you want, I can:
- Add environment variable guidance for Vite (VITE_* vars),
- Provide docker-compose for running both API + client,
- Or create short npm / dotnet scripts to start multiple services concurrently.

