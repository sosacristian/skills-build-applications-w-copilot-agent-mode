# Tienda Moderna - Documentación Completa

## 📋 Resumen Ejecutivo

**Tienda Moderna** es una aplicación de e-commerce completa desarrollada con **.NET 8** (backend) y **Vue 3 + TypeScript** (frontend). La aplicación incluye funcionalidades completas de gestión de productos, carrito de compras, autenticación JWT, y un panel de administración con importación masiva de productos desde Excel.

---

## 🏗️ Arquitectura del Sistema

### Stack Tecnológico

#### Backend (.NET 8 API)
- **Framework**: ASP.NET Core 8.0
- **Base de Datos**: MySQL 8.0 (Docker)
- **ORM**: Entity Framework Core 9.0.0
- **Autenticación**: JWT (JSON Web Tokens)
- **Librerías Adicionales**:
  - EPPlus 7.0.0 (para operaciones con Excel)
  - AutoMapper 13.0.1 (mapeo de objetos)
  - Swashbuckle (documentación API)

#### Frontend (Vue 3 + Vite)
- **Framework**: Vue 3 con Composition API
- **Lenguaje**: TypeScript
- **Build Tool**: Vite 6.0.3
- **Router**: Vue Router 4.4.5
- **Estado**: Pinia 2.3.0
- **HTTP Client**: Axios 1.7.9
- **Estilos**: Tailwind CSS 3.4.17
- **JWT**: jwt-decode 4.0.0

### Estructura del Proyecto

```
tienda-moderna/
├── backend/
│   ├── TiendaModerna.API/          # Capa de presentación (Controllers, Program.cs)
│   ├── TiendaModerna.Application/  # Lógica de negocio (DTOs, Services, Interfaces)
│   ├── TiendaModerna.Domain/       # Entidades del dominio
│   ├── TiendaModerna.Infrastructure/ # Acceso a datos (EF Core, Repositories)
│   └── TiendaModerna.Shared/       # Utilidades compartidas
├── frontend-app/
│   ├── src/
│   │   ├── components/             # Componentes reutilizables
│   │   ├── views/                  # Vistas de páginas
│   │   │   ├── admin/              # Panel de administración
│   │   │   ├── Home.vue
│   │   │   ├── Login.vue
│   │   │   └── Register.vue
│   │   ├── router/                 # Configuración de rutas
│   │   ├── stores/                 # Estado global (Pinia)
│   │   ├── services/               # Servicios API
│   │   └── types/                  # Definiciones TypeScript
│   └── public/
└── docker-compose.yml              # Orquestación de servicios
```

---

## 🗄️ Base de Datos

### Modelo de Datos

La base de datos `tienda_moderna` contiene **9 tablas** con las siguientes relaciones:

#### Diagrama de Entidades

```
Usuarios (1) ─────< (N) Ordenes (1) ─────< (N) ItemsOrden
                                              │
                                              └──────> (N) Productos
                                                         │
Categorias (1) ───< (N) ────────────────────────────────┤
                                                         │
Marcas (1) ───< (N) ─────────────────────────────────────┘
                                                         │
                  Variantes (N) ────< (1) ───────────────┤
                                                         │
                  Imagenes (N) ────< (1) ─────────────────┘
```

#### Tablas Principales

**1. Usuarios**
```sql
- Id (PK)
- Email (único, índice)
- PasswordHash
- NombreCompleto
- Telefono
- Rol (enum: Cliente, Administrador)
- EstaActivo
- FechaCreacion
- UltimoInicioSesion
- Direcciones (predeterminadas)
```

**2. Productos**
```sql
- Id (PK)
- CodigoSKU (único, índice)
- Nombre (índice)
- Descripcion
- PrecioBase
- PorcentajeDescuento
- PrecioFinal (calculado)
- CantidadStock
- EstaActivo
- EsDestacado
- CategoriaId (FK)
- MarcaId (FK, nullable)
- FechaCreacion
- FechaActualizacion
```

**3. Categorias**
```sql
- Id (PK)
- Nombre (único)
- Descripcion
- Icono
- Orden
- EstaActiva
```

**4. Marcas**
```sql
- Id (PK)
- Nombre (único)
- Descripcion
- LogoUrl
- SitioWeb
- EstaActiva
```

**5. Ordenes**
```sql
- Id (PK)
- NumeroOrden (único, generado)
- UsuarioId (FK)
- FechaOrden
- EstadoOrden (enum: Pendiente, Confirmada, Enviada, etc.)
- TotalProductos
- TotalDescuentos
- TotalFinal
- DireccionEnvio
- MetodoPago
- NotasOrden
```

