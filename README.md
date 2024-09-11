# Backend: E-commerce App
## Overview
E-commerce is a modern web application designed for managing e-commerce operations. It includes functionalities for creating, reading, updating, and deleting categories, products, users, and other entities necessary for an online store.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies](#technologies)
- [Structure](#structure)
- [Installation](#installation)
- [Running the Application](#running-the-application)
- [Testing](#testing)
- [API Documentation](#api-documentation)

## Features
- Authentication and Authorization: Secure user authentication using JWT tokens.
- Category Management: CRUD operations for product categories.
- Product Management: CRUD operations for products.
- User Management: CRUD operations for users.
- Review Management: CRUD operations for product reviews.
- Order Management: Manage customer orders.
- File Management: Handle file uploads and storage.
- Swagger Documentation: Auto-generated API documentation.
- Unit and Integration Testing using xUnit and Moq.
- CI/CD: Continuous integration and deployment using GitHub Actions.

## Technologies
- ASP.NET Core: Web framework for building the application.
- Entity Framework Core: ORM for database interactions.
- PostgreSQL: Relational database management system.
- FluentValidation: Library for validating data.
- Swagger: Tool for documenting APIs.
- xUnit: Testing framework.
- Moq: Library for creating mock objects in tests.

## Project structure

```
.github/
    pull_request_template.md
    workflows/
        check.yml
.gitignore
.vscode/
    settings.json
docs/
    README.md
Ecommerce/
    Ecommerce.Controllers/
        AppController.cs
        AuthController.cs
        CartItemController.cs
        CategoryController.cs
        CustomAuthorization/
        CustomMiddleware/
        ...
    Ecommerce.Domain/
    Ecommerce.Infrastructure/
    Ecommerce.Services/
    Ecommerce.sln
    Ecommerce.Tests/
README.md
```

### Layers and Entities
#### Controllers

Controllers handle HTTP requests and return responses. They act as an interface between the client and the server-side logic.

- AuthController: Manages user authentication and authorization.
- CategoryController: Handles CRUD operations for product categories.
- ProductController: Manages CRUD operations for products.
- UserController: Manages CRUD operations for users.
- OrderController: Handles customer orders.
- ReviewController: Manages product reviews.

There is also exception handling middleware that has additional features, such as specific ways of handling SQL exceptions and generating JSON-friendly output.

#### Services
Services contain the business logic of the application. They interact with repositories to perform operations on the data.

- AuthService: Handles authentication and token generation.
- CategoryService: Manages business logic for categories.
- ProductService: Manages business logic for products.
- UserService: Manages business logic for users.
- OrderService: Manages business logic for orders.
- ReviewService: Manages business logic for reviews.

#### Repositories
Repositories encapsulate the logic required to access data sources. They provide an abstraction layer over the database.

- CategoryRepository: Handles data operations for categories.
- ProductRepository: Handles data operations for products.
- UserRepository: Handles data operations for users.
- OrderRepository: Handles data operations for orders.
- ReviewRepository: Handles data operations for reviews.

#### Domain
The domain layer contains the core entities and interfaces of the application.

- Entities: Represent the data models, such as Category, Product, User, Order, and Review.
- Interfaces: Define the contracts for repositories and services.

The ERD diagram, which describes the database structure (without additional tables not related to the business logic, such as UserSalt and migrations):
![Blank diagram (1)](https://github.com/user-attachments/assets/a1876200-cf8d-4b85-8c77-ba278d9c2383)


#### Infrastructure
The infrastructure layer contains the implementation of repositories, the database context, and additional services.

- EcommerceContext: The Entity Framework Core database context.
- Migrations: Database migrations for schema changes.
- Database Functions: Database migrations for SQL functions.

In the repository, there are many implemented SQL functions, for example, a function for getting categories with pagination and recursive filtering based on the parent category, a function for converting cart items into order items while checking the available quantity of the product and updating it, and a function for deleting an order while restoring the number of products.

Additional Infrastructure Services:
- File Service: Manages file uploads and storage.
- Password Hash Service: Handles secure password hashing and verification.
- Token Generation Service: Creates and manages JWT tokens for authentication.

#### Extension Methods
The project extensively uses extension methods for efficient conversion between entities and DTOs (Data Transfer Objects). These methods provide a clean and reusable way to map data between different object types, enhancing code readability and maintainability.

#### Tests
The tests layer contains unit and integration tests to ensure the correctness of the application.

- Unit Tests: Test individual components in isolation.
- Integration Tests: Test the interaction between multiple components.

## Installation

1. Clone the repository:
```
git clone git@github.com:rokuzzz/fs18_CSharp_FullStack_Backend.git
```
2. Navigate to the project directory:
```
cd fs18_CSharp_Teamwork/Ecommerce/
```
3. Restore dependencies:
```
dotnet restore
```
4. Navigate to the teamwork_cicd (optional, if you have your own Postgres database, you can use it).
```
cd ../teamwork_cicd
```
5. Run the docker container for the database (optional)
```
docker-compose -f docker-compose.local.yml up -d
```
6. Add appsettings.json to the Ecommerce/Ecommerce.Infrastructure folder and specify your connection string (if you used our docker setup, you can use the example file as is).
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Localhost": "Host=localhost;Port=5435;Database=postgres;Username=postgres;Password=postgres"
  }
}
```
   
## Running the Application Locally
1. Build the project (from the Ecommerce/Ecommerce.Infrastructure folder):
```
dotnet build
```
2. Apply the migrations (make sure the database is up and running and you can use the default `public' schema)
```
dotnet ef database update
```
3. Run the project:
```
dotnet run 
```
4. Open your browser and navigate to `http://localhost:5169`.

## Testing
1. Run tests:
```
dotnet test Ecommerce.Tests/
```

## API Documentation

API documentation is available at `http://localhost:5169/index.html`. 

<img width="1457" alt="Screenshot 2024-08-30 at 10 02 55" src="https://github.com/user-attachments/assets/b9384662-7fed-4fdb-b9e3-63da842e95db">

[The API Design prototype](docs/README.md) is also available in the repository.

## Contributors:

- [Mahmood Sharifizemeidani](https://github.com/mahmood-sharifi)
- [Roman Kuzero](https://github.com/rokuzzz)
- [Arseniiy Kapshtyk](https://github.com/kapshtyk)

---


Feel free to reach out if you have any questions!
