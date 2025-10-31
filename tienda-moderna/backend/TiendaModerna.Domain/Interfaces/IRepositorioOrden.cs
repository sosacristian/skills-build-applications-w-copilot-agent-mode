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
