using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaModerna.Application.DTOs.Common;
using TiendaModerna.Application.DTOs.Orden;
using TiendaModerna.Application.Interfaces;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.API.Controllers
{
    /// <summary>
    /// Controlador para gestión de órdenes
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdenesController : ControllerBase
    {
        private readonly IOrdenService _ordenService;
        private readonly ILogger<OrdenesController> _logger;

        public OrdenesController(IOrdenService ordenService, ILogger<OrdenesController> logger)
        {
            _ordenService = ordenService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener orden por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrdenDto>> ObtenerPorId(int id)
        {
            try
            {
                var orden = await _ordenService.ObtenerPorIdAsync(id);
                if (orden == null)
                    return NotFound(new { error = "Orden no encontrada" });

                // Verificar que el usuario pueda ver esta orden
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var esAdmin = User.IsInRole("Administrador") || User.IsInRole("SuperAdministrador");
                
                if (orden.UsuarioId != usuarioId && !esAdmin)
                    return Forbid();

                return Ok(orden);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener orden {Id}", id);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener orden por número de orden
        /// </summary>
        [HttpGet("numero/{numeroOrden}")]
        [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrdenDto>> ObtenerPorNumero(string numeroOrden)
        {
            try
            {
                var orden = await _ordenService.ObtenerPorNumeroAsync(numeroOrden);
                if (orden == null)
                    return NotFound(new { error = "Orden no encontrada" });

                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var esAdmin = User.IsInRole("Administrador") || User.IsInRole("SuperAdministrador");
                
                if (orden.UsuarioId != usuarioId && !esAdmin)
                    return Forbid();

                return Ok(orden);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener orden por número");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener órdenes del usuario autenticado
        /// </summary>
        [HttpGet("mis-ordenes")]
        [ProducesResponseType(typeof(PagedResult<OrdenDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrdenDto>>> ObtenerMisOrdenes(
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanoPagina = 10)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var resultado = await _ordenService.ObtenerPorUsuarioAsync(usuarioId, pagina, tamanoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener órdenes del usuario");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener órdenes por estado (solo administradores)
        /// </summary>
        [HttpGet("por-estado/{estado}")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(typeof(PagedResult<OrdenDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<OrdenDto>>> ObtenerPorEstado(
            EstadoOrden estado,
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanoPagina = 20)
        {
            try
            {
                var resultado = await _ordenService.ObtenerPorEstadoAsync(estado, pagina, tamanoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener órdenes por estado");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crear una nueva orden
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrdenDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrdenDto>> Crear([FromBody] CrearOrdenDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Asegurar que la orden sea del usuario autenticado
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (dto.UsuarioId != usuarioId)
                    return Forbid();

                var orden = await _ordenService.CrearAsync(dto);
                _logger.LogInformation("Orden creada: {NumeroOrden}", orden.NumeroOrden);
                
                return CreatedAtAction(nameof(ObtenerPorId), new { id = orden.Id }, orden);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error al crear orden: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear orden");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Cancelar una orden
        /// </summary>
        [HttpPost("{id}/cancelar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var orden = await _ordenService.ObtenerPorIdAsync(id);
                if (orden == null)
                    return NotFound(new { error = "Orden no encontrada" });

                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var esAdmin = User.IsInRole("Administrador") || User.IsInRole("SuperAdministrador");
                
                if (orden.UsuarioId != usuarioId && !esAdmin)
                    return Forbid();

                var resultado = await _ordenService.CancelarAsync(id);
                if (!resultado)
                    return BadRequest(new { error = "No se pudo cancelar la orden" });

                _logger.LogInformation("Orden cancelada: {Id}", id);
                return Ok(new { mensaje = "Orden cancelada exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar orden");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Marcar orden como pagada (solo administradores)
        /// </summary>
        [HttpPost("{id}/marcar-pagada")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MarcarComoPagada(int id, [FromBody] string idTransaccion)
        {
            try
            {
                var resultado = await _ordenService.MarcarComoPagadaAsync(id, idTransaccion);
                if (!resultado)
                    return NotFound(new { error = "Orden no encontrada" });

                _logger.LogInformation("Orden marcada como pagada: {Id}", id);
                return Ok(new { mensaje = "Orden marcada como pagada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar orden como pagada");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Marcar orden como enviada (solo administradores)
        /// </summary>
        [HttpPost("{id}/marcar-enviada")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> MarcarComoEnviada(int id, [FromBody] EnvioDto dto)
        {
            try
            {
                var resultado = await _ordenService.MarcarComoEnviadaAsync(id, dto.EmpresaTransporte, dto.CodigoSeguimiento);
                if (!resultado)
                    return NotFound(new { error = "Orden no encontrada" });

                _logger.LogInformation("Orden marcada como enviada: {Id}", id);
                return Ok(new { mensaje = "Orden marcada como enviada" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar orden como enviada");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener total de ventas (solo administradores)
        /// </summary>
        [HttpGet("total-ventas")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> ObtenerTotalVentas(
            [FromQuery] DateTime? desde = null, 
            [FromQuery] DateTime? hasta = null)
        {
            try
            {
                var total = await _ordenService.ObtenerTotalVentasAsync(desde, hasta);
                return Ok(new { total, desde, hasta });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener total de ventas");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }

    /// <summary>
    /// DTO para información de envío
    /// </summary>
    public class EnvioDto
    {
        public string EmpresaTransporte { get; set; } = string.Empty;
        public string CodigoSeguimiento { get; set; } = string.Empty;
    }
}
