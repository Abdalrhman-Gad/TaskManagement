# Task Management Backend API

![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20%2F%20DDD-success)
![Database](https://img.shields.io/badge/Database-SQL%20Server-lightgrey)
![Caching](https://img.shields.io/badge/Caching-Redis-red)

A robust, scalable, and maintainable Task Management Backend API built with **.NET 10**. This project adheres to **Clean Architecture** and **Domain-Driven Design (DDD)** principles to ensure separation of concerns and testability.

---

## Table of Contents
- [Architecture & Design](#architecture--design)
- [Features Implemented](#features-implemented)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Execution](#installation--execution)
- [Authentication & Seeding](#authentication--seeding)
- [API Overview](#api-overview)
- [Key Implementations & Assumptions](#key-implementations--assumptions)
- [ERD & Database Mapping](#erd--database-mapping)
- [Video Walkthrough](#video-walkthrough)

---

## Architecture & Design

The solution is divided into four main layers based on Clean Architecture:

- **`Domain`**: The core of the system. Contains domain entities (e.g., `User`, `TaskItem`), enums, and domain-specific interfaces. Has no dependencies on other projects.
- **`Application`**: Contains the business logic. Implements CQRS using **MediatR** for Commands and Queries. Defines DTOs and repository interfaces.
- **`Infrastructure`**: Implements data access using **Entity Framework Core**. Handles JWT generation, **Redis** caching, and **Background Processing** (using `.NET BackgroundService`).
- **`Api`**: The presentation layer. Exposes RESTful endpoints via Controllers, configures Swagger, and handles global exceptions via middleware.

---

## Features Implemented

- **Clean Architecture & DDD**: Clear separation of concerns and highly decoupled codebase.
- **RESTful API Design**: Clear resource-based endpoints documented with Swagger/OpenAPI.
- **JWT Authentication**: Secure endpoints with Access & Refresh tokens.
- **Role-Based Authorization**: Distinct roles (`Admin` and `User`) with specific privileges.
- **Redis Caching**: Optimized `Get Task by ID` endpoint with automatic cache invalidation upon task updates.
- **Background Processing**: Simulated asynchronous task processing utilizing `Channel` and `.NET BackgroundService`.
- **Soft Delete**: Non-destructive deletion for users (`IsDeleted` flag).
- **Global Exception Handling**: Centralized error formatting using custom middleware.
- **Business Logic Rules**: 
  - Tasks sorted by priority and creation date.
  - Prevention of duplicate tasks (same title, same user, same day).

---

## Getting Started

### Prerequisites

Ensure you have the following installed on your machine:
1. [**.NET 10 SDK**](https://dotnet.microsoft.com/download/dotnet/10.0)
2. **SQL Server** (LocalDB is configured by default, or you can update the connection string in `appsettings.json`).
3. **Redis Server** (Default configuration points to `localhost:6379`).

### Installation & Execution

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Abdalrhman-Gad/TaskManagement.git
   cd TaskManagement
   ```

2. **Start Redis:**
   If you have Docker installed, you can quickly spin up a Redis instance:
   ```bash
   docker run -d -p 6379:6379 --name redis-server redis
   ```

3. **Run the API:**
   Navigate to the API project and run it:
   ```bash
   cd TaskManagement.Api
   dotnet run
   ```

4. **Automatic Migrations & Seeding:**
   The application will automatically apply Entity Framework Core migrations and seed the database on startup.

5. **Explore via Swagger:**
   Open your browser and navigate to the Swagger UI (usually `https://localhost:7170/swagger` or similar based on your launch profile) to test the endpoints.

---

## Authentication & Seeding

On the first run, the system automatically seeds a default **Admin** user. You can use these credentials in the `/api/Auth/login` endpoint to retrieve your JWT token:

- **Email:** `admin@example.com`
- **Password:** `Admin@123`

*Note: Pass this token in the `Authorization` header (`Bearer <token>`) to access protected endpoints.*

---

## API Overview

### Auth
- `POST /api/Auth/register` - Register a new user
- `POST /api/Auth/login` - Authenticate and receive JWT & Refresh Token
- `GET /api/Auth/me` - Get current logged-in user profile

### Admin
- `GET /api/Admin/users` - List all users (Admin only)
- `POST /api/Admin/users` - Create a new user (Admin only)
- `DELETE /api/Admin/users/{id}` - Soft delete a user (Admin only)

### Tasks
- `POST /api/Task` - Create a new task (Logged-in user)
- `GET /api/Task` - Retrieve all tasks for the logged-in user
- `GET /api/Task/{id}` - Retrieve a specific task (cached via Redis)
- `PUT /api/Task/{id}/status` - Update the status of a specific task

---

## Key Implementations & Assumptions

1. **Caching Strategy**: Redis is assumed to be running locally on the default port. The `GetTaskById` endpoint checks Redis first. If a task's status is updated, the associated cache key is actively invalidated/refreshed.
2. **Background Processing**: To keep the project self-contained and lightweight as per requirements, an in-memory `Channel` coupled with a `BackgroundService` is used instead of an external message broker (e.g., RabbitMQ). It simulates background work by delaying and automatically updating the task status to `Done`.
3. **Soft Delete**: Deleting a user via the Admin endpoint toggles an `IsDeleted` flag rather than removing the record completely, ensuring data integrity.
4. **Duplicate Tasks Rule**: Duplicate tasks are blocked if a user attempts to create a task with the exact same Title on the exact same calendar day (UTC).

---

## ERD & Database Mapping

To view the Entity-Relationship Diagram (ERD) and database mapping used for this project, please click the link below:

- [View ERD & Database Mapping Diagram](https://drive.google.com/file/d/1Wzib51Igj3Lvhc55y8OFyyjEoiSl5Tbg/view?usp=sharing)

---

## Video Walkthrough

- [Click here to view the video walkthrough](https://)
