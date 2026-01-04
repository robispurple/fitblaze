# FitBlaze Roadmap

This document outlines the high-level epics for a Blazor-based FitNesse successor powered by FitSharp. It treats FitNesse + FitSharp as a combined ecosystem and translates that into a clean, modern roadmap.

---

## 1. 📚 Wiki & Content Management Epic  
**Purpose:** Replace FitNesse's file‑based wiki with a modern, Blazor‑driven content system.

### What FitNesse Does Today
- Stores pages as text files in a hierarchical folder structure  
- Uses its own wiki syntax  
- Provides page editing, history, and linking  

### What You Need to Build
- Blazor UI for viewing/editing pages  
- Markdown or HTML-based content model  
- Page hierarchy (folders, subpages, navigation)  
- Versioning (optional MVP: simple change log)  
- Search and tagging  
- Page templates (test page, suite page, fixture page)

### Deliverables
- Wiki viewer  
- Wiki editor  
- Page tree navigation  
- Storage provider (filesystem, SQLite, or cloud)

---

## 2. 🧪 Test Execution Engine Integration Epic  
**Purpose:** Connect the Blazor UI to FitSharp so tests can run from the browser.

### What FitNesse Does Today
- Parses wiki tables  
- Converts them into Slim/Fit commands  
- Executes fixtures  
- Returns results as HTML  

### What You Need to Build
- A service layer that:
  - Extracts test tables/specs from wiki pages  
  - Converts them into FitSharp input format  
  - Invokes FitSharp programmatically  
  - Captures results (pass/fail, exceptions, logs)  
- A result serializer (JSON)  
- A Blazor component to display results  

### Deliverables
- FitSharp integration service  
- Test runner API  
- Test result viewer (tables, colors, logs)  

---

## 3. 🧩 Fixture & Library Management Epic  
**Purpose:** Provide a modern way to manage fixtures, assemblies, and dependencies.

### What FitNesse Does Today
- Uses classpaths and Java packages  
- Loads fixtures dynamically  
- Requires manual configuration in wiki pages  

### What You Need to Build
- Assembly loader for .NET fixtures  
- UI for registering fixture assemblies  
- Reflection-based fixture discovery  
- Validation tools (e.g., "fixture not found")  

### Deliverables
- Fixture registry UI  
- Assembly upload/selection  
- Fixture browser (list classes, methods, parameters)

---

## 4. 🧱 Test Suite Management Epic  
**Purpose:** Support grouping, running, and reporting on suites of tests.

### What FitNesse Does Today
- Suites are folders containing test pages  
- Running a suite executes all children  
- Produces summary reports  

### What You Need to Build
- Suite definition model  
- Recursive test execution  
- Suite dashboards  
- Aggregated pass/fail metrics  

### Deliverables
- Suite runner  
- Suite result dashboard  
- Historical suite summaries (optional)

---

## 5. 📊 Reporting & Visualization Epic  
**Purpose:** Modernize FitNesse's HTML reports into interactive Blazor dashboards.

### What FitNesse Does Today
- Generates static HTML reports  
- Shows pass/fail counts  
- Minimal visualization  

### What You Need to Build
- Interactive result dashboards  
- Trend charts (optional)  
- Drill‑down views for failures  
- Export options (JSON, HTML)  

### Deliverables
- Test result page  
- Suite result dashboard  
- Failure detail viewer  

---

## 6. 🔐 Authentication & Permissions Epic  
**Purpose:** Add modern security that FitNesse largely lacks.

### What FitNesse Does Today
- Very basic authentication  
- No role-based permissions  

### What You Need to Build
- Login system (Identity or external provider)  
- Roles: Admin, Editor, Viewer  
- Page-level permissions  
- Audit logs  

### Deliverables
- Auth system  
- Role-based access control  
- Audit trail

---

## 7. 🔄 CI/CD & Automation Integration Epic  
**Purpose:** Make the system usable in pipelines and automated environments.

### What FitNesse Does Today
- Provides a command-line runner  
- Integrates with CI via scripts  

### What You Need to Build
- REST API for triggering tests  
- CLI runner (optional)  
- Webhooks for reporting results  
- GitHub Actions/Azure DevOps samples  

### Deliverables
- Test execution API  
- CI integration examples  
- Webhook support

---

## 8. 🌐 Deployment & Hosting Epic  
**Purpose:** Package the system so teams can easily run it.

### What FitNesse Does Today
- Runs as a standalone Java server  
- Can be embedded in CI  

### What You Need to Build
- Docker image  
- Self-hosted ASP.NET Core server  
- Optional Blazor WebAssembly mode  
- Configuration system  

### Deliverables
- Dockerfile  
- Appsettings configuration  
- Deployment guide

---

## 9. 🧭 Migration & Compatibility Epic  
**Purpose:** Help existing FitNesse users transition.

### What FitNesse Does Today
- Uses proprietary wiki syntax  
- Stores pages in text files  

### What You Need to Build
- Import tool for FitNesse page folders  
- Syntax converter (FitNesse wiki → Markdown)  
- Fixture compatibility layer (FitSharp already helps here)  

### Deliverables
- FitNesse importer  
- Syntax converter  
- Migration guide

---

## MVP Scope  

### Must‑Haves
- Wiki viewer/editor  
- Page hierarchy  
- FitSharp integration  
- Run test button  
- Basic result display  
- Fixture assembly loading  
- Simple suite execution  

### Nice‑to‑Haves
- Authentication  
- Basic reporting  
- Docker deployment  

---

## Epic Sequence (Recommended Order)
1. Wiki & Content Management  
2. Test Execution Engine Integration  
3. Fixture & Library Management  
4. Test Suite Management  
5. Reporting & Visualization  
6. Authentication & Permissions  
7. CI/CD Integration  
8. Deployment & Hosting  
9. Migration & Compatibility  

This sequence provides a clean, incremental path from "blank Blazor app" to "full FitNesse successor."
