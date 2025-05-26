# Service Discovery POC: Inventory and Order Services

This project demonstrates a microservices architecture using **.NET 8** and Python, focusing on service registration, health checks.

## Overview

The project consists of two main services:

1. **Order Service API** (`OrderServiceApi`):
   - Built with ASP.NET Core (.NET 8).
   - Exposes endpoints for order management.
   - Includes a health check endpoint for monitoring service status.
   - Includes a `ServiceRegistryClient` for registering the service with a service registry or sidecar for service discovery.
   - Configured via `Program.cs` and controllers such as `OrderController` and `HealthCheckController`.

2. **Inventory Service API** (`InventoryServiceApi`):
   - Built with Python.
   - Handles inventory-related operations.
   - Includes a health check endpoint for monitoring service status.
   - Includes a `ServiceRegistryClient` for registering the service with a service registry or sidecar for service discovery.

## Business Domain

- **Order Service**: creating orders - calls the Inventory Service to verify stock levels before processing orders.
- **Inventory Service**: verifying stock levels

## Key Components

- **Service Registration**: The `ServiceRegistryClient` in the Inventory Service registers the service with a registry for discoverability by other services.
- **Health Checks**: The Order Service exposes a health check endpoint to support monitoring and orchestration.
- **API Controllers**: Both services use controllers or handlers to process HTTP requests and encapsulate business logic.


1. Ensure you have **.NET 8 SDK** and **Python 3.8** installed.
2. For some reason Visual Studio gives a build error because of the Python project, so need to build/run using docker-compose.
	Navigate to the solution directory and Run the following command to build and start the services:
	```bash
	docker-compose build
	docker-compose up
	```
3. you can access to consul: http://localhost:8500/ui/dc1/services
   order Service API: http://localhost:6002/swagger/index.html
   inventory Service API: http://localhost:6003/docs#/
4. You can see egistered services in Consul UI.
   Use the POST http://localhost:6002/orders endpoint to create an order, which will internally call the Inventory Service (using consul/sidcar) to check stock levels, e.g.:
   ```json
   {
	"sku": "IP14P",
	"quantity": 100
	}
   ```


## Conclusion

The architecture supports service discovery, health monitoring, and clear separation of concerns, providing a solid foundation for more complex distributed systems.