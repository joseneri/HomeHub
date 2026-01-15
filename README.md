# 🏠 HomeHub - Real Estate Platform

![.NET 8](https://img.shields.io/badge/.NET%208-LTS-purple?style=for-the-badge&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue?style=for-the-badge)
![RabbitMQ](https://img.shields.io/badge/Messaging-RabbitMQ%20%2B%20MassTransit-orange?style=for-the-badge)

## 🚀 Overview

**HomeHub** is a modern Backend portfolio project built to demonstrate **Enterprise-Grade** software architecture using **.NET 8**.

It is designed as a scalable system that handles real estate operations (Communities, Homes, Leads) while decoupling complex business logic from infrastructure concerns. The project showcases the transition from a monolithic approach to a distributed service-oriented architecture using **Event-Driven patterns**.

## 🛠️ Tech Stack & Modern Features

This project utilizes the latest features of the .NET ecosystem to ensure performance, maintainability, and scalability:

* **Framework:** .NET 8 (Long Term Support)
* **Architecture:** Clean Architecture (Domain-Centric Design)
* **Messaging & Async:** RabbitMQ with MassTransit for decoupled microservices communication.
* **Data Access:** Entity Framework Core (Code-First) with Optimistic Concurrency (`RowVersion`).
* **Resilience:** Polly policies for HTTP retries and circuit breaking.
* **Validation:** FluentValidation for robust Request/DTO validation.
* **Observability:** Structured Logging & Correlation IDs for distributed tracing.
* **API Documentation:** Swagger / OpenAPI (Swashbuckle).

## 🏗️ Architecture Structure

The solution follows strict separation of concerns:

1.  **HomeHub.Domain:** The core. Pure C# entities, Enums, and Repository Interfaces. No external dependencies.
2.  **HomeHub.Application:** Business logic, Use Cases (Services), DTOs, Validators, and AutoMapper profiles.
3.  **HomeHub.Infrastructure:** Implementation of Repositories, DBContext, External API Clients (Zippopotamus), and Message Bus configuration.
4.  **HomeHub.Api:** The entry point. Minimalist Controllers and Global Exception Handling (`IExceptionHandler`).

## ⚡ Key Implementations

### 1. Event-Driven Architecture (MassTransit)
Instead of tight coupling, services communicate via events.
* *Example:* When a `Lead` is created, a `LeadCreatedEvent` is published to the bus, allowing other consumers (Notification Service, Analytics, etc.) to react asynchronously.

### 2. Global Exception Handling (.NET 8 Style)
Uses the modern `IExceptionHandler` interface to centralize error management, transforming exceptions into standard **RFC 7807 Problem Details** responses without cluttering controllers with try-catch blocks.

### 3. Robust Concurrency
Implements **Optimistic Concurrency Control** using `[Timestamp]` row versioning to prevent data inconsistency when multiple admins edit the same property simultaneously.

## 🔧 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker](https://www.docker.com/) (for RabbitMQ)

### Running the Infrastructure
Start the message broker using Docker:

```bash
docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
Running the API
Clone the repository.

Navigate to the API folder and run:

Bash

dotnet run --project HomeHub.Api
Access Swagger UI at: https://localhost:7196/swagger (or your configured port).

Author: Rodrigo Neri
