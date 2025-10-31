# 🎯 Domain Layer - Parte 3 (Interfaces de Repositorios)

## 📁 Interfaces/IRepositorioGenerico.cs

```csharp
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz genérica que define las operaciones básicas de un repositorio.
    /// 
    /// ¿POR QUÉ UN REPOSITORIO GENÉRICO?
    /// - DRY: No repetir código común en cada repositorio
    /// - Abstracción: La capa de dominio no conoce EF Core
    /// - Testeable: Se puede crear un mock fácilmente
    /// - Intercambiable: Se puede cambiar de ORM sin afectar el dominio
    /// 
    /// Principio de Inversión de Dependencias (SOLID):
    /// El dominio define la interfaz, la infraestructura la implementa.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad del repositorio</typeparam>
    public interface IRepositorioGenerico<T> where T : class
    {
        /// <summary>
        /// Obtiene una entidad por su ID
        /// </summary>
        /// <param name="id">ID de la entidad</param>
        /// <returns>Entidad encontrada o null</returns>
        Task<T?> ObtenerPorIdAsync(int id);

        /// <summary>
        /// Obtiene todas las entidades
        /// ADVERTENCIA: Usar con cuidado en tablas grandes
        /// </summary>
        /// <returns>Colección de todas las entidades</returns>
        Task<IEnumerable<T>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene entidades que cumplen una condición
        /// Ejemplo: await repo.BuscarAsync(p => p.Precio > 1000 && p.EstaActivo)
        /// </summary>
        /// <param name="predicado">Expresión lambda para filtrar</param>
        /// <returns>Colección de entidades que cumplen la condición</returns>
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado);

        /// <summary>
        /// Obtiene entidades con paginación
        /// Fundamental para no traer miles de registros a memoria
        /// </summary>
        /// <param name="pagina">Número de página (base 1)</param>
        /// <param name="tamanoPagina">Cantidad de elementos por página</param>
        /// <returns>Colección paginada de entidades</returns>
        Task<IEnumerable<T>> ObtenerPaginadoAsync(int pagina, int tamanoPagina);

        /// <summary>
        /// Cuenta el total de entidades
        /// </summary>
        /// <returns>Cantidad total de registros</returns>
        Task<int> ContarAsync();

        /// <summary>
        /// Cuenta las entidades que cumplen una condición
        /// </summary>
        /// <param name="predicado">Expresión lambda para filtrar</param>
        /// <returns>Cantidad de registros que cumplen la condición</returns>
        Task<int> ContarAsync(Expression<Func<T, bool>> predicado);

        /// <summary>
        /// Verifica si existe al menos una entidad que cumple la condición
        /// Más eficiente que contar cuando solo necesitas saber si existe
        /// </summary>
        /// <param name="predicado">Expresión lambda para filtrar</param>
        /// <returns>True si existe al menos una entidad</returns>
        Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado);

        /// <summary>
        /// Agrega una nueva entidad
        /// IMPORTANTE: No se persiste hasta llamar a SaveChangesAsync() del UnitOfWork
        /// </summary>
        /// <param name="entidad">Entidad a agregar</param>
        Task AgregarAsync(T entidad);

        /// <summary>
        /// Agrega múltiples entidades de una vez
        /// Más eficiente que agregar una por una en un loop
        /// </summary>
        /// <param name="entidades">Colección de entidades a agregar</param>
        Task AgregarRangoAsync(IEnumerable<T> entidades);

        /// <summary>
        /// Actualiza una entidad existente
        /// IMPORTANTE: No se persiste hasta llamar a SaveChangesAsync() del UnitOfWork
        /// </summary>
        /// <param name="entidad">Entidad con los cambios</param>
        void Actualizar(T entidad);

        /// <summary>
        /// Elimina una entidad
        /// IMPORTANTE: Es eliminación física (DELETE). Para eliminación lógica,
        /// mejor actualizar un campo "EstaActivo" o similar.
        /// </summary>
        /// <param name="entidad">Entidad a eliminar</param>
        void Eliminar(T entidad);

        /// <summary>
        /// Elimina múltiples entidades de una vez
        /// </summary>
        /// <param name="entidades">Colección de entidades a eliminar</param>
        void EliminarRango(IEnumerable<T> entidades);
    }
}
```

