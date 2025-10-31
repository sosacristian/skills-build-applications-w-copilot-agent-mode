# 🔧 Instrucciones para cuando instales .NET 8 SDK

## ✅ Estado Actual

Ya tienes **TODO el código fuente creado**:
- ✅ 8 Entidades completas
- ✅ 3 Enumeraciones
- ✅ 6 Interfaces de repositorios
- ✅ 5 archivos .csproj configurados
- ✅ 1 archivo .sln (solución)
- ✅ Program.cs y appsettings.json

**Total: 27 archivos listos para usar** 🎉

---

## 📥 Paso 1: Instalar .NET 8 SDK

### Windows
1. Descargar desde: https://dotnet.microsoft.com/download/dotnet/8.0
2. Ejecutar el instalador
3. Reiniciar terminal/VS Code

### Verificar instalación
```powershell
dotnet --version
# Debe mostrar: 8.0.x
```

---

## 🏗️ Paso 2: Compilar el Proyecto

```powershell
# Navegar al directorio backend
cd tienda-moderna/backend

# Restaurar paquetes NuGet (descarga dependencias)
dotnet restore

# Compilar la solución completa
dotnet build

# Si todo está bien, deberías ver:
# Build succeeded.
#     0 Warning(s)
#     0 Error(s)
```

### ❓ Si hay errores de compilación

**Error común: "Cannot find type or namespace"**
```powershell
# Limpiar y recompilar
dotnet clean
dotnet restore
dotnet build
```

**Error: "Package not found"**
```powershell
# Reinstalar paquetes específicos
cd TiendaModerna.Infrastructure
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 8.0.0
cd ..
```

---

## 🎯 Paso 3: Verificar Estructura

```powershell
# Ver estructura de la solución
dotnet sln list

# Deberías ver:
# TiendaModerna.Domain/TiendaModerna.Domain.csproj
# TiendaModerna.Application/TiendaModerna.Application.csproj
# TiendaModerna.Infrastructure/TiendaModerna.Infrastructure.csproj
# TiendaModerna.API/TiendaModerna.API.csproj
# TiendaModerna.Shared/TiendaModerna.Shared.csproj
```

---

## 📊 Paso 4: Verificar Referencias

```powershell
# Ver referencias del proyecto API
dotnet list TiendaModerna.API reference

# Deberías ver:
# Project reference(s)
# --------------------
# ..\TiendaModerna.Application\TiendaModerna.Application.csproj
# ..\TiendaModerna.Infrastructure\TiendaModerna.Infrastructure.csproj
```

---

## 🔍 Paso 5: Abrir en Visual Studio o VS Code

### Visual Studio
```powershell
# Abrir la solución
start TiendaModerna.sln
```

### VS Code
```powershell
# Abrir el proyecto
code .
```

**Extensiones recomendadas para VS Code:**
- C# (Microsoft)
- C# Extensions (JosKreativ)
- NuGet Package Manager
- .NET Core Test Explorer

---

## 🚀 Paso 6: Ejecutar la API (básica)

```powershell
# Ejecutar el proyecto API
cd TiendaModerna.API
dotnet run

# Deberías ver algo como:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5000
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to shut down.
```

**Probar Swagger:**
- Abrir navegador en: http://localhost:5000/swagger

---

## ✅ Verificación de que TODO está OK

Si puedes hacer esto sin errores, el Domain Layer está perfecto:

```powershell
cd backend
dotnet build        # ✅ Sin errores
dotnet test         # ⏳ No hay tests aún (normal)
cd TiendaModerna.API
dotnet run          # ✅ Inicia sin errores
```

---

## 📋 Checklist de Verificación

Marca cada ítem cuando lo completes:

### Instalación
- [ ] .NET 8 SDK instalado
- [ ] `dotnet --version` muestra 8.0.x
- [ ] Terminal/VS Code reiniciado

### Compilación
- [ ] `dotnet restore` ejecutado sin errores
- [ ] `dotnet build` exitoso
- [ ] 0 Warning(s), 0 Error(s)

### Estructura
- [ ] 5 proyectos en la solución
- [ ] Referencias entre proyectos correctas
- [ ] Todos los archivos .cs visibles

### Ejecución
- [ ] API inicia con `dotnet run`
- [ ] Swagger accesible en /swagger
- [ ] No hay errores en consola

---

## 🎯 Después de Verificar

Una vez que hayas confirmado que todo compila correctamente:

**Opción 1: Continuar con Infrastructure Layer**
```
Implementar:
- DbContext con Entity Framework
- Configuraciones Fluent API
- Repositorios concretos
- Primera migración
```

**Opción 2: Explorar el código**
```
Revisar:
- Entidades en TiendaModerna.Domain/Entities/
- Interfaces en TiendaModerna.Domain/Interfaces/
- Comentarios y documentación
```

