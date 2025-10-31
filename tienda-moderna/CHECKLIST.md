# ✅ Checklist de Implementación - Tienda Moderna

Este documento te ayudará a trackear el progreso de implementación del proyecto.

**Instrucciones**: Marca cada ítem con `[x]` a medida que lo completes.

---

## 📋 FASE 0: Preparación (15 min)

### Instalación de Herramientas
- [ ] .NET 8 SDK instalado (`dotnet --version` muestra 8.0.x)
- [ ] Visual Studio 2022 o VS Code instalado
- [ ] MySQL 8.0 instalado (o Docker configurado)
- [ ] Git instalado
- [ ] Postman o similar para probar APIs (opcional)

### Lectura de Documentación
- [ ] Leído: `docs/INDICE.md` (índice maestro)
- [ ] Leído: `docs/RESUMEN_EJECUTIVO.md` (visión completa)
- [ ] Leído: `README.md` (este archivo)
- [ ] Revisado: `INICIO_RAPIDO.md` (guía de 15 minutos)

---

## 🏗️ FASE 1: Estructura del Proyecto (15 min)

### Creación de Proyectos .NET
- [ ] Solución `TiendaModerna.sln` creada
- [ ] Proyecto `TiendaModerna.Domain` creado
- [ ] Proyecto `TiendaModerna.Application` creado
- [ ] Proyecto `TiendaModerna.Infrastructure` creado
- [ ] Proyecto `TiendaModerna.API` creado
- [ ] Proyecto `TiendaModerna.Shared` creado
- [ ] Todos los proyectos agregados a la solución
- [ ] Referencias entre proyectos establecidas correctamente
- [ ] `dotnet build` ejecuta sin errores

---

## 💎 FASE 2: Domain Layer (30 min)

### Entidades Básicas (desde `codigo-completo-domain-layer.md`)
- [ ] `Entities/Producto.cs` creado y copiado
- [ ] `Entities/Categoria.cs` creado y copiado
- [ ] `Entities/Variante.cs` creado y copiado
- [ ] `Entities/Imagen.cs` creado y copiado
- [ ] `Entities/Marca.cs` creado y copiado

### Entidades de Órdenes (desde `codigo-completo-domain-layer-parte2.md`)
- [ ] `Entities/Orden.cs` creado y copiado
- [ ] `Entities/DetalleOrden.cs` creado y copiado
- [ ] `Entities/Usuario.cs` creado y copiado

### Enumeraciones (desde `codigo-completo-domain-layer-parte2.md`)
- [ ] `Enums/EstadoOrden.cs` creado y copiado
- [ ] `Enums/RolUsuario.cs` creado y copiado
- [ ] `Enums/TipoDescuento.cs` creado y copiado

### Interfaces de Repositorios (desde `codigo-completo-domain-layer-parte3.md`)
- [ ] `Interfaces/IRepositorioGenerico.cs` creado y copiado
- [ ] `Interfaces/IRepositorioProducto.cs` creado y copiado
- [ ] `Interfaces/IRepositorioCategoria.cs` creado y copiado
- [ ] `Interfaces/IRepositorioOrden.cs` creado y copiado
- [ ] `Interfaces/IRepositorioUsuario.cs` creado y copiado
- [ ] `Interfaces/IUnitOfWork.cs` creado y copiado

### Verificación
- [ ] Todos los archivos compilan sin errores
- [ ] Namespaces correctos en todos los archivos
- [ ] `dotnet build` ejecuta correctamente

---

## 🔧 FASE 3: Infrastructure Layer (3-4 horas)

### Paquetes NuGet
- [ ] EntityFrameworkCore 8.0.0 instalado
- [ ] EntityFrameworkCore.Design 8.0.0 instalado
- [ ] Pomelo.EntityFrameworkCore.MySql 8.0.0 instalado
- [ ] EntityFrameworkCore.Tools 8.0.0 instalado
- [ ] BCrypt.Net-Next 4.0.3 instalado
- [ ] EPPlus 7.0.0 instalado

### DbContext
- [ ] `Data/TiendaContext.cs` creado
- [ ] DbSets de todas las entidades configurados
- [ ] `OnModelCreating` implementado