**6. ItemsOrden**
```sql
- Id (PK)
- OrdenId (FK)
- ProductoId (FK)
- VarianteId (FK, nullable)
- Cantidad
- PrecioUnitario
- PrecioTotal
- DescuentoAplicado
```

**7. Variantes**
```sql
- Id (PK)
- ProductoId (FK)
- CodigoSKU (único)
- Talla
- Color
- Material
- AjustePrecio
- CantidadStock
```

**8. Imagenes**
```sql
- Id (PK)
- ProductoId (FK)
- Url
- TextoAlternativo
- Orden
- EsPrincipal
- FechaSubida
```

**9. ValoracionesProducto**
```sql
- Id (PK)
- ProductoId (FK)
- UsuarioId (FK)
- Calificacion (1-5)
- Comentario
- FechaValoracion
```

### Datos de Seed

La aplicación incluye datos iniciales:
- **5 categorías**: Remeras, Buzos, Pantalones, Zapatillas, Accesorios
- **5 marcas**: Nike, Adidas, Puma, Reebok, Under Armour
- **10 productos** de ejemplo con precios, descripciones y stock

---

## 🔐 Autenticación y Seguridad

### Sistema JWT

#### Configuración Backend
```csharp
// appsettings.json
"Jwt": {
  "Key": "TuClaveSecretaMuySeguraDeAlMenos32CaracteresParaJWT",
  "Issuer": "TiendaModernaAPI",
  "Audience": "TiendaModernaClientes",
  "ExpiracionMinutos": 60
}
```

#### Flujo de Autenticación
1. Usuario se registra o inicia sesión
2. Backend valida credenciales
3. Genera token JWT con claims:
   - `nameid`: ID del usuario
   - `unique_name`: Nombre completo
   - `email`: Email
   - `role`: Rol del usuario
4. Token se envía al frontend
5. Frontend almacena token en localStorage
6. Cada petición incluye header: `Authorization: Bearer {token}`

#### Roles y Permisos

**Cliente** (por defecto):
- Ver productos y categorías
- Agregar al carrito
- Realizar compras
- Ver su perfil y órdenes

**Administrador**:
- Todo lo anterior +
- Acceso al Panel Admin
- Gestión de productos
- Importar/exportar productos Excel
- Ver todas las órdenes
- Gestionar usuarios

### Lógica de Primer Usuario

El primer usuario registrado obtiene automáticamente el rol de **Administrador**:

```csharp
// UsuarioService.cs
var totalUsuarios = await _unitOfWork.Usuarios.ContarAsync();
usuario.Rol = totalUsuarios == 0 ? RolUsuario.Administrador : RolUsuario.Cliente;
```

---

## 📦 Funcionalidades Implementadas

### Backend API Endpoints

#### Autenticación
- `POST /api/usuarios/registrar` - Registro de nuevos usuarios
- `POST /api/usuarios/login` - Inicio de sesión (retorna JWT)

#### Productos
- `GET /api/productos?pagina=1&tamanoPagina=20` - Listar productos (paginado)
- `GET /api/productos/{id}` - Detalle de producto
- `GET /api/productos/destacados` - Productos destacados
- `GET /api/productos/buscar?termino=X` - Búsqueda de productos
- `POST /api/productos` - Crear producto (Admin)
- `PUT /api/productos/{id}` - Actualizar producto (Admin)
- `DELETE /api/productos/{id}` - Eliminar producto (Admin)

#### Categorías
- `GET /api/categorias` - Listar todas las categorías
- `GET /api/categorias/{id}` - Detalle de categoría
- `GET /api/categorias/{id}/productos` - Productos de una categoría

#### Marcas
- `GET /api/marcas` - Listar todas las marcas
- `GET /api/marcas/{id}` - Detalle de marca

#### Admin (Panel de Administración)
- `POST /api/admin/productos/importar` - Importar productos desde Excel
- `GET /api/admin/productos/plantilla` - Descargar plantilla Excel

### Frontend (Vue 3)

#### Vistas Públicas
1. **Home** (`/`)
   - Hero section con call-to-action
   - Productos destacados
   - Categorías principales
   - Banners promocionales

2. **Productos** (`/productos`)
   - Listado completo de productos
   - Filtros por categoría y marca
   - Ordenamiento (precio, nombre)
   - Paginación

3. **Detalle de Producto** (`/producto/:id`)
   - Información completa
   - Galería de imágenes
   - Selección de variantes
   - Agregar al carrito

