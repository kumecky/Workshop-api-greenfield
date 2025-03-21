# Room Reservation API

A .NET 8 Web API for managing office room reservations.

## Features

- Create, read, update, and delete reservations
- View available rooms for a specific time slot
- Manage users and rooms
- Business rule validation for reservations

## Architecture

This project follows a domain-driven design approach with the following layers:

- **Domain Layer**: Contains business entities and domain services
- **Application Layer**: Contains application services and DTOs
- **Infrastructure Layer**: Contains database and repository implementations
- **API Layer**: Contains API controllers and endpoints

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core with InMemory database
- FluentValidation for validation
- Swagger for API documentation

## Getting Started

### Prerequisites

- .NET 8 SDK or later

### Running the API

1. Clone the repository
2. Navigate to the project directory
3. Run the following command:

```bash
dotnet run
```

4. Access the Swagger UI at https://localhost:5001/swagger or http://localhost:5000/swagger

## API Endpoints

### Reservations

- `GET /api/reservations` - Get all reservations
- `GET /api/reservations/{id}` - Get a reservation by ID
- `POST /api/reservations` - Create a new reservation
- `PUT /api/reservations/{id}` - Update a reservation
- `PATCH /api/reservations/{id}/cancel` - Cancel a reservation
- `DELETE /api/reservations/{id}` - Delete a reservation

### Rooms

- `GET /api/rooms` - Get all rooms
- `GET /api/rooms/{id}` - Get a room by ID
- `GET /api/rooms/available` - Get available rooms for a time slot

## Data Models

### Reservation

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "roomId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "roomName": "Conference Room A",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userName": "John Doe",
  "startTime": "2023-04-01T09:00:00Z",
  "endTime": "2023-04-01T10:00:00Z",
  "purpose": "Team Meeting",
  "status": "Confirmed",
  "createdAt": "2023-03-25T12:00:00Z",
  "updatedAt": "2023-03-26T12:00:00Z"
}
```

### Room

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Conference Room A",
  "capacity": 20,
  "description": "Large conference room with projector",
  "floor": 1,
  "features": ["Projector", "Whiteboard", "VideoConference"]
}
```

## License

This project is licensed under the MIT License. 