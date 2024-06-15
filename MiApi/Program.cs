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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text; 

var builder = WebApplication.CreateBuilder(args);




//Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Service
builder.Services.AddKeyedScoped<ICommonService<ProductDto, ProductInsertDto, ProductUpdateDto>, ProductService>("productService");
builder.Services.AddKeyedScoped<ICommonService<CategoryDto, CategoryInsertDto, CategoryUpdateDto>, CategoryService>("categoryService");
builder.Services.AddKeyedScoped<ICommonService<WarehouseDto, WarehouseInsertDto, WarehouseUpdateDto>, WarehouseService>("warehouseService");
builder.Services.AddKeyedScoped<ICommonService<UserDto, UserInsertDto, UserUpdateDto>, UserService>("userService");


//Repository
builder.Services.AddKeyedScoped<IRepository<Product>, ProductRepository>("productRepository");
builder.Services.AddKeyedScoped<IRepository<Category>, CategoryRepository>("categoryRepository");
builder.Services.AddKeyedScoped<IRepository<Warehouse>, WarehouseRepository>("warehouseRepository");
builder.Services.AddKeyedScoped<IRepository<User>, UserRepository>("userRepository");

//Database Local EntityFramework
builder.Services.AddDbContext<InventoryContext>(context =>
{
    context.UseSqlServer(builder.Configuration.GetConnectionString("InventoryConnection"));
});

//Somee HostingDB  EntityFramework
/*builder.Services.AddDbContext<InventoryContext>(context =>
{
    context.UseSqlServer(builder.Configuration.GetConnectionString("SomeeHostingInventory"));
});*/


//Validators
builder.Services.AddScoped<IValidator<ProductInsertDto>, ProductInsertValidator>();
builder.Services.AddScoped<IValidator<ProductUpdateDto>, ProductUpdateValidator>();
builder.Services.AddScoped<IValidator<CategoryInsertDto>, CategoryInsertValidator>();
builder.Services.AddScoped<IValidator<CategoryUpdateDto>, CategoryUpdateValidator>();
builder.Services.AddScoped<IValidator<WarehouseInsertDto>, WarehouseInsertValidator>();
builder.Services.AddScoped<IValidator<WarehouseUpdateDto>, WarehouseUpdateValidator>();


// Add services to the container. 

//Agregando el JWT --2
builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = builder.Configuration.GetSection("settings").GetSection("secretKey").ToString();

var keyBytes = Encoding.UTF8.GetBytes(secretKey);


builder.Services.AddAuthentication(b =>
{
    b.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    b.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer( b =>
{
    b.RequireHttpsMetadata = false;
    b.SaveToken = true;
    b.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
////////////////////////////////////////////////////////////


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregando para Conectarse a Una Api ----1
var proveedor = builder.Services.BuildServiceProvider();
var configuration = proveedor.GetRequiredService<IConfiguration>();

builder.Services.AddCors(b =>
{
    var frontendUrl = configuration.GetValue<string>("Frontend_url");

    b.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendUrl).AllowAnyMethod().AllowAnyHeader();
    });
});

////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//---1
app.UseCors();


//---2
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
