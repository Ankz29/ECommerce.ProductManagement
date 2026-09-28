#region using directives

using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Data;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ECommerce.ProductManagement.API.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

#endregion

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];

// Ensure the same signing key derivation is used for token validation as is used when issuing tokens.
byte[] validationKeyBytes;
if (string.IsNullOrWhiteSpace(key))
    throw new InvalidOperationException("JWT key is not configured.");
try
{
    validationKeyBytes = Convert.FromBase64String(key);
}
catch (FormatException)
{
    validationKeyBytes = Encoding.UTF8.GetBytes(key);
}

if (validationKeyBytes.Length * 8 <= 256)
{
    using var deriveBytes = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("ECommerceJwtKeySalt"), 10000, HashAlgorithmName.SHA256);
    validationKeyBytes = deriveBytes.GetBytes(64);
}

// Standardize automatic ModelState invalid responses to a consistent JSON shape
builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problem = new
        {
            status = StatusCodes.Status400BadRequest,
            error = "Bad Request",
            message = "Validation failed",
            errors = context.ModelState,
            traceId = System.Diagnostics.Activity.Current?.Id
        };

        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(problem);
    };
});

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(validationKeyBytes)
        };
    });

// Add the CORS policy- so that the front-end react UI App can access the backend API's.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// DB Connection for EF-DB Migrations
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository registry of services
builder.Services.AddScoped<IProductServiceAgent, ProductServiceAgent>();
builder.Services.AddScoped<ICategoryServiceAgent, CategoryServiceAgent>();
builder.Services.AddScoped<IInventoryServiceAgent, InventoryServiceAgent>();

// Register the services
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<InventoryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ECommerce API", Version = "v1" });

    // Add JWT Bearer definition
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token.\nExample: \"Bearer eyJhbGciOiJI...\""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Add global exception handling middleware early in the pipeline so it can catch errors
// from authentication, authorization, and controllers.
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
