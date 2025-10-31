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
