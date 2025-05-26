# Service Discovery POC: Inventory & Order Services

Proof of Concept for microservices using **.NET 8** and **Python**, focusing on **service registration** and **health checks**.

## Services

### Order Service API (`OrderServiceApi`)
- Built with ASP.NET Core (.NET 8)
- Endpoints for creating orders
- Health check endpoint
- Registers itself via `ServiceRegistryClient`

### Inventory Service API (`InventoryServiceApi`)
- Built with Python (FastAPI)
- Handles inventory operations
- Health check endpoint
- Registers itself via `ServiceRegistryClient`

## Domain Logic

- `OrderService`:  
  Creates orders and checks stock levels via InventoryService

- `InventoryService`:  
  Verifies and manages stock availability

## Features

- **Service Registration**: Consul-based using `ServiceRegistryClient`
- **Health Checks**: endpoints for service monitoring
- **HTTP APIs**: RESTful endpoints in both services

## How to Run

### 1. Requirements
- .NET 8 SDK
- Python 3.8+

### 2. Note
Visual Studio may fail to build the Python project — use Docker Compose instead.

### 3. Start services
```bash
docker-compose build
docker-compose up
```

### 4. Access services
- Consul UI: [http://localhost:8500/ui/dc1/services](http://localhost:8500/ui/dc1/services)
- Order API: [http://localhost:6002/swagger/index.html](http://localhost:6002/swagger/index.html)
- Inventory API: [http://localhost:6003/docs#/](http://localhost:6003/docs#/)

### 5. Sample request
```http
POST http://localhost:6002/orders
Content-Type: application/json

{
  "sku": "IP14P",
  "quantity": 100
}
```

---

## Summary

This POC demonstrates:

- Service discovery via Consul
- Sidecar pattern for inter-service communication
- Health monitoring endpoints