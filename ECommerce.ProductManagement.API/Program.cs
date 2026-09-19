using ECommerce.ProductManagement.Core.Services;
using ECommerce.ProductManagement.Infrastructure.Data;
using ECommerce.ProductManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
