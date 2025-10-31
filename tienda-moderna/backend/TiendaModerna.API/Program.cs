using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TiendaModerna.Application.Interfaces;
using TiendaModerna.Application.Services;
using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;
using TiendaModerna.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ============ CONFIGURACIÓN DE BASE DE DATOS ============
builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 0)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)
    )
);

// ============ CONFIGURACIÓN DE REPOSITORIOS (INFRASTRUCTURE) ============
builder.Services.AddScoped<IRepositorioProducto, RepositorioProducto>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioOrden, RepositorioOrden>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ============ CONFIGURACIÓN DE SERVICIOS (APPLICATION) ============
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IOrdenService, OrdenService>();

// ============ CONFIGURACIÓN DE AUTOMAPPER ============
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ============ CONFIGURACIÓN DE JWT ============
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer no configurado");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience no configurado");

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
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

// ============ CONFIGURACIÓN DE CORS ============
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ============ CONFIGURACIÓN DE CONTROLLERS Y JSON ============
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// ============ CONFIGURACIÓN DE SWAGGER ============
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tienda Moderna API",
        Version = "v1",
        Description = "API RESTful para plataforma de venta de indumentaria",
        Contact = new OpenApiContact
        {
            Name = "Equipo Tienda Moderna",
            Email = "contacto@tiendamoderna.com"
        }
    });

    // Configuración de JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ============ CONFIGURACIÓN DEL PIPELINE HTTP ============

// Swagger siempre disponible (desarrollo y producción)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tienda Moderna API v1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
});

// Middleware de manejo de errores global
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

// CORS debe ir antes de Authentication y Authorization
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Endpoint de health check
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithTags("Health");

app.Run();
