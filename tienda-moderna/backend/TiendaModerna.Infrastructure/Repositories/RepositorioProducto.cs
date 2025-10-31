using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para Productos.
    /// 
    /// Hereda funcionalidad común de RepositorioGenerico<Producto>
    /// y agrega métodos especializados para productos.
    /// </summary>
    public class RepositorioProducto : RepositorioGenerico<Producto>, IRepositorioProducto
    {
        public RepositorioProducto(TiendaContext contexto) : base(contexto)
        {
        }

        // ============ Métodos Especializados ============

        /// <summary>
        /// Obtiene un producto con todas sus relaciones (variantes, imágenes, categoría, marca).
        /// </summary>
        public async Task<Producto?> ObtenerCompletoAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Variantes)
                .Include(p => p.Imagenes)
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Obtiene un producto por su código SKU.
        /// </summary>
        public async Task<Producto?> ObtenerPorSKUAsync(string codigoSku)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Imagenes)
                .FirstOrDefaultAsync(p => p.CodigoSKU == codigoSku);
        }

        /// <summary>
        /// Obtiene productos por categoría.
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerPorCategoriaAsync(int categoriaId)
        {
            return await _dbSet
                .Include(p => p.Marca)
                .Include(p => p.Imagenes)
                .Where(p => p.CategoriaId == categoriaId && p.EstaActivo)
                .OrderByDescending(p => p.EsDestacado)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos por marca.
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerPorMarcaAsync(int marcaId)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Imagenes)
                .Where(p => p.MarcaId == marcaId && p.EstaActivo)
                .OrderByDescending(p => p.EsDestacado)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos destacados.
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerDestacadosAsync(int cantidad = 10)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Imagenes)
                .Where(p => p.EstaActivo && p.EsDestacado)
                .OrderByDescending(p => p.FechaCreacion)
                .Take(cantidad)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos con descuento activo.
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerEnOfertaAsync()
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Imagenes)
                .Where(p => p.EstaActivo && p.PorcentajeDescuento > 0)
                .OrderByDescending(p => p.PorcentajeDescuento)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos con stock bajo.
        /// </summary>
        public async Task<IEnumerable<Producto>> ObtenerConStockBajoAsync(int umbral = 10)
        {
            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Where(p => p.EstaActivo && p.CantidadStock <= umbral)
                .OrderBy(p => p.CantidadStock)
                .ToListAsync();
        }

        /// <summary>
        /// Busca productos por término (nombre, descripción, SKU).
        /// </summary>
        public async Task<IEnumerable<Producto>> BuscarPorTerminoAsync(string termino)
        {
            var terminoLower = termino.ToLower();

            return await _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Imagenes)
                .Where(p => p.EstaActivo &&
                           (p.Nombre.ToLower().Contains(terminoLower) ||
                           (p.Descripcion != null && p.Descripcion.ToLower().Contains(terminoLower)) ||
                           p.CodigoSKU.ToLower().Contains(terminoLower)))
                .OrderByDescending(p => p.EsDestacado)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene productos con filtros avanzados y paginación.
        /// </summary>
        public async Task<(IEnumerable<Producto> productos, int totalRegistros)> ObtenerConFiltrosAsync(
            int? categoriaId = null,
            int? marcaId = null,
            decimal? precioMin = null,
            decimal? precioMax = null,
            int pagina = 1,
            int tamanoPagina = 20)
        {
            var query = _dbSet
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Imagenes)
                .Where(p => p.EstaActivo)
                .AsQueryable();

            if (categoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == categoriaId.Value);
            }

            if (marcaId.HasValue)
            {
                query = query.Where(p => p.MarcaId == marcaId.Value);
            }

            if (precioMin.HasValue)
            {
                query = query.Where(p => p.PrecioBase >= precioMin.Value);
            }

            if (precioMax.HasValue)
            {
                query = query.Where(p => p.PrecioBase <= precioMax.Value);
            }

            var totalRegistros = await query.CountAsync();

            var productos = await query
                .OrderByDescending(p => p.EsDestacado)
                .ThenBy(p => p.Nombre)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            return (productos, totalRegistros);
        }

        /// <summary>
        /// Verifica si un SKU ya existe.
        /// </summary>
        public async Task<bool> SKUExisteAsync(string sku, int? idExcluir = null)
        {
            var query = _dbSet.Where(p => p.CodigoSKU == sku);

            if (idExcluir.HasValue)
            {
                query = query.Where(p => p.Id != idExcluir.Value);
            }

            return await query.AnyAsync();
        }
    }
}
