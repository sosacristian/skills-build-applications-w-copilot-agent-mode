using TiendaModerna.Application.DTOs.Common;
using TiendaModerna.Application.DTOs.Orden;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de órdenes
    /// </summary>
    public interface IOrdenService
    {
        Task<OrdenDto?> ObtenerPorIdAsync(int id);
        Task<OrdenDto?> ObtenerPorNumeroAsync(string numeroOrden);
        Task<PagedResult<OrdenDto>> ObtenerPorUsuarioAsync(int usuarioId, int pagina, int tamanoPagina);
        Task<PagedResult<OrdenDto>> ObtenerPorEstadoAsync(EstadoOrden estado, int pagina, int tamanoPagina);
        Task<OrdenDto> CrearAsync(CrearOrdenDto dto);
        Task<bool> ActualizarEstadoAsync(int ordenId, EstadoOrden nuevoEstado);
        Task<bool> CancelarAsync(int ordenId);
        Task<bool> MarcarComoPagadaAsync(int ordenId, string idTransaccion);
        Task<bool> MarcarComoEnviadaAsync(int ordenId, string empresaTransporte, string codigoSeguimiento);
        Task<bool> MarcarComoEntregadaAsync(int ordenId);
        Task<decimal> ObtenerTotalVentasAsync(DateTime? desde = null, DateTime? hasta = null);
    }
}