4. **Registro** (`/register`)
   - Formulario de registro
   - Validaciones en tiempo real
   - Auto-login después del registro

5. **Login** (`/login`)
   - Formulario de inicio de sesión
   - Persistencia de sesión
   - Redirección a página anterior

6. **Carrito** (`/carrito`)
   - Resumen de productos
   - Ajustar cantidades
   - Eliminar items
   - Calcular totales

#### Panel de Administración (Solo Administradores)

1. **Dashboard** (`/admin`)
   - Estadísticas generales:
     - Total de productos
     - Total de categorías
     - Total de marcas
     - Órdenes pendientes
   - Accesos rápidos a secciones

2. **Gestión de Productos** (`/admin/products`)
   - Tabla de productos con búsqueda
   - Editar/Eliminar productos
   - **Importación masiva desde Excel**
   - **Descarga de plantilla Excel**

#### Componentes Reutilizables
- `Navbar.vue` - Barra de navegación con menú usuario/admin
- `Alert.vue` - Mensajes de éxito/error
- `ProductCard.vue` - Tarjeta de producto
- `Loading.vue` - Indicador de carga

---

## 📊 Importación de Productos desde Excel

### Funcionalidad Completa

#### Backend: AdminController

**Endpoint de Importación**
```csharp
[HttpPost("productos/importar")]
[Authorize(Roles = "Administrador")]
public async Task<ActionResult<ResultadoImportacionDto>> ImportarProductos(IFormFile archivo)
```

**Formato del Archivo Excel**

| CodigoSKU | Nombre | Descripcion | PrecioBase | PorcentajeDescuento | Stock | Categoria | Marca | Destacado |
|-----------|--------|-------------|------------|---------------------|-------|-----------|-------|-----------|
| PROD-001 | Remera Classic | Remera de algodón | 10000 | 10 | 50 | Remeras | Nike | Si |

**Proceso de Importación**:
1. Valida el archivo (.xlsx)
2. Lee el Excel con EPPlus
3. Para cada fila:
   - Valida campos obligatorios (SKU, Nombre, Precio, Stock)
   - Busca o crea Categoría automáticamente
   - Busca o crea Marca automáticamente
   - Actualiza producto existente (por SKU) o crea uno nuevo
   - Calcula precio final con descuento
4. Retorna resultado detallado:
   - Total procesados
   - Exitosos
   - Fallidos (con detalles por fila)

**Endpoint de Plantilla**
```csharp
[HttpGet("productos/plantilla")]
[Authorize(Roles = "Administrador")]
public ActionResult DescargarPlantilla()
```

Genera un archivo Excel con:
- Encabezados formateados
- Fila de ejemplo
- Listo para editar y cargar

#### Frontend: Products.vue

**Modal de Importación**
```vue
<template>
  <div class="modal">
    <!-- Selector de archivo -->
    <input type="file" accept=".xlsx" @change="seleccionarArchivo" />
    
    <!-- Información del archivo -->
    <div v-if="archivoSeleccionado">
      Archivo: {{ archivoSeleccionado.name }}
      Tamaño: {{ (archivoSeleccionado.size / 1024).toFixed(2) }} KB
    </div>
    
    <!-- Resultado de importación -->
    <div v-if="resultadoImportacion">
      <div>Total: {{ resultadoImportacion.totalProcesados }}</div>
      <div>Exitosos: {{ resultadoImportacion.exitosos }}</div>
      <div>Fallidos: {{ resultadoImportacion.fallidos }}</div>
      
      <!-- Tabla de errores -->
      <table v-if="resultadoImportacion.errores.length > 0">
        <tr v-for="error in resultadoImportacion.errores">
          <td>Fila {{ error.fila }}</td>
          <td>{{ error.campo }}</td>
          <td>{{ error.mensaje }}</td>
        </tr>
      </table>
    </div>
    
    <button @click="importarArchivo">Importar</button>
  </div>
</template>
```

**Función de Importación**
```typescript
const importarArchivo = async () => {
  const formData = new FormData()
  formData.append('archivo', archivoSeleccionado.value)
  
  const response = await api.post('/admin/productos/importar', formData, {
    headers: { 'Content-Type': 'multipart/form-data' }
  })
  
  resultadoImportacion.value = response.data
}
```

**Descarga de Plantilla**
```typescript
const descargarPlantilla = async () => {
  const response = await api.get('/admin/productos/plantilla', {
    responseType: 'blob'
  })
  
  const url = window.URL.createObjectURL(new Blob([response.data]))
  const link = document.createElement('a')
  link.href = url
  link.setAttribute('download', 'plantilla_productos.xlsx')
  document.body.appendChild(link)
  link.click()
  link.remove()
}
```

