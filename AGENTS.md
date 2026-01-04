# AGENTS.md

## Project: FitBlaze
A Blazor-based successor to FitNesse, powered by FitSharp for acceptance test automation.

## Build & Test Commands
- **Build**: `dotnet build`
- **Build (Release)**: `dotnet build -c Release`
- **Run project**: `dotnet run` (from project directory)
- **Run single test**: `dotnet test --filter "FullyQualifiedName~NameOfTest"`
- **Run all tests**: `dotnet test`
- **Watch mode**: `dotnet watch run`
- **Clean**: `dotnet clean`

## Architecture
**Tech Stack**: ASP.NET Core + Blazor Server, FitSharp integration, C#/.NET
**Key Components**:
- Wiki/Content Management (Blazor UI + storage provider)
- FitSharp Test Execution Engine integration (service layer)
- Fixture & Assembly Management (reflection-based discovery)
- Test Suite Runner & Result Aggregation
- REST API for CI/CD automation
- Authentication (ASP.NET Identity)

**Databases**: SQLite (MVP) or cloud-based for page versioning, results, audit logs

## Code Style & Conventions
- **Language**: C#, modern .NET conventions
- **Naming**: PascalCase for classes/methods, camelCase for properties/variables
- **Structure**: Feature-based folder organization (Features/Wiki, Features/TestExecution, etc.)
- **Async**: Use async/await throughout
- **Interfaces**: Program interface-driven design for testability
- **Error Handling**: Use custom exceptions, log via ILogger<T>, return meaningful HTTP status codes
- **Formatting**: Run `dotnet format` before commits
- **Type Safety**: Leverage C# type system; avoid var for unclear types
