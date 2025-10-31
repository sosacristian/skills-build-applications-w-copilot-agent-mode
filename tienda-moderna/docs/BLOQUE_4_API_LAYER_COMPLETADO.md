# BLOQUE 4 - API Layer COMPLETADO ✅

**Fecha de completado:** 31 de octubre de 2025

## Resumen

La capa API está completamente implementada y compilando sin errores. Se configuró un API REST completo con autenticación JWT, documentación Swagger, CORS, y controladores para las operaciones principales.

---

## Archivos Creados/Modificados

### 1. Program.cs - Configuración Principal (~150 líneas)

**Configuraciones implementadas:**

- **DbContext**: MySQL con Pomelo.EntityFrameworkCore.MySql, retry on failure
- **Inyección de Dependencias**:
  - 4 Repositorios (Producto, Categoria, Orden, Usuario)
  - Unit of Work
  - 3 Services (ProductoService, UsuarioService, OrdenService)
- **AutoMapper**: Registrado con Assembly Scanning
- **JWT Authentication**:
  - Bearer Token
  - Validación de Issuer, Audience, Lifetime, Signing Key
  - ClockSkew = 0
- **CORS**: Política "AllowFrontend" para puertos 3000, 5173, 8080
- **JSON Options**: IgnoreCycles, WhenWritingNull
- **Swagger/OpenAPI**:
  - Documentación completa con metadata
  - Soporte JWT Bearer en UI
  - Disponible en raíz (RoutePrefix = string.Empty)
- **Middleware Pipeline**:
  - ExceptionHandler
  - CORS
  - Authentication
  - Authorization
- **Health Check**: Endpoint `/health`

### 2. appsettings.json - Configuración Actualizada

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=tiendamoderna;Uid=root;Pwd=rootpassword;"
  },
  "Jwt": {
    "Key": "TuClaveSecretaSuperSeguraDeAlMenos32CaracteresParaJWT2024",
    "Issuer": "TiendaModerna",
    "Audience": "TiendaModernaClients",
    "ExpirationInMinutes": 1440
  }
}
```

### 3. UsuariosController.cs (~200 líneas)

**Endpoints implementados:**

| Método | Ruta | Autenticación | Descripción |
|--------|------|---------------|-------------|
| POST | `/api/usuarios/registrar` | No | Registro de nuevo usuario |
| POST | `/api/usuarios/login` | No | Inicio de sesión con JWT |
| GET | `/api/usuarios/{id}` | Sí | Obtener usuario por ID |
| GET | `/api/usuarios/email-existe/{email}` | No | Verificar disponibilidad de email |
| POST | `/api/usuarios/recuperar-password` | No | Solicitar recuperación de contraseña |
| POST | `/api/usuarios/restablecer-password` | No | Restablecer contraseña con token |
| GET | `/api/usuarios/verificar-email/{token}` | No | Verificar email con token |

**Características:**
- Manejo de errores con try-catch y códigos HTTP apropiados
- Logging de eventos importantes
- Validación de ModelState
- Respuestas JSON consistentes
- DTO auxiliar: `RestablecerPasswordDto`

### 4. ProductosController.cs (~280 líneas)

**Endpoints implementados:**

| Método | Ruta | Autenticación | Roles | Descripción |
|--------|------|---------------|-------|-------------|
| GET | `/api/productos` | No | - | Listar productos paginados |
| GET | `/api/productos/{id}` | No | - | Obtener producto por ID |
| GET | `/api/productos/sku/{sku}` | No | - | Obtener producto por SKU |
| GET | `/api/productos/buscar` | No | - | Buscar productos por término |
| GET | `/api/productos/categoria/{categoriaId}` | No | - | Filtrar por categoría |
| GET | `/api/productos/destacados` | No | - | Productos destacados |
| GET | `/api/productos/ofertas` | No | - | Productos en oferta |
| POST | `/api/productos` | Sí | Admin | Crear producto |
| PUT | `/api/productos/{id}` | Sí | Admin | Actualizar producto |
| DELETE | `/api/productos/{id}` | Sí | Admin | Eliminar producto |
| PATCH | `/api/productos/{id}/estado` | Sí | Admin | Cambiar estado activo/inactivo |

**Características:**
- Paginación con `PagedResult<T>`
- Filtros y búsquedas
- Autorización por roles (Administrador, SuperAdministrador)
- Validación de SKU único
- Logging detallado

### 5. OrdenesController.cs (~260 líneas)

**Endpoints implementados:**

| Método | Ruta | Autenticación | Roles | Descripción |
|--------|------|---------------|-------|-------------|
| GET | `/api/ordenes/{id}` | Sí | - | Obtener orden por ID |
| GET | `/api/ordenes/numero/{numeroOrden}` | Sí | - | Obtener orden por número |
| GET | `/api/ordenes/mis-ordenes` | Sí | - | Órdenes del usuario |
| GET | `/api/ordenes/por-estado/{estado}` | Sí | Admin | Filtrar por estado |
| POST | `/api/ordenes` | Sí | - | Crear nueva orden |
| POST | `/api/ordenes/{id}/cancelar` | Sí | - | Cancelar orden |
| POST | `/api/ordenes/{id}/marcar-pagada` | Sí | Admin | Marcar como pagada |
| POST | `/api/ordenes/{id}/marcar-enviada` | Sí | Admin | Marcar como enviada |
| GET | `/api/ordenes/total-ventas` | Sí | Admin | Obtener total de ventas |

**Características:**
- Verificación de propiedad de orden (usuario solo ve sus órdenes)
- Autorización flexible (usuario vs administrador)
- Claims-based authorization con `ClaimTypes.NameIdentifier`
- Gestión de estados de orden
- Cálculo de ventas con filtros de fecha
- DTO auxiliar: `EnvioDto`

---

## Arquitectura de Seguridad

### JWT (JSON Web Tokens)

**Configuración:**
- **Algoritmo**: HMAC SHA256
- **Validaciones**: Issuer, Audience, Lifetime, Signing Key
- **Claims incluidos**:
  - `NameIdentifier`: ID del usuario
  - `Email`: Email del usuario
  - `Name`: Nombre completo
  - `Role`: Rol (Cliente, Administrador, SuperAdministrador)
- **Expiración**: 1440 minutos (24 horas)

**Uso en controllers:**
```csharp
[Authorize] // Requiere autenticación
[Authorize(Roles = "Administrador,SuperAdministrador")] // Requiere rol específico

