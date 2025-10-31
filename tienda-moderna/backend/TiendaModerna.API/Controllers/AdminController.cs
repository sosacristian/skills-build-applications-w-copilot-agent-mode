using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using TiendaModerna.Application.DTOs.Producto;
using TiendaModerna.Application.Interfaces;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.API.Controllers
{
    /// <summary>
    /// Controlador para operaciones administrativas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")] // Solo administradores
    public class AdminController : ControllerBase
    {
        private readonly TiendaContext _context;
        private readonly IProductoService _productoService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            TiendaContext context,
            IProductoService productoService,
            ILogger<AdminController> logger)
        {
            _context = context;
            _productoService = productoService;
            _logger = logger;
        }

        /// <summary>
        /// Importar productos desde archivo Excel
        /// </summary>
        /// <param name="archivo">Archivo Excel (.xlsx) con productos</param>
        /// <returns>Resultado de la importación</returns>
        [HttpPost("productos/importar")]
        [ProducesResponseType(typeof(ResultadoImportacionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResultadoImportacionDto>> ImportarProductos(IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { error = "No se recibió ningún archivo" });

            if (!archivo.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { error = "El archivo debe ser formato Excel (.xlsx)" });

            var resultado = new ResultadoImportacionDto();

            try
            {
                // Configurar licencia de EPPlus (modo no comercial)
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var stream = new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Primera hoja
                        var rowCount = worksheet.Dimension?.Rows ?? 0;

                        if (rowCount < 2) // Debe tener al menos encabezado + 1 fila
                            return BadRequest(new { error = "El archivo no contiene datos" });

                        // Leer desde fila 2 (la 1 es encabezado)
                        for (int row = 2; row <= rowCount; row++)
                        {
                            resultado.TotalProcesados++;

                            try
                            {
                                // Leer datos de la fila
                                var codigoSKU = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                                var nombre = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                                var descripcion = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                                var precioBaseStr = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                                var porcentajeDescuentoStr = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? "0";
                                var stockStr = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                                var nombreCategoria = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
                                var nombreMarca = worksheet.Cells[row, 8].Value?.ToString()?.Trim();
                                var esDestacadoStr = worksheet.Cells[row, 9].Value?.ToString()?.Trim() ?? "no";

                                // Validaciones básicas
                                if (string.IsNullOrEmpty(codigoSKU))
                                {
                                    resultado.Errores.Add(new ErrorImportacionDto
                                    {
                                        Fila = row,
                                        Campo = "CodigoSKU",
                                        Mensaje = "El código SKU es obligatorio",
                                        Valor = codigoSKU ?? ""
                                    });
                                    resultado.Fallidos++;
                                    continue;
                                }

                                if (string.IsNullOrEmpty(nombre))
                                {
                                    resultado.Errores.Add(new ErrorImportacionDto
                                    {
                                        Fila = row,
                                        Campo = "Nombre",
                                        Mensaje = "El nombre es obligatorio",
                                        Valor = nombre ?? ""
                                    });
                                    resultado.Fallidos++;
                                    continue;
                                }

                                // Parsear valores numéricos
                                if (!decimal.TryParse(precioBaseStr, out decimal precioBase))
                                {
                                    resultado.Errores.Add(new ErrorImportacionDto
                                    {
                                        Fila = row,
                                        Campo = "PrecioBase",
                                        Mensaje = "El precio base debe ser un número válido",
                                        Valor = precioBaseStr ?? ""
                                    });
                                    resultado.Fallidos++;
                                    continue;
                                }

                                if (!decimal.TryParse(porcentajeDescuentoStr, out decimal porcentajeDescuento))
                                    porcentajeDescuento = 0;

                                if (!int.TryParse(stockStr, out int stock))
                                {
                                    resultado.Errores.Add(new ErrorImportacionDto
                                    {
                                        Fila = row,
                                        Campo = "Stock",
                                        Mensaje = "El stock debe ser un número entero válido",
                                        Valor = stockStr ?? ""
                                    });
                                    resultado.Fallidos++;
                                    continue;
                                }

                                // Buscar o crear categoría
                                Categoria? categoria = null;
                                if (!string.IsNullOrEmpty(nombreCategoria))
                                {
                                    var nombreCategoriaLower = nombreCategoria.ToLower();
                                    categoria = _context.Categorias
                                        .FirstOrDefault(c => c.Nombre.ToLower() == nombreCategoriaLower);

                                    if (categoria == null)
                                    {
                                        categoria = new Categoria
                                        {
                                            Nombre = nombreCategoria,
                                            Slug = nombreCategoria.ToLower().Replace(" ", "-"),
                                            EstaActiva = true,
                                            FechaCreacion = DateTime.UtcNow
                                        };
                                        _context.Categorias.Add(categoria);
                                        await _context.SaveChangesAsync(); // Guardar para obtener el ID
                                    }
                                }

                                if (categoria == null)
                                {
                                    resultado.Errores.Add(new ErrorImportacionDto
                                    {
                                        Fila = row,
                                        Campo = "Categoria",
                                        Mensaje = "No se pudo encontrar o crear la categoría",
                                        Valor = nombreCategoria ?? ""
                                    });
                                    resultado.Fallidos++;
                                    continue;
                                }

                                // Buscar o crear marca
                                Marca? marca = null;
                                if (!string.IsNullOrEmpty(nombreMarca))
                                {
                                    var nombreMarcaLower = nombreMarca.ToLower();
                                    marca = _context.Marcas
                                        .FirstOrDefault(m => m.Nombre.ToLower() == nombreMarcaLower);

                                    if (marca == null)
                                    {
                                        marca = new Marca
                                        {
                                            Nombre = nombreMarca,
                                            EstaActiva = true,
                                            FechaCreacion = DateTime.UtcNow
                                        };
                                        _context.Marcas.Add(marca);
                                        await _context.SaveChangesAsync(); // Guardar para obtener el ID
                                    }
                                }

                                if (marca == null)
                                {
                                    resultado.Errores.Add(new ErrorImportacionDto
                                    {
                                        Fila = row,
                                        Campo = "Marca",
                                        Mensaje = "No se pudo encontrar o crear la marca",
                                        Valor = nombreMarca ?? ""
                                    });
                                    resultado.Fallidos++;
                                    continue;
                                }

                                // Verificar si el producto ya existe
                                var productoExistente = _context.Productos
                                    .FirstOrDefault(p => p.CodigoSKU == codigoSKU);

                                if (productoExistente != null)
                                {
                                    // Actualizar producto existente
                                    productoExistente.Nombre = nombre;
                                    productoExistente.Descripcion = descripcion;
                                    productoExistente.PrecioBase = precioBase;
                                    productoExistente.PorcentajeDescuento = porcentajeDescuento;
                                    productoExistente.CantidadStock = stock;
                                    productoExistente.CategoriaId = categoria.Id;
                                    productoExistente.MarcaId = marca.Id;
                                    productoExistente.EsDestacado = esDestacadoStr?.ToLower() == "si" || esDestacadoStr?.ToLower() == "yes";
                                    productoExistente.FechaActualizacion = DateTime.UtcNow;
                                }
                                else
                                {
                                    // Crear nuevo producto
                                    var nuevoProducto = new Producto
                                    {
                                        CodigoSKU = codigoSKU,
                                        Nombre = nombre,
                                        Descripcion = descripcion,
                                        PrecioBase = precioBase,
                                        PorcentajeDescuento = porcentajeDescuento,
                                        CantidadStock = stock,
                                        CategoriaId = categoria.Id,
                                        MarcaId = marca.Id,
                                        EstaActivo = true,
                                        EsDestacado = esDestacadoStr?.ToLower() == "si" || esDestacadoStr?.ToLower() == "yes",
                                        FechaCreacion = DateTime.UtcNow
                                    };
                                    _context.Productos.Add(nuevoProducto);
                                }

                                await _context.SaveChangesAsync();
                                resultado.Exitosos++;
                            }
                            catch (Exception ex)
                            {
                                resultado.Errores.Add(new ErrorImportacionDto
                                {
                                    Fila = row,
                                    Campo = "General",
                                    Mensaje = ex.Message,
                                    Valor = ""
                                });
                                resultado.Fallidos++;
                                _logger.LogError(ex, "Error al procesar fila {Fila}", row);
                            }
                        }
                    }
                }

                _logger.LogInformation(
                    "Importación completada: {Total} procesados, {Exitosos} exitosos, {Fallidos} fallidos",
                    resultado.TotalProcesados, resultado.Exitosos, resultado.Fallidos);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al importar productos");
                return StatusCode(500, new { error = "Error al procesar el archivo: " + ex.Message });
            }
        }

        /// <summary>
        /// Obtener plantilla Excel de ejemplo
        /// </summary>
        [HttpGet("productos/plantilla")]
        [AllowAnonymous] // Permitir descarga sin autenticación
        public IActionResult DescargarPlantilla()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Productos");

                    // Encabezados
                    worksheet.Cells[1, 1].Value = "CodigoSKU";
                    worksheet.Cells[1, 2].Value = "Nombre";
                    worksheet.Cells[1, 3].Value = "Descripcion";
                    worksheet.Cells[1, 4].Value = "PrecioBase";
                    worksheet.Cells[1, 5].Value = "PorcentajeDescuento";
                    worksheet.Cells[1, 6].Value = "Stock";
                    worksheet.Cells[1, 7].Value = "Categoria";
                    worksheet.Cells[1, 8].Value = "Marca";
                    worksheet.Cells[1, 9].Value = "Destacado";

                    // Formato de encabezados
                    using (var range = worksheet.Cells[1, 1, 1, 9])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    }

                    // Ejemplo de fila
                    worksheet.Cells[2, 1].Value = "PROD-001";
                    worksheet.Cells[2, 2].Value = "Producto Ejemplo";
                    worksheet.Cells[2, 3].Value = "Descripción del producto";
                    worksheet.Cells[2, 4].Value = 10000;
                    worksheet.Cells[2, 5].Value = 10;
                    worksheet.Cells[2, 6].Value = 50;
                    worksheet.Cells[2, 7].Value = "Remeras";
                    worksheet.Cells[2, 8].Value = "Nike";
                    worksheet.Cells[2, 9].Value = "Si";

                    worksheet.Cells.AutoFitColumns();

                    var stream = new MemoryStream(package.GetAsByteArray());
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "plantilla_productos.xlsx");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar plantilla");
                return StatusCode(500, new { error = "Error al generar plantilla" });
            }
        }
    }
}
