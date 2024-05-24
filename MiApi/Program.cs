using AutoMapper;
using FluentValidation;
using MiApi.DTOs;
using MiApi.Mappers;
using MiApi.Models;
using MiApi.Repository;
using MiApi.Service;
using MiApi.Validators;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);




//Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Service
builder.Services.AddKeyedScoped<ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto>, ProductService>("productService");
builder.Services.AddKeyedScoped<ICommonService<CategoryDto, CategoryInsertDto, CategoryUpdateDto>, CategoryService>("categoryService");
builder.Services.AddKeyedScoped<ICommonService<WarehouseDto, WarehouseInsertDto, WarehouseUpdateDto>, WarehouseService>("warehouseService");

//Repository
builder.Services.AddKeyedScoped<IRepository<Product>, ProductRepository>("productRepository");
builder.Services.AddKeyedScoped<IRepository<Category>, CategoryRepository>("categoryRepository");
builder.Services.AddKeyedScoped<IRepository<Warehouse>, WarehouseRepository>("warehouseRepository");

//Entity Framework
builder.Services.AddDbContext<InventoryContext>(context =>
{
    context.UseSqlServer(builder.Configuration.GetConnectionString("InventoryConnection"));
});

//Validators
builder.Services.AddScoped<IValidator<ProductInsertDto>, ProductInsertValidator>();
builder.Services.AddScoped<IValidator<ProductUpdateDto>, ProductUpdateValidator>();
builder.Services.AddScoped<IValidator<CategoryInsertDto>, CategoryInsertValidator>();
builder.Services.AddScoped<IValidator<CategoryUpdateDto>, CategoryUpdateValidator>();
builder.Services.AddScoped<IValidator<WarehouseInsertDto>, WarehouseInsertValidator>();
builder.Services.AddScoped<IValidator<WarehouseUpdateDto>, WarehouseUpdateValidator>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
