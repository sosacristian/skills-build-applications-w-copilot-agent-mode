# ⚡ Guía de Inicio Rápido - Tienda Moderna

Esta guía te llevará de cero a un proyecto funcionando en **15 minutos**.

---

## ✅ Pre-requisitos

Antes de comenzar, asegúrate de tener instalado:

- [ ] **.NET 8 SDK** → [Descargar](https://dotnet.microsoft.com/download/dotnet/8.0)
- [ ] **Visual Studio 2022** o **VS Code** → [VS](https://visualstudio.microsoft.com/) | [VS Code](https://code.visualstudio.com/)
- [ ] **MySQL 8.0** (o usar Docker) → [Descargar](https://dev.mysql.com/downloads/) | [Docker](https://www.docker.com/)
- [ ] **Git** → [Descargar](https://git-scm.com/)

### Verificar Instalación

```bash
# Verificar .NET
dotnet --version
# Debe mostrar: 8.0.x o superior

# Verificar MySQL (si no usas Docker)
mysql --version

# Verificar Git
git --version
```

---

## 🚀 Paso 1: Obtener el Proyecto (2 min)

```bash
# Si ya tienes el proyecto:
cd skills-build-applications-w-copilot-agent-mode/tienda-moderna

# O si es un nuevo repo:
git clone <url-del-repo>
cd tienda-moderna
```

---

## 📖 Paso 2: Leer Documentación Esencial (5 min)

Lee estos 2 documentos antes de continuar:

```bash
# 1. Resumen Ejecutivo (3 minutos)
docs/RESUMEN_EJECUTIVO.md

# 2. Índice de Documentación (2 minutos)
docs/INDICE.md
```

**¿Por qué leer primero?**
- Entenderás la arquitectura completa
- Sabrás qué estás construyendo
- Conocerás las decisiones técnicas tomadas

---

## 🏗️ Paso 3: Crear Estructura .NET (3 min)

```bash
# Navegar a backend
cd backend

# Crear solución
dotnet new sln -n TiendaModerna

# Crear proyectos de cada capa
dotnet new classlib -n TiendaModerna.Domain -f net8.0
dotnet new classlib -n TiendaModerna.Application -f net8.0
dotnet new classlib -n TiendaModerna.Infrastructure -f net8.0
dotnet new webapi -n TiendaModerna.API -f net8.0
dotnet new classlib -n TiendaModerna.Shared -f net8.0

# Agregar proyectos a la solución
dotnet sln add TiendaModerna.Domain/TiendaModerna.Domain.csproj
dotnet sln add TiendaModerna.Application/TiendaModerna.Application.csproj
dotnet sln add TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
dotnet sln add TiendaModerna.API/TiendaModerna.API.csproj
dotnet sln add TiendaModerna.Shared/TiendaModerna.Shared.csproj

# Establecer referencias entre proyectos
cd TiendaModerna.Application
dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj
cd ..

cd TiendaModerna.Infrastructure
dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj
dotnet add reference ../TiendaModerna.Application/TiendaModerna.Application.csproj
cd ..

cd TiendaModerna.API
dotnet add reference ../TiendaModerna.Application/TiendaModerna.Application.csproj
dotnet add reference ../TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
cd ..

# Verificar que compila
dotnet build
```

**Resultado esperado**: `Build succeeded. 0 Warning(s), 0 Error(s)`

---

## 💻 Paso 4: Copiar Código del Domain Layer (3 min)

### Opción A: Manual (Recomendado para aprender)

1. Abre `docs/codigo-completo-domain-layer.md`
2. Crea las carpetas en `TiendaModerna.Domain/`:
   - `Entities/`
   - `Enums/`
   - `Interfaces/`
3. Copia cada clase C# del documento a su archivo correspondiente

### Opción B: Script Rápido (PowerShell)

```powershell
# Ejecutar desde: tienda-moderna/backend/

# Crear estructura de carpetas
New-Item -ItemType Directory -Path "TiendaModerna.Domain/Entities" -Force
New-Item -ItemType Directory -Path "TiendaModerna.Domain/Enums" -Force
New-Item -ItemType Directory -Path "TiendaModerna.Domain/Interfaces" -Force

# Nota: Deberás copiar manualmente el código de los documentos
# Los archivos .cs no se pueden generar automáticamente desde Markdown
```

**Archivos a crear**:

**Entities/** (desde `codigo-completo-domain-layer.md` y `parte2.md`):
- [ ] `Producto.cs`
- [ ] `Categoria.cs`
- [ ] `Variante.cs`
- [ ] `Imagen.cs`
- [ ] `Marca.cs`
- [ ] `Orden.cs`
- [ ] `DetalleOrden.cs`
- [ ] `Usuario.cs`

**Enums/** (desde `codigo-completo-domain-layer-parte2.md`):
- [ ] `EstadoOrden.cs`
- [ ] `RolUsuario.cs`
- [ ] `TipoDescuento.cs`

**Interfaces/** (desde `codigo-completo-domain-layer-parte3.md`):
- [ ] `IRepositorioGenerico.cs`
- [ ] `IRepositorioProducto.cs`
- [ ] `IRepositorioCategoria.cs`
- [ ] `IRepositorioOrden.cs`
- [ ] `IRepositorioUsuario.cs`
- [ ] `IUnitOfWork.cs`

---

## 🔧 Paso 5: Instalar Paquetes NuGet (2 min)

```bash
# Application Layer
cd TiendaModerna.Application
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.9.0
dotnet add package MediatR --version 12.2.0
cd ..

# Infrastructure Layer
cd TiendaModerna.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package BCrypt.Net-Next --version 4.0.3
dotnet add package EPPlus --version 7.0.0
cd ..

# API Layer
cd TiendaModerna.API
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.File --version 5.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.0
cd ..

# Restaurar y compilar todo
cd ..
dotnet restore
dotnet build
```

**Resultado esperado**: `Build succeeded`

---

## ✅ Verificación Final

```bash
# Desde: tienda-moderna/backend/

# Verificar que la solución compila
dotnet build

# Verificar que los proyectos se crearon correctamente
dotnet sln list
```

**Deberías ver**:
```
TiendaModerna.Domain/TiendaModerna.Domain.csproj
TiendaModerna.Application/TiendaModerna.Application.csproj
TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
TiendaModerna.API/TiendaModerna.API.csproj
TiendaModerna.Shared/TiendaModerna.Shared.csproj
```

---

## 🎉 ¡Felicitaciones!

Has completado la configuración inicial. La estructura del proyecto está lista.

### ✅ Lo que has logrado:

- [x] Creado solución .NET 8 con 5 proyectos
- [x] Establecido referencias entre capas (Clean Architecture)
- [x] Instalado todos los paquetes NuGet necesarios
- [x] Copiado el código del Domain Layer (entidades, enums, interfaces)
- [x] Verificado que todo compila correctamente

### 📋 Estado del Proyecto

| Capa | Estado | Próximo Paso |
|------|--------|--------------|
| **Domain Layer** | ✅ Completo | - |
| **Infrastructure Layer** | ⏳ Pendiente | Crear DbContext |
| **Application Layer** | ⏳ Pendiente | Crear DTOs y servicios |
| **API Layer** | ⏳ Pendiente | Crear controladores |
| **Frontend** | ⏳ Pendiente | Inicializar Vue 3 |

---

## 🚀 Próximos Pasos

### Inmediato (Continuar ahora):

**1. Implementar Infrastructure Layer**

Lee: `docs/guia-implementacion-backend.md` → Sección "FASE 2: Infrastructure Layer"

Tareas:
- Crear `TiendaContext.cs` (DbContext de EF Core)
- Configurar entidades con Fluent API
- Implementar repositorios
- Implementar Unit of Work
- Crear primera migración

**Tiempo estimado**: 2-3 horas

---

**2. Implementar Application Layer**

Lee: `docs/guia-implementacion-backend.md` → Sección "FASE 3: Application Layer"

Tareas:
- Crear DTOs (ProductoDTO, CrearProductoDTO, etc.)
- Configurar AutoMapper profiles
- Implementar servicios de negocio
- Crear validadores con FluentValidation

**Tiempo estimado**: 2-3 horas

---

**3. Implementar API Layer**

Lee: `docs/guia-implementacion-backend.md` → Sección "FASE 4: API Layer"

Tareas:
- Configurar `Program.cs` (DI, Swagger, JWT)
- Crear controladores REST
- Implementar middleware de errores
- Configurar autenticación JWT

**Tiempo estimado**: 2-3 horas

---

### Mediano Plazo (Esta semana):

- [ ] Crear seed de datos iniciales (categorías, productos de prueba)
- [ ] Implementar sistema de importación de Excel
- [ ] Crear servicio de manejo de imágenes
- [ ] Implementar sistema de órdenes completo

---

### Largo Plazo (Próximas semanas):

- [ ] Inicializar frontend Vue 3
- [ ] Crear componentes de catálogo
- [ ] Implementar carrito de compras
- [ ] Integrar con pasarela de pago
- [ ] Deploy con Docker
- [ ] CI/CD con GitLab

---

## 📚 Recursos de Referencia

### Documentación del Proyecto
- **[RESUMEN_EJECUTIVO.md](./docs/RESUMEN_EJECUTIVO.md)** - Vista completa del proyecto
- **[guia-implementacion-backend.md](./docs/guia-implementacion-backend.md)** - Guía detallada paso a paso
- **[INDICE.md](./docs/INDICE.md)** - Índice de toda la documentación

### Documentación Externa
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

---

## 🆘 Problemas Comunes

### "dotnet command not found"
**Solución**: Instalar .NET 8 SDK desde https://dotnet.microsoft.com/download

---

### "No se puede establecer referencia entre proyectos"
**Causa**: Ruta incorrecta en el comando `dotnet add reference`

**Solución**: Asegúrate de estar en la carpeta correcta y usar rutas relativas:
```bash
cd TiendaModerna.Application
dotnet add reference ../TiendaModerna.Domain/TiendaModerna.Domain.csproj
```

---

### "Build failed con errores de namespace"
**Causa**: Código copiado incompleto o en carpetas incorrectas

**Solución**: 
1. Verificar que los archivos estén en las carpetas correctas (Entities/, Enums/, Interfaces/)
2. Verificar que los namespaces coincidan con la estructura de carpetas
3. Recompilar: `dotnet clean && dotnet build`

---

### "No tengo MySQL instalado"
**Solución**: Usar Docker:
```bash
cd tienda-moderna
docker-compose up -d mysql
```

La configuración de MySQL ya está en `docker-compose.yml`

---

## 💡 Consejos

### Para Aprender
- Lee los comentarios en el código, explican el **por qué** de cada decisión
- Implementa capa por capa, no todo a la vez
- Prueba cada capa antes de continuar con la siguiente

### Para Productividad
- Usa Visual Studio con Resharper o Rider (mejor autocompletado)
- Instala extensiones de C# en VS Code si lo prefieres
- Configura snippets para código repetitivo

### Para No Perderte
- Sigue el checklist en `docs/INDICE.md`
- Marca cada tarea completada
- Si te atascas, consulta `docs/RESUMEN_EJECUTIVO.md`

---

## ⏱️ Tiempo Total Estimado

- **Setup Inicial (esta guía)**: ~15 minutos
- **Infrastructure Layer**: 2-3 horas
- **Application Layer**: 2-3 horas
- **API Layer**: 2-3 horas
- **Testing básico**: 1-2 horas
- **Frontend Vue 3**: 5-8 horas

**Total para MVP funcional**: **15-20 horas**

---

## 🎯 Objetivo Final

Al completar todos los pasos, tendrás:

✅ Backend .NET 8 con Clean Architecture  
✅ API REST documentada con Swagger  
✅ Base de datos MySQL con migraciones  
✅ Autenticación JWT  
✅ CRUD completo de productos  
✅ Sistema de órdenes  
✅ Importación de Excel  
✅ Frontend Vue 3 integrado  
✅ Desplegado con Docker  

---

**¿Listo para continuar?** 

👉 **[Ir a guia-implementacion-backend.md](./docs/guia-implementacion-backend.md)**

---

📅 **Última actualización**: Enero 2025  
🚀 **Versión**: 1.0  
💪 **¡Mucho éxito con tu proyecto!**
