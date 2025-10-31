using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para Categorías.
    /// 
    /// Maneja la jerarquía de categorías (padre-hijos).
    /// </summary>
    public class RepositorioCategoria : RepositorioGenerico<Categoria>, IRepositorioCategoria
    {
        public RepositorioCategoria(TiendaContext contexto) : base(contexto)
        {
        }

        // ============ Métodos Especializados ============

        /// <summary>
        /// Obtiene una categoría por su slug.
        /// </summary>
        public async Task<Categoria?> ObtenerPorSlugAsync(string slug)
        {
            return await _dbSet
                .Include(c => c.SubCategorias)
                .Include(c => c.Productos.Where(p => p.EstaActivo))
                .FirstOrDefaultAsync(c => c.Slug == slug);
        }

        /// <summary>
        /// Obtiene categorías raíz (sin padre).
        /// </summary>
        public async Task<IEnumerable<Categoria>> ObtenerCategoriasRaizAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategorias)
                .Where(c => c.CategoriaPadreId == null && c.EstaActiva)
                .OrderBy(c => c.Orden)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene subcategorías de una categoría padre.
        /// </summary>
        public async Task<IEnumerable<Categoria>> ObtenerSubCategoriasAsync(int categoriaPadreId)
        {
            return await _dbSet
                .Where(c => c.CategoriaPadreId == categoriaPadreId && c.EstaActiva)
                .OrderBy(c => c.Orden)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene el árbol completo de categorías.
        /// </summary>
        public async Task<IEnumerable<Categoria>> ObtenerArbolCompletoAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategorias)
                .ThenInclude(sc => sc.SubCategorias)
                .Where(c => c.EstaActiva)
                .OrderBy(c => c.Orden)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene una categoría con todos sus productos.
        /// </summary>
        public async Task<Categoria?> ObtenerConProductosAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Productos.Where(p => p.EstaActivo))
                    .ThenInclude(p => p.Imagenes)
                .Include(c => c.SubCategorias)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Verifica si una categoría tiene productos.
        /// </summary>
        public async Task<bool> TieneProductosAsync(int id)
        {
            return await _contexto.Productos.AnyAsync(p => p.CategoriaId == id);
        }

        /// <summary>
        /// Verifica si un slug ya existe.
        /// </summary>
        public async Task<bool> SlugExisteAsync(string slug, int? idExcluir = null)
        {
            var query = _dbSet.Where(c => c.Slug == slug);

            if (idExcluir.HasValue)
            {
                query = query.Where(c => c.Id != idExcluir.Value);
            }

            return await query.AnyAsync();
        }
    }
}
