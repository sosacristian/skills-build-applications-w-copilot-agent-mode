# 🏗️ Guía de Implementación del Backend - Tienda Moderna

## 📝 Tabla de Contenidos
1. [Creación de Proyectos](#creación-de-proyectos)
2. [Estructura de Clean Architecture](#estructura-de-clean-architecture)
3. [Implementación por Capas](#implementación-por-capas)
4. [Comandos de Instalación](#comandos-de-instalación)

---

## 🎯 Creación de Proyectos

### Paso 1: Crear la Solución

```bash
# Navegar al directorio backend
cd tienda-moderna/backend

# Crear la solución principal
dotnet new sln -n TiendaModerna
```

**DECISIÓN ARQUITECTÓNICA**: 
- Una solución agrupa múltiples proyectos relacionados
- Facilita la gestión de dependencias entre capas
- Permite compilar todo el sistema de una vez

---

### Paso 2: Crear los Proyectos de cada Capa

```bash
# CAPA DE DOMINIO (Domain Layer)
# ¿Por qué primero? Porque es el núcleo del negocio, sin dependencias externas
dotnet new classlib -n TiendaModerna.Domain -f net8.0

# CAPA DE APLICACIÓN (Application Layer)
# ¿Por qué? Contiene la lógica de negocio y depende solo del dominio
dotnet new classlib -n TiendaModerna.Application -f net8.0

# CAPA DE INFRAESTRUCTURA (Infrastructure Layer)
# ¿Por qué? Implementa los detalles técnicos (BD, archivos, APIs externas)
dotnet new classlib -n TiendaModerna.Infrastructure -f net8.0

# CAPA DE PRESENTACIÓN (Presentation Layer - API)
# ¿Por qué al final? Es el punto de entrada, depende de todas las demás capas
dotnet new webapi -n TiendaModerna.API -f net8.0

# PROYECTO COMPARTIDO (Shared)
# ¿Por qué? Contiene utilidades que pueden usar todas las capas
dotnet new classlib -n TiendaModerna.Shared -f net8.0
```

---

### Paso 3: Agregar Proyectos a la Solución

```bash
# Agregar todos los proyectos a la solución
dotnet sln add TiendaModerna.Domain/TiendaModerna.Domain.csproj
dotnet sln add TiendaModerna.Application/TiendaModerna.Application.csproj
dotnet sln add TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
dotnet sln add TiendaModerna.API/TiendaModerna.API.csproj
dotnet sln add TiendaModerna.Shared/TiendaModerna.Shared.csproj
```

---

### Paso 4: Establecer Referencias entre Proyectos

**IMPORTANTE**: El orden de las referencias define la dirección de las dependencias

```bash
# APPLICATION depende de DOMAIN
cd TiendaModerna.Application
dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj
cd ..

# INFRASTRUCTURE depende de DOMAIN y APPLICATION
cd TiendaModerna.Infrastructure
dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj
dotnet add reference ../TiendaModerna.Application/TiendaModerna.Application.csproj
cd ..

# API depende de APPLICATION e INFRASTRUCTURE
cd TiendaModerna.API
dotnet add reference ../TiendaModerna.Application/TiendaModerna.Application.csproj
dotnet add reference ../TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
cd ..
```

**¿POR QUÉ ESTA ESTRUCTURA?**
```
┌─────────────────┐
│   TiendaModerna.API   │  ← Punto de entrada HTTP
└────────┬────────┘
         │
         ├──→ ┌──────────────────────┐
         │    │  Application Layer    │  ← Lógica de negocio
         │    └──────────┬───────────┘
         │               │
         └──→ ┌──────────┴───────────┐
              │  Infrastructure Layer │  ← Acceso a datos, servicios externos
              └──────────┬───────────┘
                         │
                    ┌────┴─────┐
                    │  Domain  │  ← Núcleo del negocio (sin dependencias)
                    └──────────┘
```

---

## 🧱 Estructura de Clean Architecture

### Principios Fundamentales

1. **Independencia de Frameworks**: La lógica de negocio no depende de EF Core, ASP.NET, etc.
2. **Testeable**: Cada capa se puede probar independientemente
3. **Independencia de la UI**: Se puede cambiar el frontend sin tocar el backend
4. **Independencia de la BD**: Se puede cambiar de MySQL a PostgreSQL sin afectar la lógica
5. **Independencia de Agentes Externos**: Las reglas de negocio no conocen el mundo exterior

---

## 📦 Paquetes NuGet Necesarios

### TiendaModerna.Domain
```bash
# Este proyecto NO debe tener dependencias externas
# Solo clases POCO (Plain Old CLR Objects)
# Representa el modelo de negocio puro
```

**¿POR QUÉ SIN DEPENDENCIAS?**
- El dominio es la parte más estable del sistema
- No debe cambiar por razones técnicas (cambio de BD, framework, etc.)
- Es reutilizable en otros proyectos

---

### TiendaModerna.Application
```bash
cd TiendaModerna.Application

# AutoMapper - Mapeo entre entidades y DTOs
# ¿Por qué? Evita exponer entidades de dominio directamente al cliente
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1

# FluentValidation - Validaciones expresivas
# ¿Por qué? Separa validaciones de la lógica de negocio, más legible que atributos
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.9.0

# MediatR - Patrón Mediator (opcional pero recomendado)
# ¿Por qué? Desacopla comandos/queries de sus manejadores
dotnet add package MediatR --version 12.2.0
```

**DECISIÓN**: FluentValidation vs Data Annotations
- FluentValidation: Más flexible, testeable, separa responsabilidades (✓ Elegido)
- Data Annotations: Más simple pero mezcla validación con modelo

---

### TiendaModerna.Infrastructure
```bash
cd TiendaModerna.Infrastructure

# Entity Framework Core - ORM
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Provider MySQL (Pomelo es el más maduro y mantenido)
# ¿Por qué Pomelo? Mejor rendimiento y soporte que el oficial de Oracle
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.0

# Para migraciones
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0

# Bcrypt para encriptar contraseñas
# ¿Por qué Bcrypt? Diseñado específicamente para contraseñas, resistente a ataques
dotnet add package BCrypt.Net-Next --version 4.0.3

# EPPlus para manejo de Excel
# ¿Por qué EPPlus? Mejor rendimiento que ClosedXML, más funcionalidades
dotnet add package EPPlus --version 7.0.0
```

**DECISIÓN**: Pomelo.EntityFrameworkCore.MySql vs MySql.EntityFrameworkCore
- Pomelo: Mejor rendimiento, más características, comunidad activa (✓ Elegido)
- MySql oficial: Menos optimizado, desarrollo más lento

---

### TiendaModerna.API
```bash
cd TiendaModerna.API

# Swagger/OpenAPI - Documentación automática de API
dotnet add package Swashbuckle.AspNetCore --version 6.5.0

# Authentication JWT
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0

# CORS - Para permitir peticiones desde el frontend
# Ya viene incluido en ASP.NET Core 8

# Serilog - Logging estructurado (mejor que el default)
# ¿Por qué Serilog? Logs más ricos, múltiples destinos, filtros avanzados
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.File --version 5.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.0
```

---

## 🗂️ Estructura Detallada de Carpetas

```
backend/
├── TiendaModerna.Domain/
│   ├── Entities/                    # Entidades del dominio
│   │   ├── Producto.cs
│   │   ├── Categoria.cs
│   │   ├── Variante.cs
│   │   ├── Orden.cs
│   │   ├── DetalleOrden.cs
│   │   ├── Usuario.cs
│   │   ├── Marca.cs
│   │   └── Imagen.cs
│   ├── Enums/                       # Enumeraciones
│   │   ├── EstadoOrden.cs
│   │   ├── RolUsuario.cs
│   │   └── TipoDescu ento.cs
│   └── Interfaces/                  # Contratos de repositorios
│       ├── IRepositorioGenerico.cs
│       ├── IRepositorioProducto.cs
│       ├── IRepositorioCategoria.cs
│       └── IUnitOfWork.cs
│
├── TiendaModerna.Application/
│   ├── DTOs/                        # Data Transfer Objects
│   │   ├── Productos/
│   │   │   ├── ProductoDTO.cs
│   │   │   ├── CrearProductoDTO.cs
│   │   │   └── ActualizarProductoDTO.cs
│   │   ├── Categorias/
│   │   ├── Ordenes/
│   │   └── Usuarios/
│   ├── Services/                    # Servicios de lógica de negocio
│   │   ├── Interfaces/
│   │   │   ├── IServicioProducto.cs
│   │   │   ├── IServicioCategoria.cs
│   │   │   ├── IServicioOrden.cs
│   │   │   └── IServicioImportacion.cs
│   │   └── Implementations/
│   │       ├── ServicioProducto.cs
│   │       ├── ServicioCategoria.cs
│   │       ├── ServicioOrden.cs
│   │       └── ServicioImportacion.cs
│   ├── Mappings/                    # Perfiles de AutoMapper
│   │   └── MappingProfile.cs
│   └── Validators/                  # Validadores FluentValidation
│       ├── ProductoValidator.cs
│       ├── CategoriaValidator.cs
│       └── OrdenValidator.cs
│
├── TiendaModerna.Infrastructure/
│   ├── Data/                        # Configuración de base de datos
│   │   ├── TiendaContext.cs        # DbContext principal
│   │   └── Configurations/          # Configuraciones de entidades
│   │       ├── ProductoConfiguration.cs
│   │       ├── CategoriaConfiguration.cs
│   │       └── OrdenConfiguration.cs
│   ├── Repositories/                # Implementaciones de repositorios
│   │   ├── RepositorioGenerico.cs
│   │   ├── RepositorioProducto.cs
│   │   ├── RepositorioCategoria.cs
│   │   └── UnitOfWork.cs
│   └── Services/                    # Servicios de infraestructura
│       ├── ServicioArchivos.cs      # Para manejar uploads de imágenes
│       └── ServicioEmail.cs         # Para enviar emails
│
├── TiendaModerna.API/
│   ├── Controllers/                 # Controladores REST
│   │   ├── ProductosController.cs
│   │   ├── CategoriasController.cs
│   │   ├── OrdenesController.cs
│   │   └── AutenticacionController.cs
│   ├── Middlewares/                 # Middleware personalizado
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   └── RequestLoggingMiddleware.cs
│   ├── Extensions/                  # Métodos de extensión
│   │   └── ServiceCollectionExtensions.cs
│   ├── Program.cs                   # Punto de entrada
│   └── appsettings.json            # Configuración
│
└── TiendaModerna.Shared/
    ├── Constants/                   # Constantes del sistema
    │   ├── RolesConstantes.cs
    │   └── MensajesConstantes.cs
    ├── Exceptions/                  # Excepciones personalizadas
    │   ├── ExcepcionNegocio.cs
    │   └── ExcepcionNoEncontrado.cs
    └── Helpers/                     # Clases auxiliares
        ├── PasswordHelper.cs
        └── PaginacionHelper.cs
```

---

## 🎯 Orden de Implementación Recomendado

### FASE 1: Fundamentos (Domain Layer)
1. Crear entidades básicas (Producto, Categoria, Usuario)
2. Definir enumeraciones
3. Crear interfaces de repositorios

### FASE 2: Infraestructura (Infrastructure Layer)
1. Configurar DbContext
2. Implementar repositorios
3. Implementar Unit of Work
4. Crear primera migración

### FASE 3: Lógica de Negocio (Application Layer)
1. Crear DTOs
2. Configurar AutoMapper
3. Implementar servicios básicos
4. Agregar validadores

### FASE 4: API (Presentation Layer)
1. Configurar Program.cs (DI, Swagger, JWT)
2. Crear controladores básicos
3. Implementar manejo de errores
4. Agregar autenticación

### FASE 5: Características Avanzadas
1. Sistema de descuentos
2. Importación de Excel
3. Manejo de imágenes
4. Sistema de órdenes completo

---

## 📝 Comandos de Resumen

```bash
# 1. Crear solución y proyectos
dotnet new sln -n TiendaModerna
dotnet new classlib -n TiendaModerna.Domain -f net8.0
dotnet new classlib -n TiendaModerna.Application -f net8.0
dotnet new classlib -n TiendaModerna.Infrastructure -f net8.0
dotnet new webapi -n TiendaModerna.API -f net8.0
dotnet new classlib -n TiendaModerna.Shared -f net8.0

# 2. Agregar a la solución
dotnet sln add **/*.csproj

# 3. Establecer referencias
cd TiendaModerna.Application && dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj
cd ../TiendaModerna.Infrastructure && dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj ../TiendaModerna.Application/TiendaModerna.Application.csproj
cd ../TiendaModerna.API && dotnet add reference ../TiendaModerna.Application/TiendaModerna.Application.csproj ../TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
cd ..

# 4. Restaurar y compilar
dotnet restore
dotnet build

# 5. Ejecutar
dotnet run --project TiendaModerna.API
```

---

## 🎓 Próximos Pasos

Una vez creada la estructura, seguiremos con:
1. ✅ Implementación de entidades del dominio
2. ✅ Configuración de Entity Framework y migraciones
3. ✅ Implementación de repositorios
4. ✅ Creación de servicios
5. ✅ Desarrollo de controladores

**Nota**: Cada archivo de código incluirá comentarios detallados en español explicando las decisiones arquitectónicas.