### Configuraciones de Entidades (Fluent API)
- [ ] `Data/Configurations/ProductoConfiguration.cs` creado
- [ ] `Data/Configurations/CategoriaConfiguration.cs` creado
- [ ] `Data/Configurations/VarianteConfiguration.cs` creado
- [ ] `Data/Configurations/ImagenConfiguration.cs` creado
- [ ] `Data/Configurations/MarcaConfiguration.cs` creado
- [ ] `Data/Configurations/OrdenConfiguration.cs` creado
- [ ] `Data/Configurations/DetalleOrdenConfiguration.cs` creado
- [ ] `Data/Configurations/UsuarioConfiguration.cs` creado

### Implementación de Repositorios
- [ ] `Repositories/RepositorioGenerico.cs` implementado
- [ ] `Repositories/RepositorioProducto.cs` implementado
- [ ] `Repositories/RepositorioCategoria.cs` implementado
- [ ] `Repositories/RepositorioOrden.cs` implementado
- [ ] `Repositories/RepositorioUsuario.cs` implementado
- [ ] `Repositories/UnitOfWork.cs` implementado

### Migraciones y Base de Datos
- [ ] String de conexión configurado en `appsettings.json`
- [ ] Primera migración creada (`dotnet ef migrations add Inicial`)
- [ ] Base de datos creada (`dotnet ef database update`)
- [ ] Tablas verificadas en MySQL

### Seed de Datos (Opcional pero Recomendado)
- [ ] `Data/TiendaContextSeed.cs` creado
- [ ] Categorías iniciales agregadas
- [ ] Marcas iniciales agregadas
- [ ] Usuario administrador creado
- [ ] Productos de prueba agregados (opcional)

---

## 📋 FASE 4: Application Layer (3-4 horas)

### Paquetes NuGet
- [ ] AutoMapper.Extensions.Microsoft.DependencyInjection 12.0.1 instalado
- [ ] FluentValidation.DependencyInjectionExtensions 11.9.0 instalado
- [ ] MediatR 12.2.0 instalado (opcional)

### DTOs - Productos
- [ ] `DTOs/Productos/ProductoDTO.cs` creado
- [ ] `DTOs/Productos/CrearProductoDTO.cs` creado
- [ ] `DTOs/Productos/ActualizarProductoDTO.cs` creado
- [ ] `DTOs/Productos/ProductoDetalleDTO.cs` creado

### DTOs - Categorías
- [ ] `DTOs/Categorias/CategoriaDTO.cs` creado
- [ ] `DTOs/Categorias/CrearCategoriaDTO.cs` creado
- [ ] `DTOs/Categorias/ActualizarCategoriaDTO.cs` creado

### DTOs - Órdenes
- [ ] `DTOs/Ordenes/OrdenDTO.cs` creado
- [ ] `DTOs/Ordenes/CrearOrdenDTO.cs` creado
- [ ] `DTOs/Ordenes/DetalleOrdenDTO.cs` creado

### DTOs - Usuarios
- [ ] `DTOs/Usuarios/UsuarioDTO.cs` creado
- [ ] `DTOs/Usuarios/RegistrarUsuarioDTO.cs` creado
- [ ] `DTOs/Usuarios/LoginDTO.cs` creado
- [ ] `DTOs/Usuarios/ActualizarUsuarioDTO.cs` creado

### AutoMapper Profiles
- [ ] `Mappings/MappingProfile.cs` creado
- [ ] Mapeos de Producto configurados
- [ ] Mapeos de Categoria configurados
- [ ] Mapeos de Orden configurados
- [ ] Mapeos de Usuario configurados

### Interfaces de Servicios
- [ ] `Services/Interfaces/IServicioProducto.cs` creado
- [ ] `Services/Interfaces/IServicioCategoria.cs` creado
- [ ] `Services/Interfaces/IServicioOrden.cs` creado
- [ ] `Services/Interfaces/IServicioUsuario.cs` creado
- [ ] `Services/Interfaces/IServicioAutenticacion.cs` creado
- [ ] `Services/Interfaces/IServicioImportacion.cs` creado

### Implementación de Servicios
- [ ] `Services/Implementations/ServicioProducto.cs` implementado
- [ ] `Services/Implementations/ServicioCategoria.cs` implementado
- [ ] `Services/Implementations/ServicioOrden.cs` implementado
- [ ] `Services/Implementations/ServicioUsuario.cs` implementado
- [ ] `Services/Implementations/ServicioAutenticacion.cs` implementado
- [ ] `Services/Implementations/ServicioImportacion.cs` implementado (Excel)

### Validadores FluentValidation
- [ ] `Validators/ProductoValidator.cs` creado
- [ ] `Validators/CategoriaValidator.cs` creado
- [ ] `Validators/OrdenValidator.cs` creado
- [ ] `Validators/UsuarioValidator.cs` creado