// Obtener ID del usuario autenticado
var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
```

### CORS (Cross-Origin Resource Sharing)

**Política "AllowFrontend":**
- Orígenes permitidos: `localhost:3000`, `localhost:5173`, `localhost:8080`
- Headers: Todos permitidos
- Métodos: Todos permitidos
- Credentials: Permitidas

---

## Swagger/OpenAPI

**Características:**
- Documentación automática de todos los endpoints
- Interfaz UI interactiva en la raíz (`/`)
- Soporte JWT Bearer:
  - Botón "Authorize" en UI
  - Formato: `Bearer {token}`
- Metadata de API:
  - Título: "Tienda Moderna API"
  - Versión: "v1"
  - Descripción completa
  - Información de contacto

**Acceso:**
- Development: `https://localhost:5001` o `http://localhost:5000`
- Swagger UI: Disponible automáticamente en raíz

---

## Manejo de Errores

**Estrategia implementada:**

1. **Try-Catch en cada endpoint**
2. **Códigos HTTP apropiados**:
   - `200 OK`: Operación exitosa
   - `201 Created`: Recurso creado
   - `204 No Content`: Eliminación exitosa
   - `400 Bad Request`: Validación fallida
   - `401 Unauthorized`: No autenticado
   - `403 Forbidden`: No autorizado
   - `404 Not Found`: Recurso no encontrado
   - `500 Internal Server Error`: Error del servidor

3. **Respuestas JSON consistentes**:
```json
{
  "error": "Mensaje descriptivo del error"
}
```

4. **Logging estructurado**:
   - `LogInformation`: Operaciones exitosas
   - `LogWarning`: Validaciones fallidas
   - `LogError`: Excepciones inesperadas

---

## Validación

**Niveles de validación:**

1. **Data Annotations en DTOs**:
   - `[Required]`, `[EmailAddress]`, `[StringLength]`, `[Range]`
   - Validación automática con `ModelState.IsValid`

2. **Validación de lógica de negocio en Services**:
   - SKU único
   - Email único
   - Stock disponible
   - Propiedad de recursos

3. **Autorización por Claims**:
   - Usuario solo accede a sus propios recursos
   - Administradores tienen acceso completo

---

## Testing con Swagger

**Flujo recomendado:**

1. **Registrar usuario**:
   ```
   POST /api/usuarios/registrar
   {
     "email": "admin@test.com",
     "password": "Admin123",
     "nombreCompleto": "Administrador Test",
     "telefono": "1234567890"
   }
   ```

2. **Login y obtener token**:
   ```
   POST /api/usuarios/login
   {
     "email": "admin@test.com",
     "password": "Admin123"
   }
   ```
   Respuesta incluye: `{ "token": "eyJhbGc...", ... }`

3. **Autorizar en Swagger**:
   - Click en botón "Authorize"
   - Pegar: `Bearer eyJhbGc...`
   - Click "Authorize"

4. **Probar endpoints protegidos**:
   - Crear productos
   - Crear órdenes
   - Ver mis órdenes

---

## Estado de Compilación

```
✅ TiendaModerna.Domain
✅ TiendaModerna.Shared
✅ TiendaModerna.Application
✅ TiendaModerna.Infrastructure
✅ TiendaModerna.API

Compilación correcta.
    0 Advertencia(s)
    0 Errores
```

---

## Próximos Pasos

**BLOQUE 5 - Frontend Vue 3:**
1. Inicializar proyecto Vite + Vue 3 + TypeScript
2. Configurar Pinia para state management
3. Crear servicios de API (axios)
4. Implementar componentes de UI
5. Crear vistas principales (Home, Productos, Carrito, Login, Registro, Mis Órdenes)
6. Implementar autenticación en frontend
7. Integrar con backend API

---

## Comandos Útiles

```bash
# Compilar solución
dotnet build

# Ejecutar API
cd TiendaModerna.API
dotnet run

# Restaurar paquetes
dotnet restore

# Limpiar build
dotnet clean
```

---

## Notas Técnicas

- **Arquitectura**: Clean Architecture (4 capas)
- **Patrón**: Repository + Unit of Work + Services
- **ORM**: Entity Framework Core 8 con Pomelo MySQL
- **Autenticación**: JWT Bearer Tokens
- **Documentación**: OpenAPI/Swagger
- **Logging**: ILogger (Microsoft.Extensions.Logging)
- **Validación**: Data Annotations + FluentValidation (preparado)
- **CORS**: Configurado para desarrollo local

**Backend completo y funcional** ✅
