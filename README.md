# TaskManagementApp

Full-stack task management application built with .NET 8 (Web API) and Angular 17.

## Projects
- TaskManagementApp.Core: Domain entities and interfaces.
- TaskManagementApp.Infrastructure: EF Core (SQLite) DbContext, repositories, migrations.
- TaskManagementApp.Services: Application services (Auth, Task CRUD).
- TaskManagementApp.Api: API controllers.
- TaskManagementApp.Api.Host: Host project configuring DI, Auth, Swagger, CORS.
- ClientApp: Angular 17 standalone application.

## Features
- User registration & login with JWT authentication (HS256).
- Task CRUD (create, list filter by status/priority, update, delete).
- Enum serialization as strings (e.g. "Pending", "Medium").
- Validation via data annotations (Title required max 150 chars, Description max 1000 chars, Username length 3-50).
- Angular UI with auth guard, login/register, task list, task form.

## Tech Stack
- .NET 8, ASP.NET Core minimal hosting model.
- EF Core 8 + SQLite.
- BCrypt for password hashing.
- System.Text.Json with JsonStringEnumConverter.
- Angular 17 standalone APIs, HttpClient, Router, Forms.

## Getting Started (Backend)
1. Ensure .NET 8 SDK installed.
2. Configure JWT key (already in appsettings.Development.json). For production use secrets or environment variable `Jwt__Key` with 32+ bytes.
3. Run: `dotnet run --project TaskManagementApp.Api.Host` (Swagger at /swagger).
4. Database (SQLite) auto-created: `taskmanagementapp.db`.

## Getting Started (Frontend)
1. `cd ClientApp`
2. `npm install`
3. `npm start` (Angular dev server with proxy to backend `/api`).
4. Visit http://localhost:4200

## API Endpoints
- POST /api/auth/register -> 200 OK or 409 Conflict `{ error: "Username already exists" }`
- POST /api/auth/login -> 200 OK `{ token, username }` or 401 `{ error: "Invalid username or password" }`
- GET /api/task -> list tasks (optional query: `status`, `priority`)
- GET /api/task/{id}
- POST /api/task -> create task
- PUT /api/task/{id} -> update task
- DELETE /api/task/{id} (requires JWT)

## Authentication
- Include `Authorization: Bearer <token>` header for protected endpoints.
- Angular stores token in localStorage under `jwt_token`.

## Validation Rules
- TaskItem.Title: required, <=150 chars.
- TaskItem.Description: optional, <=1000 chars.
- AppUser.Username: required, 3-50 chars.
- Password hashed with BCrypt.

## Migrations
To add new constraints (e.g. Description length in DB):
```
dotnet ef migrations add AddDescriptionMaxLength --project TaskManagementApp.Infrastructure --startup-project TaskManagementApp.Api.Host
```
Then:
```
dotnet ef database update --project TaskManagementApp.Infrastructure --startup-project TaskManagementApp.Api.Host
```

## Enum Handling
Serialized/deserialized as strings via `JsonStringEnumConverter`. Client sends `"Pending"`, `"Medium"` etc.

## Extending
- Add pagination: create query params `page`, `pageSize` and modify repository.
- Add user-task ownership: include UserId foreign key, filter by current user.
- Add refresh tokens for longer sessions.

## Security Notes
- Replace development JWT key in production.
- Consider HTTPS only and secure cookie storage if moving away from localStorage.

## License
MIT (adjust as needed).
