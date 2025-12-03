# Clean Architecture .NET 10 Blazor

A production-grade enterprise Clean Architecture solution built with .NET 10, featuring Blazor Server frontend and REST API.

## 🏗️ Architecture Overview

This project follows Clean Architecture principles, separating concerns into distinct layers:

```
src/
├── CleanArchitecture.Domain/           # Core business logic, entities, value objects
│   ├── Common/                         # Base classes (BaseEntity, BaseAuditableEntity, IDomainEvent)
│   ├── Constants/                      # Domain constants
│   ├── DomainEvents/                   # Domain event definitions
│   ├── Entities/                       # Business entities (Product, Order, Customer, OrderItem)
│   ├── Enumerators/                    # Enums (OrderStatus, PaymentMethod)
│   ├── Exceptions/                     # Custom domain exceptions
│   ├── Repositories/                   # Repository interfaces
│   └── ValueObjects/                   # Value objects (Money, Address, Email)
│
├── CleanArchitecture.Application/      # Application use cases and business logic
│   ├── Abstractions/                   # Interface abstractions
│   │   ├── Caching/                    # Cache service interface
│   │   ├── Common/                     # DateTime & CurrentUser providers
│   │   ├── Data/                       # DbContext & UnitOfWork interfaces
│   │   ├── Email/                      # Email service interface
│   │   └── Messaging/                  # Event bus interface
│   ├── Behaviors/                      # MediatR pipeline behaviors (validation, logging)
│   ├── Common/                         # Shared types (Result<T>)
│   ├── DTOs/                           # Data transfer objects
│   │   ├── Common/                     # Shared DTOs (Pagination, Money, Address)
│   │   ├── Customers/                  # Customer DTOs
│   │   ├── Orders/                     # Order DTOs
│   │   └── Products/                   # Product DTOs
│   └── Products/                       # Product CQRS handlers
│       ├── Commands/                   # Create, Update, Delete commands
│       └── Queries/                    # Get, Search queries
│
├── CleanArchitecture.Infrastructure/   # External concerns implementation
│   ├── BackgroundServices/             # Hosted background services
│   ├── Data/
│   │   ├── Configurations/             # EF Core entity configurations
│   │   ├── DataContext/                # DbContext and UnitOfWork
│   │   ├── Interceptors/               # SaveChanges interceptors (audit, domain events)
│   │   ├── Repositories/               # Repository implementations
│   │   └── Seed/                       # Database seeding
│   ├── HealthChecks/                   # Health check implementations
│   └── Services/                       # Service implementations (Cache, Email, etc.)
│
└── Presentation/
    ├── CleanArchitecture.API/          # REST API (Controllers, Swagger)
    └── CleanArchitecture.Web/          # Blazor Server frontend
```

## 🚀 Features

### Domain Layer
- **Rich Domain Models**: Product, Order, OrderItem, Customer entities
- **Value Objects**: Money, Address, Email with validation
- **Domain Events**: OrderCreatedEvent, OrderCompletedEvent
- **Custom Exceptions**: DomainException, InsufficientStockException

### Application Layer
- **CQRS Pattern**: Commands and Queries with MediatR
- **Validation Pipeline**: FluentValidation with MediatR behavior
- **Logging Pipeline**: Automatic request/response logging
- **Result Pattern**: Type-safe success/failure handling

### Infrastructure Layer
- **Entity Framework Core**: SQL Server with fluent configuration
- **Interceptors**: Audit tracking, domain event dispatching
- **Repository Pattern**: Generic and specialized repositories
- **Health Checks**: Database and cache connectivity
- **Background Services**: Cache cleanup, event processing

### Presentation Layer
- **REST API**: Full CRUD for Products with Swagger documentation
- **Blazor Server**: Interactive frontend

## 📦 Technologies

- **.NET 10** - Latest framework
- **Entity Framework Core 10** - ORM
- **MediatR** - CQRS and mediator pattern
- **FluentValidation** - Input validation
- **Swashbuckle** - OpenAPI/Swagger
- **Blazor Server** - Interactive web UI

## 🔧 Getting Started

### Prerequisites
- .NET 10 SDK
- SQL Server (LocalDB or instance)

### Configuration
Update the connection string in `CleanArchitecture.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanArchDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Run the API
```bash
cd CleanArchitecture.API
dotnet run
```
Navigate to https://localhost:{port}/swagger for API documentation.

### Run the Blazor App
```bash
cd CleanArchitecture.Web
dotnet run
```

## 📋 API Endpoints

### Products
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get paginated products |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/category/{category}` | Get products by category |
| GET | `/api/products/search?searchTerm=...` | Search products |
| POST | `/api/products` | Create product |
| PUT | `/api/products/{id}` | Update product |
| PATCH | `/api/products/{id}/stock` | Update stock |
| PATCH | `/api/products/{id}/activate` | Activate product |
| PATCH | `/api/products/{id}/deactivate` | Deactivate product |
| DELETE | `/api/products/{id}` | Delete product |

### Health
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Application health status |

## 🏛️ Architecture Principles

1. **Dependency Inversion**: Inner layers define interfaces, outer layers implement
2. **Separation of Concerns**: Each layer has a single responsibility
3. **Domain-Centric**: Business logic resides in Domain and Application layers
4. **Testability**: All dependencies are injectable
5. **Flexibility**: Infrastructure can be swapped without affecting business logic

## 📄 License

This project is for educational and demonstration purposes.