---

## 🚀 Despliegue y Ejecución

### Requisitos Previos

- **Node.js**: v20+ (para frontend)
- **.NET SDK**: 8.0+
- **Docker**: 20.10+ (para MySQL)
- **Git**: Para clonar el repositorio

### Instalación Paso a Paso

#### 1. Clonar el Repositorio
```bash
git clone <repository-url>
cd tienda-moderna
```

#### 2. Configurar la Base de Datos (MySQL con Docker)
```bash
docker-compose up -d mysql
```

Espera unos segundos hasta que MySQL esté listo (healthcheck).

#### 3. Backend (.NET)

**Restaurar paquetes**
```bash
cd backend/TiendaModerna.API
dotnet restore
```

**Aplicar migraciones**
```bash
dotnet ef database update
```

**Iniciar el servidor**
```bash
dotnet run
```

El backend estará disponible en: `http://localhost:5000`

#### 4. Frontend (Vue 3)

**Instalar dependencias**
```bash
cd frontend-app
npm install
```

**Iniciar servidor de desarrollo**
```bash
npm run dev
```

El frontend estará disponible en: `http://localhost:5173`

### Docker Compose Completo

Para iniciar todos los servicios simultáneamente:

```bash
docker-compose up --build
```

Servicios disponibles:
- MySQL: `localhost:3306`
- Backend API: `http://localhost:5000`
- Frontend: `http://localhost:5173`

---

## 🧪 Pruebas y Validación

### Casos de Prueba Realizados

#### ✅ Autenticación
1. **Registro de usuario**
   - Validación de campos obligatorios
   - Contraseña mínima 6 caracteres
   - Email único
   - Nombre completo mínimo 3 caracteres
   
2. **Inicio de sesión**
   - Credenciales correctas generan JWT
   - Token almacenado en localStorage
   - Redirección a home

3. **Primer usuario como Admin**
   - Base de datos vacía
   - Primer registro obtiene rol Administrador
   - Menú "Panel Admin" visible

#### ✅ Productos
1. **Listado y paginación**
   - 20 productos por página
   - Navegación entre páginas
   - Total de productos correcto

2. **Búsqueda y filtros**
   - Búsqueda por nombre
   - Filtro por categoría
   - Filtro por marca
   - Productos destacados

#### ✅ Panel de Administración
1. **Dashboard**
   - Estadísticas actualizadas
   - Cards con contadores
   - Enlaces funcionales

2. **Importación Excel**
   - Descarga de plantilla correcta
   - Validación de formato .xlsx
   - Importación exitosa de productos válidos
   - Reporte de errores detallado
   - Auto-creación de categorías/marcas
   - Actualización de productos existentes (por SKU)

#### ✅ Navegación y UX
1. **Routing**
   - Rutas públicas accesibles
   - Rutas protegidas requieren login
   - Rutas admin solo para administradores
   - Redirección correcta

2. **Navbar**
   - Menú adaptativo por rol
   - Contador de carrito funcional
   - Dropdown de usuario
   - Cerrar sesión

---

## 🔧 Configuración Avanzada

### Variables de Entorno

#### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=tienda_moderna;User=tienda_user;Password=tienda_pass_2024;"
  },
  "Jwt": {
    "Key": "TuClaveSecretaMuySeguraDeAlMenos32CaracteresParaJWT",
    "Issuer": "TiendaModernaAPI",
    "Audience": "TiendaModernaClientes",
    "ExpiracionMinutos": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

#### Frontend (.env)
```env
VITE_API_URL=http://localhost:5000/api
```

### CORS Configuration

Backend permite solicitudes desde:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

---

## 📝 Archivos Importantes Modificados

### Backend

1. **TiendaModerna.API/Controllers/AdminController.cs** (NUEVO - 340 líneas)
   - Manejo de importación Excel
   - Generación de plantilla
   - Auto-creación de entidades relacionadas

2. **TiendaModerna.Application/DTOs/Producto/ImportarProductoDto.cs** (NUEVO)
   - `ResultadoImportacionDto`
   - `ErrorImportacionDto`

3. **TiendaModerna.Application/Services/UsuarioService.cs** (MODIFICADO)
   - Lógica de primer usuario como Administrador

4. **TiendaModerna.API.csproj** (MODIFICADO)
   - Añadido EPPlus 7.0.0

### Frontend

1. **src/views/admin/Dashboard.vue** (NUEVO - 100 líneas)
   - Vista del panel de administración
   - Tarjetas de estadísticas
   - Navegación rápida

2. **src/views/admin/Products.vue** (NUEVO - 326 líneas)
   - Gestión completa de productos
   - Modal de importación Excel
   - Tabla de productos con búsqueda

3. **src/components/Navbar.vue** (MODIFICADO)
   - Menú "Panel Admin" condicional
   - Corrección: `nombreCompleto` en lugar de `nombre`

4. **src/views/Register.vue** (MODIFICADO)
   - Campo `nombreCompleto` alineado con backend
   - Validación mínima de 3 caracteres

5. **src/router/index.ts** (MODIFICADO)
   - Rutas `/admin` y `/admin/products`
   - Guard `requiresAdmin`

---

## 🐛 Problemas Resueltos

### 1. Error de Registro (nombreCompleto vs nombre)
**Problema**: Frontend enviaba `nombre`, backend esperaba `nombreCompleto`

**Solución**: Actualizado Register.vue
```vue
<!-- Antes -->
<input v-model="form.nombre" />

<!-- Después -->
<input v-model="form.nombreCompleto" minlength="3" />
```

### 2. LINQ Expression con Null-Conditional
**Problema**: `nombreCategoria?.ToLower()` en expresión LINQ

**Error**: CS8072 - operador de propagación NULL no permitido

**Solución**:
```csharp
// Antes
var categoria = _context.Categorias
    .FirstOrDefault(c => c.Nombre.ToLower() == nombreCategoria?.ToLower());

// Después
var nombreCategoriaLower = nombreCategoria.ToLower();
var categoria = _context.Categorias
    .FirstOrDefault(c => c.Nombre.ToLower() == nombreCategoriaLower);
```

### 3. Autorización en AdminController
**Problema**: `[Authorize(Roles = "Admin")]` no coincidía con rol "Administrador"

**Solución**:
```csharp
// Cambiar
[Authorize(Roles = "Admin")]

// Por
[Authorize(Roles = "Administrador")]
```

### 4. Token JWT Expirado
**Problema**: Token JWT no se actualizaba después de cambiar rol en base de datos

**Solución**: Cerrar sesión y volver a iniciar sesión para generar nuevo token con claims actualizados

### 5. Navbar Usuario Undefined
**Problema**: `authStore.usuario?.nombre` retornaba undefined

**Solución**: Cambiar a `authStore.usuario?.nombreCompleto` (propiedad correcta del DTO)

---

## 📚 Recursos y Documentación

### Endpoints de la API

Documentación Swagger disponible en:
- `http://localhost:5000/swagger`

### Referencias Técnicas

**Backend**:
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [EPPlus Documentation](https://github.com/EPPlusSoftware/EPPlus)

**Frontend**:
- [Vue 3 Documentation](https://vuejs.org/)
- [Vue Router](https://router.vuejs.org/)
- [Pinia](https://pinia.vuejs.org/)
- [Tailwind CSS](https://tailwindcss.com/)

---

## 👥 Equipo y Créditos

**Desarrollado por**: Cristian Sosa

**Tecnologías utilizadas**:
- .NET 8
- Vue 3 + TypeScript
- MySQL 8.0
- Docker
- EPPlus

**Fecha de finalización**: 31 de Octubre, 2025

---

## 📄 Licencia

Este proyecto es de código abierto para fines educativos.

---

## 🎯 Próximos Pasos Sugeridos

1. **Funcionalidades Adicionales**:
   - Implementar proceso completo de checkout
   - Agregar pasarela de pagos
   - Sistema de envíos
   - Notificaciones por email
   - Wishlist de productos

2. **Panel de Administración**:
   - Gestión de categorías
   - Gestión de marcas
   - Ver y gestionar órdenes
   - Reportes y estadísticas avanzadas
   - Gestión de usuarios

3. **Mejoras de UX**:
   - Añadir loading states
   - Mejores animaciones
   - Modo oscuro
   - PWA (Progressive Web App)

4. **Seguridad**:
   - Refresh tokens
   - Rate limiting
   - CSRF protection mejorada
   - Validación de imágenes

5. **Performance**:
   - Caché de productos
   - Lazy loading de imágenes
   - Code splitting
   - CDN para assets

---

## 📞 Soporte

Para reportar problemas o sugerencias:
- Email: cristianraulsosa@gmail.com
- GitHub Issues: [Crear issue]

---

**Última actualización**: 31 de Octubre, 2025
**Versión**: 1.0.0
