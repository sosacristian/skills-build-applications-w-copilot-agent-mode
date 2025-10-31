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
