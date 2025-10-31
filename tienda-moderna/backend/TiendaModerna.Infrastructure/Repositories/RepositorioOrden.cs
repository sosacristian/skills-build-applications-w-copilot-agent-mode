using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Enums;
using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para Órdenes.
    /// 
    /// Maneja el ciclo de vida completo de las órdenes y reportes de ventas.
    /// </summary>
    public class RepositorioOrden : RepositorioGenerico<Orden>, IRepositorioOrden
    {
        public RepositorioOrden(TiendaContext contexto) : base(contexto)
        {
        }

        // ============ Métodos Especializados ============

        /// <summary>
        /// Obtiene una orden por su número.
        /// </summary>
        public async Task<Orden?> ObtenerPorNumeroAsync(string numeroOrden)
        {
            return await _dbSet
                .Include(o => o.Usuario)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(o => o.NumeroOrden == numeroOrden);
        }

        /// <summary>
        /// Obtiene una orden con todos sus detalles y relaciones.
        /// </summary>
        public async Task<Orden?> ObtenerCompletaAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Usuario)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Producto)
                .Include(o => o.Detalles)
                    .ThenInclude(d => d.Variante)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        /// <summary>
        /// Obtiene órdenes de un usuario.
        /// </summary>
        public async Task<IEnumerable<Orden>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Include(o => o.Detalles)
                .Where(o => o.UsuarioId == usuarioId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene órdenes por estado.
        /// </summary>
        public async Task<IEnumerable<Orden>> ObtenerPorEstadoAsync(EstadoOrden estado)
        {
            return await _dbSet
                .Include(o => o.Usuario)
                .Include(o => o.Detalles)
                .Where(o => o.Estado == estado)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene órdenes en un rango de fechas.
        /// </summary>
        public async Task<IEnumerable<Orden>> ObtenerPorRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Include(o => o.Usuario)
                .Include(o => o.Detalles)
                .Where(o => o.FechaCreacion >= fechaInicio && o.FechaCreacion <= fechaFin)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene el total de ventas en un período.
        /// </summary>
        public async Task<decimal> ObtenerTotalVentasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _dbSet
                .Where(o => o.FechaCreacion >= fechaInicio && 
                           o.FechaCreacion <= fechaFin &&
                           o.Estado != EstadoOrden.Cancelada)
                .SumAsync(o => o.Total);
        }

        /// <summary>
        /// Obtiene la cantidad de órdenes diarias (para generar número de orden).
        /// </summary>
        public async Task<int> ObtenerContadorDiarioAsync(DateTime fecha)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fecha.Date.AddDays(1);

            return await _dbSet
                .CountAsync(o => o.FechaCreacion >= fechaInicio && o.FechaCreacion < fechaFin);
        }

        /// <summary>
        /// Obtiene las últimas órdenes creadas.
        /// </summary>
        public async Task<IEnumerable<Orden>> ObtenerUltimasAsync(int cantidad = 10)
        {
            return await _dbSet
                .Include(o => o.Usuario)
                .Include(o => o.Detalles)
                .OrderByDescending(o => o.FechaCreacion)
                .Take(cantidad)
                .ToListAsync();
        }
    }
}
