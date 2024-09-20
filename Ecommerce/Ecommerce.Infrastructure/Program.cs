using Ecommerce.Controllers;
using Ecommerce.Controllers.CustomMiddleware;
using Ecommerce.Domain.Interfaces;
using Ecommerce.Infrastructure;
using Ecommerce.Infrastructure.Database;
using Ecommerce.Infrastructure.Repository;
using Ecommerce.Infrastructure.Services;
using Ecommerce.Services.AuthService;
using Ecommerce.Services.AuthService.Helpers;
using Ecommerce.Services.AuthService.Intefaces;
using Ecommerce.Services.CategoryService;
using Ecommerce.Services.CategoryService.DTO;
using Ecommerce.Services.CategoryService.Interfaces;
using Ecommerce.Services.ProductService;
using Ecommerce.Services.ProductService.DTO;
using Ecommerce.Services.ProductService.Interfaces;
using Ecommerce.Services.ReviewService;
using Ecommerce.Services.ReviewService.DTO;
using Ecommerce.Services.ReviewService.Interfaces;
using Ecommerce.Services.Common.Interfaces;
using Ecommerce.Services.CartItemService;
using Ecommerce.Services.CartItemService.Interfaces;
using Ecommerce.Services.OrderService;
using Ecommerce.Services.OrderService.Interfaces;
using Ecommerce.Services.UserService;
using Ecommerce.Services.UserService.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Controllers.CustomAuthorization;
using Ecommerce.Services.AuthService.DTO;
using static Ecommerce.Services.AuthService.DTO.RegisterDto;
using static Ecommerce.Services.AuthService.DTO.LoginDto;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Configure Services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure Middleware and Endpoints
Configure(app, builder.Environment);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // CORS configuration
    services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    // Controllers and Routing
    services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // Swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title = "Product Shop API",
            Version = "v1"
        });

        c.EnableAnnotations();
        c.SchemaFilter<EnumSchemaFilter>();

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Bearer token authentication",
            Name = "Authorization",
            In = ParameterLocation.Header
        });

        c.OperationFilter<SecurityRequirementsOperationFilter>();
    });

    // Database
    var connectionString = configuration.GetConnectionString("NeonDatabase");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Neon database connection string is not configured.");
    }

    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    var dataSource = dataSourceBuilder.Build();
    services.AddDbContext<EcommerceContext>(options =>
        options.UseNpgsql(dataSource).UseSnakeCaseNamingConvention());

    // File Service
    services.AddScoped<IFileService, FileService>(provider =>
    {
        var environment = provider.GetRequiredService<IWebHostEnvironment>();
        return new FileService(environment);
    });

    // Category Services
    services.AddScoped<ICategoryService, CategoryService>();
    services.AddScoped<ICategoryRepo, CategoryRepository>();
    services.AddScoped(provider =>
    {
        var categoryService = provider.GetRequiredService<ICategoryService>();
        var fileService = provider.GetRequiredService<IFileService>();
        return new CategoryController(categoryService, fileService);
    });

    // CartItem Services
    services.AddScoped<ICartItemService, CartItemService>();
    services.AddScoped<ICartItemRepo, CartItemRepository>();
    services.AddScoped(provider =>
    {
        return new CartItemController(
            provider.GetRequiredService<ICartItemService>(),
            provider.GetRequiredService<IAuthorizationService>(),
            provider.GetRequiredService<ICartItemRepo>()
        );
    });

    // Order Services
    services.AddScoped<IOrderService, OrderService>();
    services.AddScoped<IOrderRepo, OrderRepository>();
    services.AddScoped(provider =>
    {
        return new OrderController(
            provider.GetRequiredService<IOrderService>(),
            provider.GetRequiredService<IOrderRepo>(),
            provider.GetRequiredService<IAuthorizationService>()
        );
    });

    // Product Services
    services.AddScoped<IProductService, ProductService>();
    services.AddScoped<IProductRepo, ProductRepository>();
    services.AddScoped(provider =>
    {
        var productService = provider.GetRequiredService<IProductService>();
        var fileService = provider.GetRequiredService<IFileService>();
        return new ProductController(productService, fileService);
    });

    // Review Services
    services.AddScoped<IReviewService, ReviewService>();
    services.AddScoped<IReviewRepo, ReviewRepository>();
    services.AddScoped(provider =>
    {
        var reviewService = provider.GetRequiredService<IReviewService>();
        var fileService = provider.GetRequiredService<IFileService>();
        return new ReviewController(reviewService);
    });

    // FluentValidation
    services.AddScoped<IValidator<CreateOrUpdateCategoryDto>, CreateOrUpdateCategoryDtoValidator>();
    services.AddScoped<IValidator<PartialUpdateCategoryDto>, PartialUpdateCategoryDtoValidator>();
    services.AddScoped<IValidator<CreateOrUpdateProductDto>, CreateOrUpdateProductDtoValidator>();
    services.AddScoped<IValidator<PartialUpdateProductDto>, PartialUpdateProductDtoValidator>();
    services.AddScoped<IValidator<CreateOrUpdateReviewDto>, CreateOrUpdateReviewDtoValidator>();
    services.AddScoped<IValidator<PartialUpdateReviewDto>, PartialUpdateReviewDtoValidator>();
    services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
    services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
    services.AddFluentValidationAutoValidation(options =>
    {
        options.DisableBuiltInModelValidation = true;
    });

    // Custom Middleware
    services.AddScoped<ExceptionHandlerMiddleware>();

    // Salt
    services.AddScoped<ISaltRepo, SaltRepository>();

    // Token
    services.AddScoped<ITokenService, TokenService>();

    // Password Hasher
    services.AddScoped<IPasswordHasher, PasswordHasher>();

    // Auth  
    services.AddScoped<IAuthRepo, AuthRepository>();
    services.AddScoped(provider => new AuthController(
        provider.GetRequiredService<IAuthService>(),
        provider.GetRequiredService<IFileService>()
    ));
    services.AddScoped<IAuthService, AuthService>(provider =>
    {
        return new AuthService(
            provider.GetRequiredService<IUserRepo>(),
            provider.GetRequiredService<IPasswordHasher>(),
            provider.GetRequiredService<ITokenService>(),
            provider.GetRequiredService<IAuthRepo>(),
            provider.GetRequiredService<ISaltRepo>()
        );
    });

    // JWT Authentication
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

    // User Services
    services.AddScoped<IUserRepo, UserRepository>();
    services.AddScoped<IUserService, UserService>();
    services.AddScoped(provider =>
    {
        return new UserController(
            provider.GetRequiredService<IUserService>(),
            provider.GetRequiredService<IAuthorizationService>(),
            provider.GetRequiredService<IUserRepo>()
        );
    });

    // Authorization
    services.AddSingleton<IAuthorizationHandler, CartItemAuthorizationHandler>();
    services.AddSingleton<IAuthorizationHandler, OrderAuthorizationHandler>();
    services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();

    services.AddAuthorizationBuilder()
        .AddPolicy("Ownership", policy =>
            policy.Requirements.Add(new OwnershipAuthorizationRequirement()));
}

void Configure(WebApplication app, IHostEnvironment env)
{
    // Use CORS before other middleware
    app.UseCors();

    if (env.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<ExceptionHandlerMiddleware>();
    app.MapControllers();
}

public partial class Program { }