---

## 📁 Interfaces/IRepositorioProducto.cs

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz específica para el repositorio de Productos.
    /// Hereda las operaciones básicas del repositorio genérico y
    /// agrega operaciones específicas del dominio de productos.
    /// 
    /// ¿POR QUÉ HEREDAR DE REPOSITORIO GENÉRICO?
    /// - Reutilizar operaciones comunes (ObtenerPorId, Agregar, etc.)
    /// - Agregar solo operaciones específicas de Producto
    /// - Mantener consistencia en la API
    /// </summary>
    public interface IRepositorioProducto : IRepositorioGenerico<Producto>
    {
        /// <summary>
        /// Obtiene un producto por su código SKU
        /// </summary>
        /// <param name="codigoSku">Código SKU del producto</param>
        /// <returns>Producto encontrado o null</returns>
        Task<Producto?> ObtenerPorSKUAsync(string codigoSku);

        /// <summary>
        /// Obtiene productos por categoría
        /// Incluye la información de la categoría y las imágenes
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns>Colección de productos de la categoría</returns>
        Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId);

        /// <summary>
        /// Obtiene productos por marca
        /// </summary>
        /// <param name="marcaId">ID de la marca</param>
        /// <returns>Colección de productos de la marca</returns>
        Task<IEnumerable<Producto>> ObtenerPorMarcaAsync(int marcaId);

        /// <summary>
        /// Obtiene productos destacados
        /// Útil para mostrar en página principal
        /// </summary>
        /// <param name="cantidad">Cantidad máxima de productos a retornar</param>
        /// <returns>Colección de productos destacados</returns>
        Task<IEnumerable<Producto>> ObtenerDestacadosAsync(int cantidad = 10);

        /// <summary>
        /// Obtiene productos con descuento activo
        /// </summary>
        /// <returns>Colección de productos en oferta</returns>
        Task<IEnumerable<Producto>> ObtenerEnOfertaAsync();

        /// <summary>
        /// Obtiene productos con poco stock
        /// Útil para alertas de inventario
        /// </summary>
        /// <param name="umbral">Cantidad mínima de stock</param>
        /// <returns>Colección de productos con stock bajo</returns>
        Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int umbral = 10);

        /// <summary>
        /// Busca productos por término de búsqueda
        /// Busca en nombre, descripción y SKU
        /// </summary>
        /// <param name="termino">Término a buscar</param>
        /// <returns>Colección de productos que coinciden con la búsqueda</returns>
        Task<IEnumerable<Producto>> BuscarPorTerminoAsync(string termino);

        /// <summary>
        /// Obtiene productos con filtros avanzados y paginación
        /// </summary>
        /// <param name="categoriaId">ID de categoría (opcional)</param>
        /// <param name="marcaId">ID de marca (opcional)</param>
        /// <param name="precioMin">Precio mínimo (opcional)</param>
        /// <param name="precioMax">Precio máximo (opcional)</param>
        /// <param name="pagina">Número de página</param>
        /// <param name="tamanoPagina">Elementos por página</param>
        /// <returns>Colección paginada de productos filtrados</returns>
        Task<(IEnumerable<Producto> productos, int totalRegistros)> ObtenerConFiltrosAsync(
            int? categoriaId = null,
            int? marcaId = null,
            decimal? precioMin = null,
            decimal? precioMax = null,
            int pagina = 1,
            int tamanoPagina = 20
        );

        /// <summary>
        /// Obtiene un producto con todas sus relaciones cargadas
        /// (Categoría, Marca, Variantes, Imágenes)
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Producto con relaciones completas o null</returns>
        Task<Producto?> ObtenerCompletoAsync(int id);

        /// <summary>
        /// Verifica si un SKU ya existe
        /// Útil para validaciones antes de crear/actualizar
        /// </summary>
        /// <param name="sku">SKU a verificar</param>
        /// <param name="idExcluir">ID del producto a excluir (para actualizaciones)</param>
        /// <returns>True si el SKU ya existe</returns>
        Task<bool> SKUExisteAsync(string sku, int? idExcluir = null);
    }
}
```

---

## 📁 Interfaces/IRepositorioCategoria.cs

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz específica para el repositorio de Categorías.
    /// </summary>
    public interface IRepositorioCategoria : IRepositorioGenerico<Categoria>
    {
        /// <summary>
        /// Obtiene una categoría por su slug
        /// Útil para URLs amigables como /categoria/vestidos-mujer
        /// </summary>
        /// <param name="slug">Slug de la categoría</param>
        /// <returns>Categoría encontrada o null</returns>
        Task<Categoria?> ObtenerPorSlugAsync(string slug);

        /// <summary>
        /// Obtiene las categorías raíz (sin categoría padre)
        /// </summary>
        /// <returns>Colección de categorías raíz</returns>
        Task<IEnumerable<Categoria>> ObtenerCategoriasRaizAsync();

        /// <summary>
        /// Obtiene las subcategorías de una categoría
        /// </summary>
        /// <param name="categoriaPadreId">ID de la categoría padre</param>
        /// <returns>Colección de subcategorías</returns>
        Task<IEnumerable<Categoria>> ObtenerSubCategoriasAsync(int categoriaPadreId);

        /// <summary>
        /// Obtiene el árbol completo de categorías
        /// Incluye todas las relaciones padre-hijo
        /// </summary>
        /// <returns>Colección jerárquica de categorías</returns>
        Task<IEnumerable<Categoria>> ObtenerArbolCompletAsync();

        /// <summary>
        /// Obtiene una categoría con todos sus productos
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría con productos o null</returns>
        Task<Categoria?> ObtenerConProductosAsync(int id);

        /// <summary>
        /// Verifica si una categoría tiene productos asociados
        /// Útil antes de eliminar una categoría
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>True si tiene productos</returns>
        Task<bool> TieneProductosAsync(int id);

        /// <summary>
        /// Verifica si un slug ya existe
        /// </summary>
        /// <param name="slug">Slug a verificar</param>
        /// <param name="idExcluir">ID de categoría a excluir (para actualizaciones)</param>
        /// <returns>True si el slug ya existe</returns>
        Task<bool> SlugExisteAsync(string slug, int? idExcluir = null);
    }
}
```