---

## 🎯 FASE 5: API Layer (3-4 horas)

### Paquetes NuGet
- [ ] Swashbuckle.AspNetCore 6.5.0 instalado
- [ ] Microsoft.AspNetCore.Authentication.JwtBearer 8.0.0 instalado
- [ ] Serilog.AspNetCore 8.0.0 instalado
- [ ] Serilog.Sinks.File 5.0.0 instalado
- [ ] Serilog.Sinks.Console 5.0.0 instalado

### Configuración de Program.cs
- [ ] Dependency Injection configurada
- [ ] DbContext registrado
- [ ] Repositorios registrados
- [ ] Servicios registrados
- [ ] AutoMapper configurado
- [ ] FluentValidation configurado
- [ ] CORS configurado
- [ ] Swagger configurado
- [ ] JWT Authentication configurado
- [ ] Serilog configurado

### Middlewares
- [ ] `Middlewares/ExceptionHandlingMiddleware.cs` creado
- [ ] `Middlewares/RequestLoggingMiddleware.cs` creado
- [ ] Middlewares registrados en Program.cs

### Controladores
- [ ] `Controllers/ProductosController.cs` creado
  - [ ] GET api/productos (listar con paginación)
  - [ ] GET api/productos/{id} (obtener por ID)
  - [ ] GET api/productos/sku/{sku} (obtener por SKU)
  - [ ] GET api/productos/categoria/{id} (filtrar por categoría)
  - [ ] POST api/productos (crear)
  - [ ] PUT api/productos/{id} (actualizar)
  - [ ] DELETE api/productos/{id} (eliminar)
  - [ ] GET api/productos/destacados (productos destacados)
  - [ ] GET api/productos/ofertas (productos en oferta)

- [ ] `Controllers/CategoriasController.cs` creado
  - [ ] GET api/categorias (listar todas)
  - [ ] GET api/categorias/{id} (obtener por ID)
  - [ ] GET api/categorias/slug/{slug} (obtener por slug)
  - [ ] GET api/categorias/raiz (categorías raíz)
  - [ ] POST api/categorias (crear)
  - [ ] PUT api/categorias/{id} (actualizar)
  - [ ] DELETE api/categorias/{id} (eliminar)

- [ ] `Controllers/OrdenesController.cs` creado
  - [ ] GET api/ordenes (listar con paginación) [Admin]
  - [ ] GET api/ordenes/{id} (obtener por ID)
  - [ ] GET api/ordenes/usuario (mis órdenes) [Autenticado]
  - [ ] POST api/ordenes (crear orden)
  - [ ] PUT api/ordenes/{id}/estado (cambiar estado) [Admin]
  - [ ] GET api/ordenes/estadisticas (dashboard) [Admin]

- [ ] `Controllers/AutenticacionController.cs` creado
  - [ ] POST api/auth/registrar (registrar usuario)
  - [ ] POST api/auth/login (iniciar sesión)
  - [ ] POST api/auth/recuperar-password (solicitar recuperación)
  - [ ] POST api/auth/restablecer-password (restablecer con token)
  - [ ] GET api/auth/verificar-email (verificar email con token)

- [ ] `Controllers/UsuariosController.cs` creado
  - [ ] GET api/usuarios/perfil (mi perfil) [Autenticado]
  - [ ] PUT api/usuarios/perfil (actualizar perfil) [Autenticado]
  - [ ] GET api/usuarios (listar usuarios) [Admin]
  - [ ] PUT api/usuarios/{id}/rol (cambiar rol) [Admin]

- [ ] `Controllers/ImportacionController.cs` creado
  - [ ] POST api/importacion/excel (subir Excel) [Admin]
  - [ ] GET api/importacion/plantilla (descargar plantilla) [Admin]

### Configuración
- [ ] `appsettings.json` configurado (strings de conexión, JWT, etc.)
- [ ] `appsettings.Development.json` configurado
- [ ] Variables de entorno documentadas

### Testing de API
- [ ] Swagger UI accesible en /swagger
- [ ] Endpoints de productos probados
- [ ] Endpoints de categorías probados
- [ ] Endpoints de autenticación probados
- [ ] JWT funcionando correctamente
- [ ] Endpoints de órdenes probados
- [ ] Importación de Excel probada

---

## 🎨 FASE 6: Frontend Vue 3 (5-8 horas)

### Setup Inicial
- [ ] Proyecto Vue 3 creado con Vite
- [ ] Tailwind CSS configurado
- [ ] Vue Router instalado y configurado
- [ ] Pinia (state management) instalado
- [ ] Axios configurado para llamadas API

