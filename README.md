# FormulaOne API

ASP.NET Core Web API for Formula 1 drivers and achievements, built with a layered architecture and CI/CD on Azure Pipelines.

**Stack:** C#, ASP.NET Core, EF Core (code-first), AutoMapper, Azure Pipelines

## Design
- **Layers:** FormulaOne.Api (controllers) → FormulaOne.DataService (repositories + Unit of Work) → FormulaOne.Entities (domain models and DTOs)
- **Data access:** Repository + Unit of Work pattern over EF Core for transactional consistency
- **Mapping:** AutoMapper separates DTOs from domain entities
- **DI:** Dependency injection throughout; SOLID and REST principles
- **Errors:** Custom exception types with centralized handling
- **CI/CD:** Azure Pipelines build on every push
