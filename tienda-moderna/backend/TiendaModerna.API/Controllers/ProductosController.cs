using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaModerna.Application.DTOs.Common;
using TiendaModerna.Application.DTOs.Producto;
using TiendaModerna.Application.Interfaces;

namespace TiendaModerna.API.Controllers
{
    /// <summary>
    /// Controlador para gestión de productos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductoService productoService, ILogger<ProductosController> logger)
        {
            _productoService = productoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los productos paginados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ProductoListaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductoListaDto>>> ObtenerTodos([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20)
        {
            try
            {
                var resultado = await _productoService.ObtenerPaginadoAsync(pagina, tamanoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener producto por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> ObtenerPorId(int id)
        {
            try
            {
                var producto = await _productoService.ObtenerPorIdAsync(id);
                if (producto == null)
                    return NotFound(new { error = "Producto no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto {Id}", id);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener producto por SKU
        /// </summary>
        [HttpGet("sku/{sku}")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> ObtenerPorSKU(string sku)
        {
            try
            {
                var producto = await _productoService.ObtenerPorSKUAsync(sku);
                if (producto == null)
                    return NotFound(new { error = "Producto no encontrado" });

                return Ok(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener producto por SKU {SKU}", sku);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Buscar productos por término
        /// </summary>
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(PagedResult<ProductoListaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductoListaDto>>> Buscar(
            [FromQuery] string termino, 
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanoPagina = 20)
        {
            try
            {
                var resultado = await _productoService.BuscarAsync(termino, pagina, tamanoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar productos");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener productos por categoría
        /// </summary>
        [HttpGet("categoria/{categoriaId}")]
        [ProducesResponseType(typeof(PagedResult<ProductoListaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ProductoListaDto>>> ObtenerPorCategoria(
            int categoriaId, 
            [FromQuery] int pagina = 1, 
            [FromQuery] int tamanoPagina = 20)
        {
            try
            {
                var resultado = await _productoService.ObtenerPorCategoriaAsync(categoriaId, pagina, tamanoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos por categoría");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener productos destacados
        /// </summary>
        [HttpGet("destacados")]
        [ProducesResponseType(typeof(List<ProductoListaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductoListaDto>>> ObtenerDestacados([FromQuery] int cantidad = 10)
        {
            try
            {
                var productos = await _productoService.ObtenerDestacadosAsync(cantidad);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos destacados");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener productos en oferta
        /// </summary>
        [HttpGet("ofertas")]
        [ProducesResponseType(typeof(List<ProductoListaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ProductoListaDto>>> ObtenerEnOferta([FromQuery] int cantidad = 10)
        {
            try
            {
                var productos = await _productoService.ObtenerEnOfertaAsync(cantidad);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener productos en oferta");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Crear un nuevo producto (solo administradores)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Crear([FromBody] CrearProductoDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var producto = await _productoService.CrearAsync(dto);
                _logger.LogInformation("Producto creado: {SKU}", producto.CodigoSKU);
                
                return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error al crear producto: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al crear producto");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Actualizar un producto (solo administradores)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductoDto>> Actualizar(int id, [FromBody] ActualizarProductoDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest(new { error = "El ID no coincide" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var producto = await _productoService.ActualizarAsync(dto);
                _logger.LogInformation("Producto actualizado: {Id}", id);
                
                return Ok(producto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Eliminar un producto (solo administradores)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var resultado = await _productoService.EliminarAsync(id);
                if (!resultado)
                    return NotFound(new { error = "Producto no encontrado" });

                _logger.LogInformation("Producto eliminado: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Cambiar estado de un producto (solo administradores)
        /// </summary>
        [HttpPatch("{id}/estado")]
        [Authorize(Roles = "Administrador,SuperAdministrador")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool estaActivo)
        {
            try
            {
                var resultado = await _productoService.CambiarEstadoAsync(id, estaActivo);
                if (!resultado)
                    return NotFound(new { error = "Producto no encontrado" });

                _logger.LogInformation("Estado de producto cambiado: {Id} -> {Estado}", id, estaActivo);
                return Ok(new { mensaje = "Estado actualizado", estaActivo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado del producto");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }
}