---

## 📁 Interfaces/IRepositorioOrden.cs

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz específica para el repositorio de Órdenes.
    /// </summary>
    public interface IRepositorioOrden : IRepositorioGenerico<Orden>
    {
        /// <summary>
        /// Obtiene una orden por su número
        /// </summary>
        /// <param name="numeroOrden">Número de orden</param>
        /// <returns>Orden encontrada o null</returns>
        Task<Orden?> ObtenerPorNumeroAsync(string numeroOrden);

        /// <summary>
        /// Obtiene una orden con todos sus detalles y relaciones
        /// </summary>
        /// <param name="id">ID de la orden</param>
        /// <returns>Orden completa o null</returns>
        Task<Orden?> ObtenerCompletaAsync(int id);

        /// <summary>
        /// Obtiene las órdenes de un usuario
        /// </summary>
        /// <param name="usuarioId">ID del usuario</param>
        /// <returns>Colección de órdenes del usuario</returns>
        Task<IEnumerable<Orden>> ObtenerPorUsuarioAsync(int usuarioId);

        /// <summary>
        /// Obtiene órdenes por estado
        /// </summary>
        /// <param name="estado">Estado de la orden</param>
        /// <returns>Colección de órdenes con ese estado</returns>
        Task<IEnumerable<Orden>> ObtenerPorEstadoAsync(EstadoOrden estado);

        /// <summary>
        /// Obtiene órdenes en un rango de fechas
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Colección de órdenes en el rango</returns>
        Task<IEnumerable<Orden>> ObtenerPorRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene el total de ventas en un período
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Total de ventas</returns>
        Task<decimal> ObtenerTotalVentasAsync(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Obtiene la cantidad de órdenes diarias (para generar número de orden)
        /// </summary>
        /// <param name="fecha">Fecha a consultar</param>
        /// <returns>Cantidad de órdenes en esa fecha</returns>
        Task<int> ObtenerContadorDiarioAsync(DateTime fecha);

        /// <summary>
        /// Obtiene las últimas órdenes creadas
        /// Útil para dashboard de administración
        /// </summary>
        /// <param name="cantidad">Cantidad de órdenes a retornar</param>
        /// <returns>Colección de órdenes recientes</returns>
        Task<IEnumerable<Orden>> ObtenerUltimasAsync(int cantidad = 10);
    }
}
```

---

## 📁 Interfaces/IRepositorioUsuario.cs

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz específica para el repositorio de Usuarios.
    /// </summary>
    public interface IRepositorioUsuario : IRepositorioGenerico<Usuario>
    {
        /// <summary>
        /// Obtiene un usuario por su email
        /// Usado principalmente para autenticación
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<Usuario?> ObtenerPorEmailAsync(string email);

        /// <summary>
        /// Verifica si un email ya está registrado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <param name="idExcluir">ID de usuario a excluir (para actualizaciones)</param>
        /// <returns>True si el email ya existe</returns>
        Task<bool> EmailExisteAsync(string email, int? idExcluir = null);

        /// <summary>
        /// Obtiene un usuario por su token de verificación de email
        /// </summary>
        /// <param name="token">Token de verificación</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<Usuario?> ObtenerPorTokenVerificacionAsync(string token);

        /// <summary>
        /// Obtiene un usuario por su token de recuperación de contraseña
        /// </summary>
        /// <param name="token">Token de recuperación</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<Usuario?> ObtenerPorTokenRecuperacionAsync(string token);

        /// <summary>
        /// Obtiene todos los administradores del sistema
        /// </summary>
        /// <returns>Colección de usuarios administradores</returns>
        Task<IEnumerable<Usuario>> ObtenerAdministradoresAsync();

        /// <summary>
        /// Obtiene los clientes más activos (por cantidad de órdenes)
        /// </summary>
        /// <param name="cantidad">Cantidad de clientes a retornar</param>
        /// <returns>Colección de clientes top</returns>
        Task<IEnumerable<Usuario>> ObtenerClientesTopAsync(int cantidad = 10);
    }
}
```

