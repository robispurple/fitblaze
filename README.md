# FitBlaze

A modern Blazor-based successor to FitNesse, powered by FitSharp for acceptance test automation.

## Overview

FitBlaze reimagines the classic FitNesse testing platform with a contemporary Blazor UI and ASP.NET Core backend. It maintains full compatibility with FitSharp test fixtures while providing a cleaner, more intuitive user experience for managing acceptance tests.

## Features

### Completed
- **Version Display** — Application version shown in navigation bar
- **Semantic Versioning** — Automatic version bumping via GitVersion based on git history and commit messages
- **GitFlow Workflow** — Support for develop, main, release, feature, and hotfix branches

### Planned
- **Modern Wiki UI** — Blazor-based page editor and viewer with hierarchical navigation
- **FitSharp Integration** — Execute acceptance tests directly from the browser
- **Fixture Management** — Assembly loader and fixture discovery
- **Test Reporting** — Interactive dashboards and detailed result visualization
- **Suite Execution** — Group and run test suites with aggregated reporting
- **Authentication & Permissions** — Role-based access control
- **CI/CD Integration** — REST API for pipeline automation
- **Docker Deployment** — Containerized deployment support

## Current Status

This project is in early planning and development. See the [Roadmap](docs/ROADMAP.md) for the detailed epic breakdown and MVP scope.

## Prerequisites

- .NET 10 or later
- Visual Studio 2026 or Visual Studio Code
- git
- bd (Beads) — issue tracking
- gh (Github CLI) — GitHub integration
- GitVersion.Tool — semantic versioning (`dotnet tool install --global GitVersion.Tool`)

## Contributing

Contributions are welcome. Please follow the code style guidelines in [AGENTS.md](AGENTS.md).

### Clone the repository
```bash
git clone https://github.com/robispurple/fitblaze.git
cd fitblaze
```

### Build
```bash
dotnet build .\FitBlaze.slnx
```

### Run (when code is ready)
```bash
dotnet run --project .\FitBlaze\FitBlaze.csproj
```

### Run tests (when tests are added)
```bash
dotnet test
```

For more details, see [AGENTS.md](AGENTS.md).

### Version Management

FitBlaze uses [GitVersion](https://gitversion.net/) for automated semantic versioning. The application version is displayed in the navigation bar.

**Check current version:**
```bash
dotnet-gitversion
```

**Control version bumping with commit messages:**
```bash
git commit -m "feat: Add new feature" +semver: minor      # Bump minor version
git commit -m "fix: Bug fix" +semver: patch               # Bump patch version
git commit -m "BREAKING CHANGE: API redesign" +semver: major  # Bump major version
git commit -m "docs: Update README" +semver: none         # No version bump
```

See [AGENTS.md](AGENTS.md) for detailed versioning workflow and release process.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Contact & Support

For questions or issues, please open an issue on GitHub.
