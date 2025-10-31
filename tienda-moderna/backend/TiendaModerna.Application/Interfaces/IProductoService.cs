using TiendaModerna.Application.DTOs.Producto;
using TiendaModerna.Application.DTOs.Common;

namespace TiendaModerna.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de productos
    /// </summary>
    public interface IProductoService
    {
        Task<ProductoDto?> ObtenerPorIdAsync(int id);
        Task<ProductoDto?> ObtenerPorSKUAsync(string sku);
        Task<PagedResult<ProductoListaDto>> ObtenerPaginadoAsync(int pagina, int tamanoPagina);
        Task<PagedResult<ProductoListaDto>> ObtenerPorCategoriaAsync(int categoriaId, int pagina, int tamanoPagina);
        Task<PagedResult<ProductoListaDto>> ObtenerPorMarcaAsync(int marcaId, int pagina, int tamanoPagina);
        Task<List<ProductoListaDto>> ObtenerDestacadosAsync(int cantidad = 10);
        Task<List<ProductoListaDto>> ObtenerEnOfertaAsync(int cantidad = 10);
        Task<PagedResult<ProductoListaDto>> BuscarAsync(string termino, int pagina, int tamanoPagina);
        Task<ProductoDto> CrearAsync(CrearProductoDto dto);
        Task<ProductoDto> ActualizarAsync(ActualizarProductoDto dto);
        Task<bool> EliminarAsync(int id);
        Task<bool> CambiarEstadoAsync(int id, bool estaActivo);
        Task<bool> ActualizarStockAsync(int id, int nuevoStock);
        Task<bool> SKUExisteAsync(string sku, int? idExcluir = null);
    }
}