---

## 📁 Interfaces/IUnitOfWork.cs

```csharp
using System;
using System.Threading.Tasks;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz del patrón Unit of Work.
    /// 
    /// ¿QUÉ ES UNIT OF WORK?
    /// Un patrón que mantiene una lista de objetos afectados por una transacción
    /// de negocio y coordina la escritura de cambios.
    /// 
    /// ¿POR QUÉ LO NECESITAMOS?
    /// 1. Transacciones: Todos los cambios se confirman juntos o ninguno
    /// 2. Consistencia: Evita estados inconsistentes en la BD
    /// 3. Performance: Agrupa múltiples operaciones en una sola transacción
    /// 4. Simplicidad: No hay que llamar SaveChanges en cada repositorio
    /// 
    /// EJEMPLO DE USO:
    /// ```csharp
    /// var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(1);
    /// producto.CantidadStock -= 5;
    /// 
    /// var orden = new Orden { ... };
    /// await _unitOfWork.Ordenes.AgregarAsync(orden);
    /// 
    /// // Ambos cambios se guardan juntos
    /// await _unitOfWork.CompletarAsync();
    /// ```
    /// 
    /// Si algo falla, ningún cambio se aplica (rollback automático).
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Productos
        /// </summary>
        IRepositorioProducto Productos { get; }

        /// <summary>
        /// Repositorio de Categorías
        /// </summary>
        IRepositorioCategoria Categorias { get; }

        /// <summary>
        /// Repositorio de Órdenes
        /// </summary>
        IRepositorioOrden Ordenes { get; }

        /// <summary>
        /// Repositorio de Usuarios
        /// </summary>
        IRepositorioUsuario Usuarios { get; }

        // Agregar más repositorios según sea necesario:
        // IRepositorioVariante Variantes { get; }
        // IRepositorioMarca Marcas { get; }
        // etc.

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// dentro de una transacción.
        /// </summary>
        /// <returns>Número de registros afectados</returns>
        Task<int> CompletarAsync();

        /// <summary>
        /// Inicia una transacción explícita
        /// Útil cuando necesitas control fino sobre transacciones anidadas
        /// </summary>
        Task IniciarTransaccionAsync();

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        Task ConfirmarTransaccionAsync();

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        Task RevertirTransaccionAsync();
    }
}
```

---

## 🎓 RESUMEN: ARQUITECTURA DEL DOMAIN LAYER

### ✅ Lo que HEMOS DEFINIDO:

1. **Entidades** (Entities/):
   - Producto, Categoria, Variante, Imagen, Marca
   - Orden, DetalleOrden
   - Usuario

2. **Enumeraciones** (Enums/):
   - EstadoOrden
   - RolUsuario
   - TipoDescuento

3. **Interfaces** (Interfaces/):
   - IRepositorioGenerico<T>
   - IRepositorioProducto
   - IRepositorioCategoria
   - IRepositorioOrden
   - IRepositorioUsuario
   - IUnitOfWork

### 🎯 VENTAJAS DE ESTA ARQUITECTURA:

#### 1. **Independencia de Tecnología**
```
Domain Layer NO CONOCE:
❌ Entity Framework Core
❌ ASP.NET Core
❌ MySQL
❌ Ninguna librería externa

