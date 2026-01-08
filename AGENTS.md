# AGENTS.md

## Project: FitBlaze
A Blazor-based successor/port to [FitNesse](https://github.com/unclebob/fitnesse), powered by FitSharp for acceptance test automation.

## Issue Tracking

This project uses **bd (beads)** for issue tracking. **DO NOT** make markdown files documenting tasking. All tasking and details about tasking is within bd (beads).
Run `bd prime` for workflow context, or install hooks (`bd hooks install`) for auto-injection.

### Quick Reference

```bash
# Find ready work (no blockers)
bd ready --json

# Find ready work including future deferred issues
bd ready --include-deferred --json

# Create new issue
bd create "Issue title" -t bug|feature|task -p 0-4 -d "Description" --json

# Create issue with due date and defer (GH#820)
bd create "Task" --due=+6h              # Due in 6 hours
bd create "Task" --defer=tomorrow       # Hidden from bd ready until tomorrow
bd create "Task" --due="next monday" --defer=+1h  # Both

# Update issue status
bd update <id> --status in_progress --json

# Update issue with due/defer dates
bd update <id> --due=+2d                # Set due date
bd update <id> --defer=""               # Clear defer (show immediately)

# Link discovered work
bd dep add <discovered-id> <parent-id> --type discovered-from

# Complete work
bd close <id> --reason "Done" --json

# Show dependency tree
bd dep tree <id>

# Get issue details
bd show <id> --json

# Query issues by time-based scheduling (GH#820)
bd list --deferred              # Show issues with defer_until set
bd list --defer-before=tomorrow # Deferred before tomorrow
bd list --defer-after=+1w       # Deferred after one week from now
bd list --due-before=+2d        # Due within 2 days
bd list --due-after="next monday" # Due after next Monday
bd list --overdue               # Due date in past (not closed)
```

### Workflow

1. **Check for ready work**: Run `bd ready` to see what's unblocked
2. **Claim your task**: `bd update <id> --status in_progress`
3. **Check out a new branch**: name the branch appropriately for the task and issue type
4. **Start watching the app**: `dotnet watch run --project .\FitBlaze\FitBlaze.csproj`
5. **Work on it**: Implement, test, document the task in the dedicated branch
6. **Discover new work**: If you find bugs or TODOs, create issues:
   - `bd create "Found bug in auth" -t bug -p 1 --json`
   - Link it: `bd dep add <new-id> <current-id> --type discovered-from`
7. **Complete**: `bd close <id> --reason "Implemented"`
8. **Export**: Run `bd export -o .beads/issues.jsonl` before committing

### Issue Types

- `bug` - Something broken that needs fixing
- `feature` - New functionality
- `task` - Work item (tests, docs, refactoring)
- `epic` - Large feature composed of multiple issues
- `chore` - Maintenance work (dependencies, tooling)

### Priorities

- `0` - Critical (security, data loss, broken builds)
- `1` - High (major features, important bugs)
- `2` - Medium (nice-to-have features, minor bugs)
- `3` - Low (polish, optimization)
- `4` - Backlog (future ideas)

### Dependency Types

- `blocks` - Hard dependency (issue X blocks issue Y)
- `related` - Soft relationship (issues are connected)
- `parent-child` - Epic/subtask relationship
- `discovered-from` - Track issues discovered during work

Only `blocks` dependencies affect the ready work queue.

## Landing the Plane

**When the user says "let's land the plane"**, you MUST complete ALL steps below. The plane is NOT landed until `git push` succeeds. NEVER stop before pushing. NEVER say "ready to push when you are!" - that is a FAILURE.

**MANDATORY WORKFLOW - COMPLETE ALL STEPS:**

1. **File beads issues for any remaining work** that needs follow-up
2. **Ensure all quality gates pass** (only if code changes were made):
    - Run `dotnet format --verbosity diagnostic .\FitBlaze.slnx`
    - Run `dotnet test`
    - File P0 issues if quality gates are broken
3. **Verify version bump** (if code changes were made):
    - Check current version: `dotnet-gitversion`
    - If version should be bumped, ensure commits have proper `+semver:` markers
    - For features: `+semver: minor` or `+semver: major` (breaking)
    - For bugfixes: `+semver: patch` (default)
    - For docs/chores: `+semver: none`
4. **Update beads issues** - close finished work, update status
5. **PUSH TO REMOTE - NON-NEGOTIABLE** - This step is MANDATORY. Execute ALL commands below:
   ```bash
   # Pull first to catch any remote changes
   git pull --rebase

   # If conflicts in .beads/issues.jsonl, resolve thoughtfully:
   #   - git checkout --theirs .beads/issues.jsonl (accept remote)
   #   - bd import -i .beads/issues.jsonl (re-import)
   #   - Or manual merge, then import

   # Sync the database (exports to JSONL, commits)
   bd sync

   # MANDATORY: Push everything to remote
   # DO NOT STOP BEFORE THIS COMMAND COMPLETES
   git push

   # MANDATORY: Verify push succeeded
   git status  # MUST show "up to date with origin/main"
   ```

   **CRITICAL RULES:**
   - The plane has NOT landed until `git push` completes successfully
   - NEVER stop before `git push` - that leaves work stranded locally
   - NEVER say "ready to push when you are!" - YOU must push, not the user
   - If `git push` fails, resolve the issue and retry until it succeeds
   - The user is managing multiple agents - unpushed work breaks their coordination workflow
   - You may create a PR but never merge the PR without the users permission.

   6. **Clean up git state** - Clear old stashes and prune dead remote branches:
   ```bash
   git stash clear                    # Remove old stashes
   git remote prune origin            # Clean up deleted remote branches
   ```
   7. **Verify clean state** - Ensure all changes are committed AND PUSHED, no untracked files remain
   8. **Choose a follow-up issue for next session**
   - Provide a prompt for the user to give to you in the next session
   - Format: "Continue work on bd-X: [issue title]. [Brief context about what's been done and what's next]"

**REMEMBER: Landing the plane means EVERYTHING is pushed to remote. You may create a PR but never merge the PR without the users permission. No exceptions. No "ready when you are". PUSH IT. NERVER MERGE IT without permission.**

**Example "land the plane" session:**

```bash
# 1. File remaining work
bd create "Add integration tests for sync" -t task -p 2 --json

# 2. Run quality gates (only if code changes were made)
dotnet format --verbosity diagnostic .\FitBlaze.slnx
dotnet test

# 3. Verify version bump (only if code changes were made)
dotnet-gitversion
# Check if version was bumped; if not and changes warrant it:
# - Ensure commits have proper +semver: markers
# - For features: git commit --amend --no-edit (then add marker to message)
# - For bugfixes: git commit --amend --no-edit
# - For docs: add +semver: none to commit message

# 4. Close finished issues
bd close bd-42 bd-43 --reason "Completed" --json

# 5. PUSH TO REMOTE - MANDATORY, NO STOPPING BEFORE THIS IS DONE
git pull --rebase
# If conflicts in .beads/issues.jsonl, resolve thoughtfully:
#   - git checkout --theirs .beads/issues.jsonl (accept remote)
#   - bd import -i .beads/issues.jsonl (re-import)
#   - Or manual merge, then import
bd sync        # Export/import/commit
git push       # MANDATORY - THE PLANE IS STILL IN THE AIR UNTIL THIS SUCCEEDS
git status     # MUST verify "up to date with origin/main"

# 6. Clean up git state
git stash clear
git remote prune origin

# 7. Verify everything is clean and pushed
git status

# 8. Choose next work
bd ready --json
bd show bd-44 --json
```

**Then provide the user with:**

- Summary of what was completed this session
- What issues were filed for follow-up
- Status of quality gates (all passing / issues filed)
- Current version (from `dotnet-gitversion`)
- Confirmation that ALL changes have been pushed to remote
- Recommended prompt for next session

**CRITICAL: Never end a "land the plane" session without successfully pushing. The user is coordinating multiple agents and unpushed work causes severe rebase conflicts.**

## Development Guidelines

### Code Style, Conventions, and Standards
- **Language**: C#, modern .NET conventions
- **Formatting / Linting**: Run `dotnet format` to enforce `.editconfig` compliance
- **Testing**: All new features need tests (`dotnet test`)
- **Naming**: PascalCase for classes/methods, camelCase for properties/variables
- **Structure**: Feature-based folder organization (Features/Wiki, Features/TestExecution, etc.)
- **Dependency Injection**: Where applicable features should be implemented as services and incorporated using dependency injection
- **Async**: Use async/await throughout
- **Interfaces**: Program interface-driven design for testability
- **Error Handling**: Use custom exceptions, log via ILogger<T>, return meaningful HTTP status codes
- **Type Safety**: Leverage C# type system; avoid var for unclear types

### Architecture

**Tech Stack**:
- ASP.NET Core + Blazor Server
- FitSharp integration
- C# .NET 10.0+
- XUnit and BUnit for testing
- Aspire for front end, API, and database observability
- Husky.NET for git hooks
- Github CLI (gh) for checking Issues and PRs
**Key Components**:
- Wiki/Content Management (Blazor UI + storage provider)
- FitSharp Test Execution Engine integration (service layer)
- Fixture & Assembly Management (reflection-based discovery)
- Test Suite Runner & Result Aggregation
- REST API for CI/CD automation
- Authentication (ASP.NET Identity)
**Databases**:
- SQLite (MVP) with potential growth to a Firebase clone like Trailbase for page versioning, results, audit logs

### File Organization

```
fitblaze/
├── FitBlaze/                      # Main Blazor Server application
│   ├── Features/                  # Feature-based organization
│   │   └── Wiki/                  # Wiki management feature
│   │       ├── Components/        # Reusable Blazor components
│   │       ├── Pages/             # Page-level Blazor components
│   │       ├── Services/          # Business logic (PageService, SlugService)
│   │       ├── Models/            # Data models (WikiPage)
│   │       ├── Repositories/      # Data access (IPageRepository, implementations)
│   │       └── Controllers/       # REST API endpoints (PagesController)
│   ├── Pages/                     # Root-level pages (Index, Counter, etc.)
│   ├── Shared/                    # Shared layout components (NavMenu, etc.)
│   ├── Data/                      # Database context (WikiContext)
│   ├── wwwroot/                   # Static assets (CSS, JS, images)
│   ├── Properties/                # Project properties
│   ├── FitBlaze.csproj           # Project file
│   ├── Program.cs                 # Application startup
│   └── App.razor                  # Root component
│
├── FitBlaze.Tests/                # xUnit test project
│   ├── Features/                  # Feature tests (mirrored structure)
│   │   └── Wiki/                  # Wiki feature tests
│   │       ├── Services/          # Service unit tests
│   │       ├── Controllers/       # Controller integration tests
│   │       └── Repositories/      # Repository tests
│   ├── Pages/                     # Component tests (PageList, PageViewer, PageEditor tests)
│   └── FitBlaze.Tests.csproj     # Test project file
│
├── docs/                          # Documentation
│   ├── ROADMAP.md                # Feature roadmap and epics breakdown
│   └── *_FEATURES_WIKI.md        # (Future) Wiki feature architecture docs
│
├── .beads/                        # Beads issue tracking
│   ├── config.json               # Beads configuration
│   └── issues.jsonl              # Issue storage (JSONL format)
│
├── .husky/                        # Git hooks (Husky.Net)
│   └── pre-commit                # Pre-commit hook (dotnet format)
│
├── FitBlaze.slnx                 # Solution file
├── AGENTS.md                      # Build/test commands and code guidelines
├── README.md                      # This file
├── LICENSE                        # MIT License
├── .gitignore                     # Git ignore rules
└── .editconfig                    # Code style configuration
```

## Common Tasks

### Adding Examples

1. Create directory in `examples/`
2. Add README.md explaining the example
3. Include working code
4. Link from `examples/README.md`
5. Mention in main README.md

### Check Outdated Dependencies

1. Check solution dependencies: `dotnet list .\FitBlaze.slnx package --outdated`
2. Check if existing issues exist for updating them: `bd list`
3. If they do not, create new issues for tracking their upgrade: `bd create "Update {Dependency Name - Version}: ..." -t task -p 2`

## Questions?

- Check existing issues: `bd list`
- Look at recent commits: `git log --oneline -20`
- Read the docs: README.md
- Create an issue if unsure: `bd create "Question: ..." -t task -p 2`

## Important Files

- **README.md** - Main documentation (keep this updated!)
- **docs/roadmap.md** - Long term milestones for the project

## Pro Tips for Agents

- Always use `--json` flags for programmatic use
- Link discoveries with `discovered-from` to maintain context
- Check `bd ready` before asking "what next?"
- Export to JSONL before committing (or use git hooks)
- Use `bd dep tree` to understand complex dependencies
- Priority 0-1 issues are usually more important than 2-4


## Build & Test Commands

- **Build**: `dotnet build .\FitBlaze.slnx`
- **Build (Release)**: `dotnet build .\FitBlaze.sln -c Release`
- **Run project**: `dotnet run` (from project directory)
- **Run single test**: `dotnet test --filter "FullyQualifiedName~NameOfTest"`
- **Run all tests**: `dotnet test`
- **Watch mode**: `dotnet watch run --project .\FitBlaze\FitBlaze.csproj`
- **Clean**: `dotnet clean`

## Semantic Versioning with GitVersion & Versionize

FitBlaze uses a complementary two-tool approach for automated versioning and changelog management:

### Architecture: GitVersion + Versionize

**GitVersion** = Authoritative version source
- Calculates version from git history and tags
- Feeds version to builds, packages, and assembly attributes
- Visible in app navbar
- Configuration: `gitversion.yml` (GitFlow-based)

**Versionize** = Changelog and release notes
- Generates CHANGELOG.md from conventional commits
- Creates GitHub Release tags
- **Does NOT bump the version** (GitVersion handles that)
- Configuration: `.versionize`

This pattern avoids version conflicts and gives you both machine-friendly versioning and human-friendly release notes.

### How They Work Together

1. Developer commits using conventional format: `feat:`, `fix:`, `BREAKING CHANGE:`, etc.
2. GitVersion automatically calculates version based on branch/tags
3. CI builds and packages using GitVersion's version
4. On release (main branch), Versionize generates CHANGELOG.md with `--skip-version` flag
5. Versionize creates a tag, CI publishes package

### Current Version & Configuration

- **Check version**: `dotnet-gitversion`
- **GitVersion config**: `gitversion.yml`
- **Versionize config**: `.versionize`
- **Version Format**: `MAJOR.MINOR.PATCH[-PRERELEASE]`

### Commit Message Format (Conventional Commits)

FitBlaze uses [Conventional Commits](https://www.conventionalcommits.org/) for consistent, machine-readable commit messages. This enables both GitVersion version calculations and Versionize changelog generation:

```bash
# New feature (increments MINOR version)
git commit -m "feat: Add wiki search functionality"

# Bug fix (increments PATCH version)
git commit -m "fix: Correct test execution timeout"

# Breaking change (increments MAJOR version)
git commit -m "feat!: Redesign test result API"
git commit -m "fix!: Remove deprecated fixture loader"

# Documentation (no version bump)
git commit -m "docs: Update README with examples"

# Chore/maintenance (typically no version bump)
git commit -m "chore: Update dependencies"
```

**Conventional Commit Structure:**
```
<type>[optional scope]: <description>

[optional body]

[optional footer with BREAKING CHANGE:]
```

**Valid types:**
- `feat` — A new feature (minor version bump)
- `fix` — A bug fix (patch version bump)
- `docs` — Documentation changes
- `test` — Adding or updating tests
- `chore` — Build process, dependencies, tooling
- `refactor` — Code changes that don't fix bugs or add features
- `perf` — Performance improvements

**Breaking changes:**
- Add `!` before the colon: `feat!:` or `fix!:`
- Or include `BREAKING CHANGE:` in the footer
- Triggers a MAJOR version bump

### Branch Versioning Behavior

| Branch | Mode | Label | Behavior |
|--------|------|-------|----------|
| `main` | Manual | (none) | Patch increment; stable releases |
| `develop` | Continuous Delivery | `alpha` | Minor increment; integration branch |
| `release/*` | Manual | `beta` | Patch increment; release candidates |
| `feature/*` | Manual | branch name | Inherited; development features |
| `hotfix/*` | Manual | `beta` | Patch increment; urgent fixes |
| `support/*` | Manual | (none) | Patch increment; long-term support |

### Creating Release Tags

Tag releases on `main` branch to freeze versions:

```bash
# Create and push a release tag
git tag -a v1.2.0 -m "Release version 1.2.0"
git push origin v1.2.0

# List existing tags
git tag -l

# Check version for a specific tag
git checkout v1.0.0
dotnet-gitversion
```

### Checking Current Version

Use the CLI tool to inspect the current calculated version:

```bash
# Full version details (JSON)
dotnet-gitversion

# Quick version string
dotnet-gitversion | grep SemVer

# View effective GitVersion configuration
dotnet-gitversion /showconfig
```

### Workflow Example: Releasing v1.0.0

1. **Develop features** on `develop` branch (versions: 0.1.0-alpha.1, 0.1.0-alpha.2, ...)
2. **Create release branch** from `develop`:
   ```bash
   git checkout develop
   git pull
   git checkout -b release/1.0.0
   git push -u origin release/1.0.0
   ```
3. **Test and fix** on release branch (versions: 1.0.0-beta.1, 1.0.0-beta.2, ...)
4. **Merge to main** and tag:
   ```bash
   git checkout main
   git pull
   git merge --no-ff release/1.0.0
   git tag -a v1.0.0 -m "Release version 1.0.0"
   git push origin main --tags
   ```
5. **Merge back to develop** (or delete release branch for mainline flow)

### Common Scenarios

**I made a bug fix on main** — Use `+semver: patch`:
```bash
git commit -m "fix: Handle null wiki pages" +semver: patch
```

**I added a breaking API change** — Use `+semver: major`:
```bash
git commit -m "refactor!: Rewrite test execution API" +semver: major
```

**I updated docs** — Use `+semver: none`:
```bash
git commit -m "docs: Add GitVersion guide" +semver: none
```

**I'm working on a feature** — Just commit normally; develop branch auto-increments minor.

## Release Process (Maintainers)

1. **Prepare release branch** from `develop` (if using GitFlow)
   ```bash
   git checkout develop
   git pull
   git checkout -b release/X.Y.Z
   git push -u origin release/X.Y.Z
   ```

2. **Test thoroughly** on release branch
   ```bash
   dotnet build .\FitBlaze.slnx
   dotnet test
   dotnet format --verbosity diagnostic .\FitBlaze.slnx
   ```

3. **Verify version calculation** on release branch
   ```bash
   dotnet-gitversion
   # Release branches build as X.Y.Z-beta.N
   ```

4. **Merge to main** with fast-forward disabled
   ```bash
   git checkout main
   git pull
   git merge --no-ff release/X.Y.Z
   ```

6. **Verify final version** on main branch
   ```bash
   dotnet-gitversion
   # Main builds as X.Y.Z (no pre-release)
   ```

7. **Generate changelog with Versionize** (on main branch)
   ```bash
   # Versionize generates CHANGELOG.md but skips version bumping
   # (GitVersion is the authoritative version source)
   versionize --skip-version
   ```

8. **Create and push release tag**
   ```bash
   git tag -a vX.Y.Z -m "Release version X.Y.Z"
   git push origin main --tags
   ```

9. **Merge release branch back to develop** (if continuing development)
   ```bash
   git checkout develop
   git pull
   git merge --no-ff release/X.Y.Z
   git push
   # Optional: delete release branch
   git branch -d release/X.Y.Z
   git push origin --delete release/X.Y.Z
   ```

10. **GitHub Actions** handles CI/CD and package publication (TBD)

**Important Notes:**
- Version numbers are calculated automatically by GitVersion based on commits and tags
- **Do not manually edit version numbers in code**
- Versionize generates CHANGELOG.md using `--skip-version` to avoid version conflicts
- Release branches use `beta` pre-release labels
- Main branch shows final semantic versions (no pre-release)
- Tags freeze versions at specific commits
- GitVersion is the authoritative version source; Versionize only handles changelog generation

---
