# Task Management API

## Description
A simple .NET Core API for managing tasks, with features like authentication, role-based access, and task assignment.

## Prerequisites
- .NET SDK 8.0
- Visual Studio or VS Code
- Postman (optional)
- Git

## Technologies Used
- .NET Core 8.0
- Entity Framework Core
- JWT Authentication
- In-Memory Database

## How to Run
1. Clone the repository:
   ```bash
   git clone https://github.com/chaomein123/TaskManagementAPI.git
   cd TaskManagementAPI
   
2. Restore packages and run application:
   dotnet restore
   dotnet run

3. Open http://localhost:{port}/swagger in a browser to test the API.
    Endpoints
        Authentication
            POST /auth/login
                Use admin:adminpass or user:userpass.

                How to Use the Bearer Token
                    Get the token:
                        Send a POST request to /auth/login with the user's username and password.
                            Example request body:
                                {
                                "username": "admin",
                                "password": "adminpass"
                                }
                            The response will include a token like:
                                {
                                "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
                                }
                    Include the token in your Task API requests:
                        For any request to /tasks, /tasks/{id}, or /tasks/user/{userId}, you must include the token in the HTTP header.
                            Example header:
                                Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        Task Management
            POST /tasks - Create a new task.
            GET /tasks/{id} - Get task details by ID.
            GET /tasks/user/{userId} - Get tasks assigned to a specific user.

    Testing
    Use Swagger at http://localhost:{port}/swagger