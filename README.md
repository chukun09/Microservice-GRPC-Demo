# Attendance Management System with Microservices

---

## Introduction

This project is an Attendance Management System designed to handle attendance tracking efficiently in distributed environments. Leveraging microservices architecture, gRPC for communication between services, and RabbitMQ for asynchronous messaging, it provides a scalable and resilient solution for managing attendance records.

## Features

- **Microservices Architecture**: The system is built as a collection of loosely coupled microservices, each responsible for a specific aspect of attendance management.
- **gRPC Communication**: Services communicate with each other using gRPC, enabling efficient and type-safe communication between services.
- **RabbitMQ Integration**: RabbitMQ is used as a message broker for asynchronous communication between services, ensuring reliability and fault tolerance.
- **Ocelot API Gateway**: Ocelot acts as an API gateway, providing a single entry point for clients to access the various microservices.
- **YARP (Yet Another Reverse Proxy)**: YARP is used alongside Ocelot to handle incoming requests, providing advanced routing and load balancing capabilities.

## Services

1. **User Service**: Manages user accounts and authentication.
2. **Attendance Service**: Handles attendance tracking and reporting.
3. **Employee Service**: Manager information of employee.
4. **Gateway Service (Ocelot)**: Provides API gateway functionality.
5. **Reverse Proxy Service (YARP)**: Handles incoming requests and forwards them to the appropriate services.

## Prerequisites

- .NET Core SDK
- Docker
- RabbitMQ
- gRPC Tools

## Getting Started

1. Clone the repository.
2. Navigate to the project directory.
3. Start RabbitMQ server.
4. Build and run each microservice individually.
5. Access the API endpoints through the gateway service provided by Ocelot.

## Configuration

- Each microservice can be configured via environment variables or configuration files.
- Update the `appsettings.json` or `.env` files in each service's directory to customize settings such as database connection strings, RabbitMQ configuration, and gRPC endpoints.

## Usage

- Use the provided API documentation to understand the available endpoints and their functionalities.
- Integrate with your frontend or other systems to start managing attendance effectively.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- The developers of gRPC, RabbitMQ, Ocelot, and YARP for providing the tools necessary to build this system.
- Inspiration from real-world attendance management systems and best practices in microservices architecture.

