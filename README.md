# PMS-ClinicAPI
A comprehensive backend service for clinic management operations, providing a REST API for the [PMS (Practice
Management Software)](https://github.com/stars/JanFelixFuchs/lists/pms) system. Designed to manage medical practices 
and small clinics with a focus on clean architecture and maintainability.


## Table of Contents
- [Overview](#overview)
  - [Core Responsibilities](#core-responsibilities)
  - [Key Features](#key-features)
  - [Technology Stack](#technology-stack)
- [Architecture](#architecture)
  - [Layered Architecture](#layered-architecture)
  - [Request Flow](#request-flow)
  - [Domain Entities](#domain-entities)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Configuration](#installation--configuration)
  - [Running the Application](#running-the-application)
  - [Accessing the API Documentation](#accessing-the-api-documentation)


## Overview
PMS-ClinicAPI serves as the backend foundation for clinic and practice management operations. It exposes REST API 
endpoints consumed by the [PMS-ClinicApp](https://github.com/JanFelixFuchs/PMS-ClinicApp) frontend application 
and provides comprehensive business logic for managing clinic operations.

### Core Responsibilities
The service is responsible for:
- **REST API Endpoints**: Providing well-designed API endpoints for clinic operations
- **Data Validation**: Processing and validating clinic-related data with comprehensive business rules
- **Business Logic**: Executing application and business logic following Clean Architecture principles
- **Data Persistence**: Communicating with the infrastructure layer for data storage and retrieval
- **Structured Responses**: Returning consistent, well-formatted HTTP responses to client applications
- **Error Handling**: Providing meaningful error messages and proper HTTP status codes

### Key Features
- **Clean Architecture**: Layered architecture with clear separation of concerns
- **Validation**: Fluent Validation for robust data validation
- **CQRS Pattern**: MediatR for handling commands and queries
- **Logging**: Comprehensive logging with Serilog
- **API Documentation**: OpenAPI/Swagger support via Swashbuckle


### Technology Stack
| Component                  | Technology                    |
|----------------------------|-------------------------------|
| **Framework**              | .NET 9.0 with C# 13.0         |
| **ORM**                    | Entity Framework Core         |
| **Command/Query Handling** | MediatR                       |
| **Validation**             | Fluent Validation             |
| **Logging**                | Serilog                       |
| **API Documentation**      | Swashbuckle (OpenAPI/Swagger) |
| **Database**               | MySQL                         |


## Architecture
This application implements a layered architecture inspired by Clean Architecture principles, ensuring a clear 
separation of concerns, maintainability, and extendability.

### Layered Architecture

- **Domain Layer**: The innermost layer containing core business models and domain concepts.
  - Core business entities
  - Business rules and domain logic
  - Independent of external frameworks and libraries

- **Application Layer**: Orchestrates business workflows and use cases.
  - Application-specific logic and workflows
  - Use case handlers (using CQRS)
  - Validation rules using Fluent Validation
  - Business process orchestration
  - DTOs for inter-layer communication

- **Infrastructure Layer**: Handles technical implementation details and external service integrations.
  - Database context and repository implementations
  - Entity Framework Core configurations
  - External service integrations
  - Technical infrastructure concerns

- **API Layer**: Exposes REST API endpoints to client applications.
  - HTTP controllers and routing
  - Request/response mapping
  - Cross-cutting concerns (logging, exception handling)
  - Security and authorization
  - OpenAPI/Swagger documentation

- **Utilities Layer**: Shared utilities and common functionality.

### Request Flow
A typical request follows this flow through the application layers:
```
1. Client → HTTP Request
        ↓
2. API Layer → Routing & Model Binding
        ↓
3. Application Layer → Use Case Handler
        ↓
4. Validation → Fluent Validation Rules
        ↓
5. Business Logic → Execute Use Case
        ↓
6. Infrastructure Layer → Database Access
        ↓
7. Response → Structured HTTP Response
        ↓
8. Client ← Response
```

### Domain Entities
The system manages the following core business entities:

- **Appointments**: Scheduling and management of clinic appointments
- **Patients**: Patient information and medical records
- **Clinicians**: Healthcare staff and their management
- **Rooms**: Clinic rooms and resource management
- **Devices**: Medical equipment and device tracking
- **Results**: Medical documents such as test results and outcomes
- **Users & Roles**: Authentication, authorization, and access control

For a detailed entity relationship diagram, see [docs/architecture.drawio](docs/architecture.drawio).


## Getting Started
Notice that this guide only covers instructions on setting up the environment for local development. Instructions
on the setup of the production environment can be found in [PMS-Compose repository](https://github.com/JanFelixFuchs/PMS-Compose).

### Prerequisites
For local development, you will need:

- **.NET SDK 9.0**
- **MySQL** - Local or remote instance
- **IDE** - JetBrains Rider, Visual Studio or VS Code
- **Postman** - For testing API endpoints

### Installation & Configuration

#### 1. Clone the Repository
```bash
git clone https://github.com/JanFelixFuchs/PMS-ClinicAPI.git
cd PMS-ClinicAPI
```

#### 2. Configure Database Connection
Create a local development configuration file:

```bash
cp ClinicAPI/appsettings.json ClinicAPI/appsettings.Development.json
```

Edit `ClinicAPI/appsettings.Development.json` and fill in configuration settings, such as...
```json
{
  "DatabaseSettings": {
    "Server": "localhost",
    "Database": "PMS_ClinicDB",
    "User": "username",
    "Password": "YourSecurePassword123!",
    "Port": "1433",
    "Version": "9.3.0"
  }
}
```

### Running the Application
The application can be run using either the built-in ui functionality of the chosen ide or the .NET CLI
```bash
cd ClinicAPI
dotnet run
```
The application will start and be available at `http://localhost:5059` by default.

### Accessing the API Documentation

Once running, access the Swagger/OpenAPI documentation at: `http://localhost:5059/api/docs`.