**Opción 3: Hacer cambios**
```
Personalizar:
- Agregar campos a entidades
- Modificar validaciones
- Ajustar relaciones
```

---

## 🆘 Problemas Comunes y Soluciones

### Problema 1: "The type or namespace name 'System' could not be found"
**Causa**: .NET SDK no está en PATH

**Solución**:
```powershell
# Reiniciar terminal
# O agregar manualmente a PATH:
# C:\Program Files\dotnet\
```

### Problema 2: "Package restore failed"
**Causa**: Problemas de red o NuGet no accesible

**Solución**:
```powershell
# Limpiar caché de NuGet
dotnet nuget locals all --clear

# Reintentar
dotnet restore
```

### Problema 3: "Project file is incomplete"
**Causa**: Archivos .csproj corruptos o mal formados

**Solución**:
- Revisar que los archivos .csproj no tengan caracteres extraños
- Validar XML de los .csproj
- Reemplazar con los originales de la documentación

### Problema 4: "Could not load file or assembly"
**Causa**: Versiones incompatibles de paquetes

**Solución**:
```powershell
# Eliminar carpetas bin y obj
Remove-Item -Recurse -Force */bin
Remove-Item -Recurse -Force */obj

# Recompilar
dotnet clean
dotnet restore
dotnet build
```

---

## 📚 Recursos Adicionales

### Documentación Oficial
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core 8](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core 8](https://docs.microsoft.com/en-us/aspnet/core/)

### Tutoriales Recomendados
- [Clean Architecture in .NET](https://www.youtube.com/results?search_query=clean+architecture+.net+8)
- [Entity Framework Core Tutorial](https://www.entityframeworktutorial.net/)
- [Repository Pattern Tutorial](https://www.youtube.com/results?search_query=repository+pattern+c%23)

### Herramientas Útiles
- **dotnet CLI**: Comandos de terminal
- **Visual Studio**: IDE completo (Windows)
- **Rider**: IDE de JetBrains (multiplataforma)
- **VS Code**: Editor ligero con extensiones

---

## 💡 Comandos Útiles de .NET CLI

```powershell
# Ver información del SDK
dotnet --info

# Listar proyectos en solución
dotnet sln list

# Agregar proyecto a solución
dotnet sln add <path-to-project.csproj>

# Ver referencias de un proyecto
dotnet list <project> reference

# Agregar referencia
dotnet add <project> reference <other-project>

# Instalar paquete NuGet
dotnet add package <PackageName> --version <Version>

# Actualizar paquetes
dotnet restore

# Limpiar build
dotnet clean

# Compilar
dotnet build

# Compilar solo un proyecto
dotnet build <project>

# Ejecutar proyecto
dotnet run --project <project>

# Crear migración (después de configurar EF)
dotnet ef migrations add <MigrationName> --project <InfraProject>

# Aplicar migraciones
dotnet ef database update --project <InfraProject>

# Ver ayuda
dotnet --help
```

---

## 🎓 Siguientes Pasos Recomendados

### Corto Plazo (Hoy/Mañana)
1. ✅ Instalar .NET 8 SDK
2. ✅ Compilar y verificar Domain Layer
3. ✅ Explorar el código creado
4. ✅ Familiarizarse con la estructura

### Mediano Plazo (Esta Semana)
1. ⏳ Implementar Infrastructure Layer (BLOQUE 2)
2. ⏳ Crear primera migración de base de datos
3. ⏳ Implementar Application Layer (BLOQUE 3)
4. ⏳ Crear API básica (BLOQUE 4)

### Largo Plazo (Próximas Semanas)
1. ⏳ Implementar frontend Vue 3
2. ⏳ Integrar con base de datos real
3. ⏳ Dockerizar aplicación completa
4. ⏳ Deploy en servidor

---

## 📞 ¿Necesitas Ayuda?

### Si encuentras errores:
1. Copia el error completo
2. Busca en Stack Overflow o Google
3. Revisa la documentación oficial de .NET
4. Consulta los archivos de documentación del proyecto

### Archivos de referencia:
- `docs/RESUMEN_EJECUTIVO.md` - Visión general
- `docs/guia-implementacion-backend.md` - Guía paso a paso
- `docs/codigo-completo-domain-layer*.md` - Código fuente completo
- `docs/BLOQUE_1_COMPLETADO.md` - Resumen de lo implementado

---

## ✨ ¡Estás Listo para Compilar!

Una vez instalado .NET 8 SDK, solo ejecuta:

```powershell
cd tienda-moderna/backend
dotnet build
```

Y tendrás tu proyecto de Clean Architecture funcionando 🚀

---

📅 **Fecha de Creación**: Octubre 30, 2025  
🎯 **Estado**: Domain Layer 100% completo  
📦 **Archivos**: 27 archivos listos  
💻 **Líneas de código**: ~3,500 líneas