### Estructura de Carpetas
- [ ] `src/components/` creada
- [ ] `src/views/` creada
- [ ] `src/stores/` creada
- [ ] `src/services/` creada
- [ ] `src/composables/` creada
- [ ] `src/utils/` creada

### Stores (Pinia)
- [ ] `stores/auth.js` creado (autenticación)
- [ ] `stores/products.js` creado (productos)
- [ ] `stores/categories.js` creado (categorías)
- [ ] `stores/cart.js` creado (carrito)
- [ ] `stores/orders.js` creado (órdenes)

### Servicios API
- [ ] `services/api.js` creado (configuración axios)
- [ ] `services/productsService.js` creado
- [ ] `services/categoriesService.js` creado
- [ ] `services/authService.js` creado
- [ ] `services/ordersService.js` creado

### Componentes Globales
- [ ] `components/Navbar.vue` creado
- [ ] `components/Footer.vue` creado
- [ ] `components/ProductCard.vue` creado
- [ ] `components/CategoryCard.vue` creado
- [ ] `components/LoadingSpinner.vue` creado
- [ ] `components/Modal.vue` creado
- [ ] `components/Pagination.vue` creado

### Vistas de Cliente
- [ ] `views/Home.vue` creada (página principal)
- [ ] `views/Products.vue` creada (catálogo)
- [ ] `views/ProductDetail.vue` creada (detalle de producto)
- [ ] `views/Cart.vue` creada (carrito)
- [ ] `views/Checkout.vue` creada (checkout)
- [ ] `views/Login.vue` creada (inicio de sesión)
- [ ] `views/Register.vue` creada (registro)
- [ ] `views/Profile.vue` creada (perfil de usuario)
- [ ] `views/Orders.vue` creada (mis órdenes)
- [ ] `views/OrderDetail.vue` creada (detalle de orden)

### Vistas de Admin
- [ ] `views/admin/Dashboard.vue` creada
- [ ] `views/admin/ProductsList.vue` creada
- [ ] `views/admin/ProductForm.vue` creada
- [ ] `views/admin/CategoriesList.vue` creada
- [ ] `views/admin/OrdersList.vue` creada
- [ ] `views/admin/ImportExcel.vue` creada

### Funcionalidades
- [ ] Navegación funcional
- [ ] Catálogo con filtros y paginación
- [ ] Detalle de producto con variantes
- [ ] Agregar/quitar productos del carrito
- [ ] Registro de usuarios
- [ ] Login/logout
- [ ] Proceso de checkout
- [ ] Historial de órdenes
- [ ] Panel de administración

---

## 🐳 FASE 7: Docker y DevOps (2-3 horas)

### Dockerfiles
- [ ] `backend/Dockerfile` creado
- [ ] `frontend/Dockerfile` creado
- [ ] Dockerfiles optimizados (multi-stage builds)

### Docker Compose
- [ ] `docker-compose.yml` verificado
- [ ] Servicio MySQL configurado
- [ ] Servicio backend configurado
- [ ] Servicio frontend configurado
- [ ] Volúmenes persistentes configurados
- [ ] Networks configuradas
- [ ] Health checks configurados

### Testing de Contenedores
- [ ] `docker-compose up` funciona correctamente
- [ ] Backend accesible en http://localhost:5000
- [ ] Frontend accesible en http://localhost:3000
- [ ] MySQL accesible en localhost:3306
- [ ] Datos persisten después de reiniciar contenedores

### CI/CD (Opcional)
- [ ] `.gitlab-ci.yml` creado
- [ ] Pipeline de build configurado
- [ ] Pipeline de tests configurado
- [ ] Pipeline de deploy configurado

---

## 🧪 FASE 8: Testing (3-4 horas)

### Unit Tests - Domain Layer
- [ ] Tests para métodos de Producto
- [ ] Tests para métodos de Orden
- [ ] Tests para métodos de Usuario
- [ ] Tests para validaciones de negocio

### Unit Tests - Application Layer
- [ ] Tests para ServicioProducto
- [ ] Tests para ServicioCategoria
- [ ] Tests para ServicioOrden
- [ ] Tests para ServicioAutenticacion
- [ ] Mocks de repositorios configurados

### Integration Tests
- [ ] Tests de API con base de datos en memoria
- [ ] Tests de endpoints de productos
- [ ] Tests de endpoints de autenticación
- [ ] Tests de endpoints de órdenes

