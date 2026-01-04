# Versioning & Release Process

FitBlaze uses a complementary two-tool approach for automated versioning and release notes:

## GitVersion + Versionize Architecture

### GitVersion (Authoritative Version Source)
- **Purpose**: Calculate semantic version based on git history
- **Configuration**: `gitversion.yml`
- **Usage**: Automatic in builds, embedded in assemblies
- **Output**: Version visible in app navbar
- **Workflow**: Calculates version from branch + commit history + tags

### Versionize (Changelog & Release Notes)
- **Purpose**: Generate CHANGELOG.md from conventional commits
- **Configuration**: `.versionize`
- **Usage**: Runs on main branch during releases
- **Output**: Human-readable changelog with GitHub Release notes
- **Key Feature**: Uses `--skip-version` flag to avoid version conflicts

### Why Two Tools?

**Machine-friendly (GitVersion)**
- No commit message discipline required
- Automatic prerelease versions for branches
- Version embedded in builds
- Works with any git history

**Human-friendly (Versionize)**
- Beautiful, organized changelogs
- Conventional commit-based organization
- GitHub Release integration
- Clear contributor experience

### How They Work Together

```
Developer writes conventional commits
    ↓
GitVersion calculates version automatically
    ↓
CI builds & packages with GitVersion version
    ↓
On main branch: Versionize generates CHANGELOG.md
    ↓
Versionize creates tag (doesn't bump version)
    ↓
CI publishes package with GitVersion version
```

**Critical**: Versionize runs with `--skip-version` flag, so it NEVER touches the version number. GitVersion remains the single source of truth.

## Commit Message Format

Use [Conventional Commits](https://www.conventionalcommits.org/):

```bash
# New feature
git commit -m "feat: Add new widget"

# Bug fix
git commit -m "fix: Resolve race condition"

# Breaking change
git commit -m "feat!: Redesign API"

# Documentation
git commit -m "docs: Update README"

# Chore
git commit -m "chore: Update dependencies"
```

## Release Workflow

1. **Develop on feature branches** → GitVersion assigns prerelease versions
2. **Merge to develop** → Continuous delivery builds
3. **Create release branch** → `release/X.Y.Z` (GitVersion: X.Y.Z-beta.N)
4. **Test & fix on release** → Bug fixes only
5. **Merge to main** → GitVersion: X.Y.Z (final)
6. **Run Versionize** → Generates CHANGELOG.md (no version bump)
7. **Tag & push** → Ready for release

## Checking Versions

**GitVersion**:
```bash
dotnet-gitversion          # Full version info (JSON)
dotnet-gitversion | grep SemVer   # Quick version
dotnet-gitversion /showconfig      # View configuration
```

**Versionize**:
```bash
versionize --dry-run --skip-version  # Preview changelog
versionize --skip-version            # Generate changelog
```

## Configuration Files

### gitversion.yml
GitFlow-based configuration with:
- Main branch: patch increments, no pre-release
- Develop branch: minor increments, alpha pre-release
- Release branches: patch increments, beta pre-release
- Feature/hotfix branches: inherited increments

### .versionize
Changelog generation settings:
- Sections: Features, Bug Fixes, Performance, Refactoring, Documentation, Maintenance, Tests
- Commit link format to GitHub
- GitHub Release compare links

## Key Rules

1. **GitVersion is authoritative** for version numbers
2. **Do not manually edit version numbers** in `.csproj`
3. **Versionize uses `--skip-version`** to avoid conflicts
4. **Conventional commits** enable changelog generation
5. **Tags freeze versions** at specific commits
6. **Release branches** use beta pre-release labels
7. **Main branch** has clean semantic versions (no pre-release)
