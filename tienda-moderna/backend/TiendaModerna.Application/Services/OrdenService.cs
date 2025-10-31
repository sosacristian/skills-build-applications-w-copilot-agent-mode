using AutoMapper;
using TiendaModerna.Application.DTOs.Common;
using TiendaModerna.Application.DTOs.Orden;
using TiendaModerna.Application.Interfaces;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Enums;
using TiendaModerna.Domain.Interfaces;

namespace TiendaModerna.Application.Services
{
    /// <summary>
    /// Servicio de órdenes con lógica de negocio
    /// </summary>
    public class OrdenService : IOrdenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdenService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrdenDto?> ObtenerPorIdAsync(int id)
        {
            var orden = await _unitOfWork.Ordenes.ObtenerCompletaAsync(id);
            return orden != null ? _mapper.Map<OrdenDto>(orden) : null;
        }

        public async Task<OrdenDto?> ObtenerPorNumeroAsync(string numeroOrden)
        {
            var orden = await _unitOfWork.Ordenes.ObtenerPorNumeroAsync(numeroOrden);
            if (orden == null) return null;

            // Recargar completa
            orden = await _unitOfWork.Ordenes.ObtenerCompletaAsync(orden.Id);
            return _mapper.Map<OrdenDto>(orden);
        }

        public async Task<PagedResult<OrdenDto>> ObtenerPorUsuarioAsync(int usuarioId, int pagina, int tamanoPagina)
        {
            var ordenesEnumerable = await _unitOfWork.Ordenes.ObtenerPorUsuarioAsync(usuarioId);
            var ordenes = ordenesEnumerable.ToList();
            var total = ordenes.Count;

            var ordenesPaginadas = ordenes
                .OrderByDescending(o => o.FechaCreacion)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return new PagedResult<OrdenDto>
            {
                Items = _mapper.Map<List<OrdenDto>>(ordenesPaginadas),
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina)
            };
        }

        public async Task<PagedResult<OrdenDto>> ObtenerPorEstadoAsync(EstadoOrden estado, int pagina, int tamanoPagina)
        {
            var ordenesEnumerable = await _unitOfWork.Ordenes.ObtenerPorEstadoAsync(estado);
            var ordenes = ordenesEnumerable.ToList();
            var total = ordenes.Count;

            var ordenesPaginadas = ordenes
                .OrderByDescending(o => o.FechaCreacion)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return new PagedResult<OrdenDto>
            {
                Items = _mapper.Map<List<OrdenDto>>(ordenesPaginadas),
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina)
            };
        }

        public async Task<OrdenDto> CrearAsync(CrearOrdenDto dto)
        {
            // Iniciar transacción
            await _unitOfWork.IniciarTransaccionAsync();

            try
            {
                var orden = _mapper.Map<Orden>(dto);
                orden.NumeroOrden = await GenerarNumeroOrdenAsync();
                orden.Estado = EstadoOrden.Pendiente;
                orden.Detalles = new List<DetalleOrden>();

                decimal subtotal = 0;
                decimal totalDescuentos = 0;

                // Procesar cada item del carrito
                foreach (var itemDto in dto.Detalles)
                {
                    var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(itemDto.ProductoId);
                    if (producto == null)
                    {
                        throw new InvalidOperationException($"Producto {itemDto.ProductoId} no encontrado");
                    }

                    if (!producto.EstaActivo)
                    {
                        throw new InvalidOperationException($"El producto '{producto.Nombre}' no está disponible");
                    }

                    if (producto.CantidadStock < itemDto.Cantidad)
                    {
                        throw new InvalidOperationException($"Stock insuficiente para '{producto.Nombre}'");
                    }

                    var descuentoUnitario = (producto.PrecioBase * producto.PorcentajeDescuento) / 100;
                    
                    var detalle = new DetalleOrden
                    {
                        ProductoId = producto.Id,
                        Cantidad = itemDto.Cantidad,
                        PrecioUnitario = producto.PrecioBase,
                        DescuentoUnitario = descuentoUnitario,
                        Subtotal = producto.PrecioBase * itemDto.Cantidad,
                        Total = producto.PrecioFinal * itemDto.Cantidad
                    };

                    orden.Detalles.Add(detalle);
                    subtotal += detalle.Subtotal;
                    totalDescuentos += (descuentoUnitario * itemDto.Cantidad);

                    // Reducir stock
                    producto.CantidadStock -= itemDto.Cantidad;
                    _unitOfWork.Productos.Actualizar(producto);
                }

                // Calcular totales
                orden.Subtotal = subtotal;
                orden.TotalDescuentos = totalDescuentos;
                orden.CostoEnvio = CalcularCostoEnvio(orden.Subtotal, orden.Provincia);
                orden.Total = orden.Subtotal - orden.TotalDescuentos + orden.CostoEnvio;

                await _unitOfWork.Ordenes.AgregarAsync(orden);
                await _unitOfWork.CompletarAsync();
                await _unitOfWork.ConfirmarTransaccionAsync();

                // Recargar orden completa
                var ordenCompleta = await _unitOfWork.Ordenes.ObtenerCompletaAsync(orden.Id);
                return _mapper.Map<OrdenDto>(ordenCompleta!);
            }
            catch
            {
                await _unitOfWork.RevertirTransaccionAsync();
                throw;
            }
        }

        public async Task<bool> ActualizarEstadoAsync(int ordenId, EstadoOrden nuevoEstado)
        {
            var orden = await _unitOfWork.Ordenes.ObtenerPorIdAsync(ordenId);
            if (orden == null) return false;

            orden.Estado = nuevoEstado;

            if (nuevoEstado == EstadoOrden.Enviada && !orden.FechaEnvio.HasValue)
            {
                orden.FechaEnvio = DateTime.UtcNow;
            }
            else if (nuevoEstado == EstadoOrden.Entregada && !orden.FechaEntrega.HasValue)
            {
                orden.FechaEntrega = DateTime.UtcNow;
            }

            _unitOfWork.Ordenes.Actualizar(orden);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> CancelarAsync(int ordenId)
        {
            var orden = await _unitOfWork.Ordenes.ObtenerCompletaAsync(ordenId);
            if (orden == null) return false;

            if (orden.Estado != EstadoOrden.Pendiente && orden.Estado != EstadoOrden.Pagada)
            {
                throw new InvalidOperationException("Solo se pueden cancelar órdenes pendientes o pagadas");
            }

            // Restaurar stock
            foreach (var detalle in orden.Detalles)
            {
                var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.CantidadStock += detalle.Cantidad;
                    _unitOfWork.Productos.Actualizar(producto);
                }
            }

            orden.Estado = EstadoOrden.Cancelada;
            _unitOfWork.Ordenes.Actualizar(orden);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> MarcarComoPagadaAsync(int ordenId, string idTransaccion)
        {
            var orden = await _unitOfWork.Ordenes.ObtenerPorIdAsync(ordenId);
            if (orden == null) return false;

            orden.Estado = EstadoOrden.Pagada;
            orden.IdTransaccionPago = idTransaccion;
            orden.FechaPago = DateTime.UtcNow;

            _unitOfWork.Ordenes.Actualizar(orden);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> MarcarComoEnviadaAsync(int ordenId, string empresaTransporte, string codigoSeguimiento)
        {
            var orden = await _unitOfWork.Ordenes.ObtenerPorIdAsync(ordenId);
            if (orden == null) return false;

            orden.Estado = EstadoOrden.Enviada;
            orden.EmpresaTransporte = empresaTransporte;
            orden.CodigoSeguimiento = codigoSeguimiento;
            orden.FechaEnvio = DateTime.UtcNow;

            _unitOfWork.Ordenes.Actualizar(orden);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> MarcarComoEntregadaAsync(int ordenId)
        {
            return await ActualizarEstadoAsync(ordenId, EstadoOrden.Entregada);
        }

        public async Task<decimal> ObtenerTotalVentasAsync(DateTime? desde = null, DateTime? hasta = null)
        {
            return await _unitOfWork.Ordenes.ObtenerTotalVentasAsync(
                desde ?? DateTime.UtcNow.AddMonths(-1),
                hasta ?? DateTime.UtcNow
            );
        }

        private async Task<string> GenerarNumeroOrdenAsync()
        {
            var contador = await _unitOfWork.Ordenes.ObtenerContadorDiarioAsync(DateTime.UtcNow.Date);
            var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
            return $"ORD-{fecha}-{contador + 1:D4}";
        }

        private decimal CalcularCostoEnvio(decimal subtotal, string provincia)
        {
            // Lógica de cálculo de envío
            if (subtotal >= 50000) return 0; // Envío gratis

            return provincia.ToUpper() switch
            {
                "BUENOS AIRES" or "CAPITAL FEDERAL" or "CABA" => 2500,
                "CORDOBA" or "SANTA FE" or "MENDOZA" => 3500,
                _ => 4500
            };
        }
    }
}