### E2E Tests (Opcional)
- [ ] Playwright o Cypress configurado
- [ ] Test de flujo completo de compra
- [ ] Test de registro y login
- [ ] Test de panel de administración

---

## 📊 FASE 9: Características Avanzadas (5-8 horas)

### Sistema de Descuentos
- [ ] Modelo de Cupon creado
- [ ] Servicio de aplicación de cupones
- [ ] Endpoint para validar cupones
- [ ] Interfaz de aplicar cupón en checkout

### Manejo de Imágenes
- [ ] Servicio de subida de imágenes
- [ ] Integración con almacenamiento (local/cloud)
- [ ] Redimensionamiento automático
- [ ] Galería de imágenes en admin

### Importación de Excel Completa
- [ ] Plantilla Excel con formato definido
- [ ] Validación de datos del Excel
- [ ] Creación masiva de productos
- [ ] Actualización masiva de productos
- [ ] Reporte de errores en importación

### Sistema de Notificaciones
- [ ] Configuración de SMTP
- [ ] Email de confirmación de registro
- [ ] Email de confirmación de orden
- [ ] Email de cambio de estado de orden
- [ ] Email de recuperación de contraseña

### Dashboard de Administración
- [ ] Gráficos de ventas
- [ ] Productos más vendidos
- [ ] Estadísticas de órdenes por estado
- [ ] Alertas de stock bajo
- [ ] Reporte de ingresos

---

## 📈 FASE 10: Optimización y Deploy (3-5 horas)

### Optimización de Backend
- [ ] Paginación en todos los listados
- [ ] Eager/lazy loading optimizado
- [ ] Índices de base de datos creados
- [ ] Caché implementado (Redis opcional)
- [ ] Rate limiting configurado

### Optimización de Frontend
- [ ] Lazy loading de componentes
- [ ] Imágenes optimizadas
- [ ] Build de producción optimizado
- [ ] PWA configurado (opcional)

### Seguridad
- [ ] Validación de entrada en todos los endpoints
- [ ] Protección contra SQL injection
- [ ] Protección contra XSS
- [ ] HTTPS configurado
- [ ] Secrets en variables de entorno

### Documentación
- [ ] README.md completo
- [ ] API documentada en Swagger
- [ ] Guía de instalación
- [ ] Guía de contribución
- [ ] Changelog

### Deploy
- [ ] Entorno de producción configurado
- [ ] Base de datos de producción
- [ ] Variables de entorno de producción
- [ ] Dominio configurado
- [ ] SSL/TLS configurado
- [ ] Backup automático configurado

---

## ✅ Verificación Final

### Funcional
- [ ] Usuarios pueden registrarse y hacer login
- [ ] Usuarios pueden ver catálogo de productos
- [ ] Usuarios pueden filtrar productos
- [ ] Usuarios pueden agregar productos al carrito
- [ ] Usuarios pueden realizar compras
- [ ] Usuarios pueden ver su historial de órdenes
- [ ] Admins pueden crear/editar/eliminar productos
- [ ] Admins pueden importar productos desde Excel
- [ ] Admins pueden gestionar órdenes
- [ ] Admins pueden ver estadísticas

### Técnico
- [ ] Código sigue principios SOLID
- [ ] Arquitectura Clean Architecture implementada
- [ ] Unit tests con >70% coverage
- [ ] Sin warnings en compilación
- [ ] Sin errores de linting
- [ ] Documentación completa

### Performance
- [ ] Tiempos de respuesta < 200ms
- [ ] Imágenes optimizadas
- [ ] Frontend carga en < 3 segundos
- [ ] Base de datos con índices adecuados

---

## 🎉 ¡PROYECTO COMPLETO!

Cuando hayas marcado todos los ítems, ¡habrás completado el proyecto Tienda Moderna!

### 📊 Progreso Total

Puedes calcular tu progreso contando los `[x]` vs `[ ]`:

```bash
# En Linux/Mac/Git Bash:
grep -o "\[x\]" CHECKLIST.md | wc -l  # Completados
grep -o "\[ \]" CHECKLIST.md | wc -l  # Pendientes
```

---

## 📝 Notas

Usa esta sección para anotar problemas encontrados, decisiones tomadas, o cambios realizados:

```
[Fecha] - [Tu Nota]
-------------------

```

---

**Fecha de Inicio**: _______________  
**Fecha de Finalización**: _______________  
**Horas Totales Invertidas**: _______________

¡Mucho éxito con tu implementación! 🚀
