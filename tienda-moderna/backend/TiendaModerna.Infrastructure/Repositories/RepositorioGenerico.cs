using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación genérica del repositorio base.
    /// 
    /// PATRÓN: Repository Pattern
    /// - Abstrae el acceso a datos
    /// - Centraliza operaciones CRUD comunes
    /// - Facilita testing (se puede mockear)
    /// - Desacopla Domain de Infrastructure
    /// 
    /// PRINCIPIO: DRY (Don't Repeat Yourself)
    /// - Implementa operaciones comunes una sola vez
    /// - Los repositorios específicos heredan esta funcionalidad
    /// </summary>
    public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
    {
        protected readonly TiendaContext _contexto;
        protected readonly DbSet<T> _dbSet;

        public RepositorioGenerico(TiendaContext contexto)
        {
            _contexto = contexto;
            _dbSet = contexto.Set<T>();
        }

        // ============ Operaciones de Lectura ============

        public virtual async Task<T?> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicado)
        {
            return await _dbSet.Where(predicado).ToListAsync();
        }

        public virtual async Task<T?> ObtenerPrimeroAsync(Expression<Func<T, bool>> predicado)
        {
            return await _dbSet.FirstOrDefaultAsync(predicado);
        }

        public virtual async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado)
        {
            return await _dbSet.AnyAsync(predicado);
        }

        public virtual async Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null)
        {
            if (predicado == null)
                return await _dbSet.CountAsync();

            return await _dbSet.CountAsync(predicado);
        }

        public virtual async Task<int> ContarAsync()
        {
            return await _dbSet.CountAsync();
        }

        // ============ Operaciones con Paginación ============

        /// <summary>
        /// Obtiene una página de resultados.
        /// </summary>
        /// <param name="numeroPagina">Número de página (comienza en 1)</param>
        /// <param name="tamanioPagina">Cantidad de elementos por página</param>
        /// <returns>Lista paginada de entidades</returns>
        public virtual async Task<IEnumerable<T>> ObtenerPaginadoAsync(int numeroPagina, int tamanioPagina)
        {
            return await _dbSet
                .Skip((numeroPagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una página de resultados con filtro.
        /// </summary>
        public virtual async Task<IEnumerable<T>> ObtenerPaginadoAsync(
            Expression<Func<T, bool>> predicado,
            int numeroPagina,
            int tamanioPagina)
        {
            return await _dbSet
                .Where(predicado)
                .Skip((numeroPagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una página de resultados ordenados.
        /// </summary>
        public virtual async Task<IEnumerable<T>> ObtenerPaginadoAsync<TKey>(
            Expression<Func<T, TKey>> ordenarPor,
            bool ascendente,
            int numeroPagina,
            int tamanioPagina)
        {
            IQueryable<T> query = _dbSet;

            query = ascendente
                ? query.OrderBy(ordenarPor)
                : query.OrderByDescending(ordenarPor);

            return await query
                .Skip((numeroPagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();
        }

        // ============ Operaciones de Escritura ============

        public virtual async Task AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
        }

        public virtual async Task AgregarRangoAsync(IEnumerable<T> entidades)
        {
            await _dbSet.AddRangeAsync(entidades);
        }

        public virtual void Actualizar(T entidad)
        {
            _dbSet.Update(entidad);
        }

        public virtual void Eliminar(T entidad)
        {
            _dbSet.Remove(entidad);
        }

        public virtual void EliminarRango(IEnumerable<T> entidades)
        {
            _dbSet.RemoveRange(entidades);
        }

        // ============ Operaciones con Includes (Eager Loading) ============

        /// <summary>
        /// Obtiene entidades incluyendo propiedades de navegación relacionadas.
        /// Ejemplo: ObtenerConIncludesAsync(p => p.Categoria, p => p.Marca)
        /// </summary>
        public virtual async Task<IEnumerable<T>> ObtenerConIncludesAsync(
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Busca entidades con filtro e includes.
        /// </summary>
        public virtual async Task<IEnumerable<T>> BuscarConIncludesAsync(
            Expression<Func<T, bool>> predicado,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(predicado).ToListAsync();
        }

        /// <summary>
        /// Obtiene la primera entidad que coincida con el predicado, incluyendo propiedades de navegación.
        /// </summary>
        public virtual async Task<T?> ObtenerPrimeroConIncludesAsync(
            Expression<Func<T, bool>> predicado,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(predicado);
        }
    }
}
