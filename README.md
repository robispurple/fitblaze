# FitBlaze

A modern Blazor-based successor to FitNesse, powered by FitSharp for acceptance test automation.

## Overview

FitBlaze reimagines the classic FitNesse testing platform with a contemporary Blazor UI and ASP.NET Core backend. It maintains full compatibility with FitSharp test fixtures while providing a cleaner, more intuitive user experience for managing acceptance tests.

## Features (Planned)

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
- bd (Beads)
- gh (Github CLI)

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

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

## Contact & Support

For questions or issues, please open an issue on GitHub.
