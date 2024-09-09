## Link to ticket:

[Add link to ticket]

## Changes proposed in this PR:

[Explain what was done and why. You can also add screenshots here if it helps.]

> [!IMPORTANT]
> Since we do not have integration testing in our pipeline, integration testing should be done locally before PR creation. Was it green? 

## How to test:

1. Do this
2. Then do that


## Best practices:

<details>
<summary><h3>Best Practices for C# and .NET Core Development</h3></summary>
<p>

### 1. **Project Structure**
- **Organize Your Code:** Use folders to separate different layers of application (e.g., Controllers, Services, Repositories).
- **Naming Conventions:** Follow consistent naming conventions for files, classes, and methods.

### 2. **Coding Standards**
- **Write Clean Code:** Follow C# coding conventions and keep your code simple and readable.

### 3. **Dependency Injection**
- **Service Registration:** Register services in `Startup.cs` or `Program.cs` using the built-in dependency injection system.
- **Understand Service Lifetimes:** Use scoped, transient, and singleton services based on your needs.

### 4. **Security**
- **Validate Input:** Always validate user input to protect against attacks.

### 5. **Database Access**
- **Use Entity Framework Core:** Leverage EF Core for database operations and handle schema changes with migrations.
- **Repository Pattern:** Implement repositories to abstract data access.

### 6. **Testing**
- **Write Unit Tests:** Create unit tests for your methods using xUnit.
- **Integration Tests:** Test how different parts of your application work together.

### 8. **Documentation**
- **API Docs:** Use Swagger to generate API documentation.
- **Code Comments:** Document your code with comments to explain complex logic.

### 9. **Version Control**
- **Commit Messages:** Write clear and descriptive commit messages.
- **Branching Strategy:** Use branches to manage features and fixes.

### 10. **Resources**
- **Follow Official Guides:** Refer to [Microsoft Documentation](https://learn.microsoft.com/en-us/dotnet/) for detailed best practices and updates.

</p>
</details>