Solo define:
✅ Entidades de negocio
✅ Reglas de negocio
✅ Contratos (interfaces)
```

#### 2. **Testeable**
```csharp
// Puedes probar lógica de negocio sin base de datos:
var producto = new Producto {
    PrecioBase = 1000,
    PorcentajeDescuento = 20
};

Assert.Equal(800, producto.PrecioFinal); // ✅ Test unitario puro
```

#### 3. **Reutilizable**
```
Este Domain Layer puede usarse en:
✅ API REST
✅ API GraphQL
✅ Aplicación de Consola
✅ Blazor Server
✅ Worker Service
✅ Azure Functions
```

#### 4. **Mantenible**
```
Si cambiamos de MySQL a PostgreSQL:
- Domain Layer: Sin cambios ✅
- Application Layer: Sin cambios ✅
- Infrastructure Layer: Cambiar provider de EF Core
- API Layer: Sin cambios ✅
```

### 📋 PRÓXIMOS PASOS:

1. **Infrastructure Layer**: Implementar las interfaces con EF Core
2. **Application Layer**: Crear servicios y DTOs
3. **API Layer**: Exponer endpoints REST
4. **Frontend**: Consumir la API desde Vue 3

---

### 💡 RECORDATORIOS IMPORTANTES:

1. **El Dominio es el Rey**: Todas las otras capas existen para servir al dominio
2. **Reglas de Negocio en Entidades**: Los métodos como `ReducirStock()`, `AplicarDescuento()` encapsulan lógica de negocio
3. **Validaciones en Dos Niveles**:
   - Entidades: Validaciones básicas (ej: stock no negativo)
   - Application Layer: Validaciones de negocio complejas (con FluentValidation)
4. **Nunca Exponer Entidades Directamente**: Usar DTOs en la capa de aplicación

---

¡El Domain Layer está COMPLETO! 🎉
