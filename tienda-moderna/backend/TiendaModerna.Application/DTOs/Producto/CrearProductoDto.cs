using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Application.DTOs.Producto
{
    /// <summary>
    /// DTO para crear un nuevo producto.
    /// Incluye validaciones de data annotations.
    /// </summary>
    public class CrearProductoDto
    {
        [Required(ErrorMessage = "El código SKU es obligatorio")]
        [StringLength(50, ErrorMessage = "El código SKU no puede exceder 50 caracteres")]
        public string CodigoSKU { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "La descripción no puede exceder 2000 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio base es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe estar entre 0.01 y 999999.99")]
        public decimal PrecioBase { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de descuento debe estar entre 0 y 100")]
        public decimal PorcentajeDescuento { get; set; } = 0;

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad de stock no puede ser negativa")]
        public int CantidadStock { get; set; } = 0;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public int CategoriaId { get; set; }

        public int? MarcaId { get; set; }

        public bool EstaActivo { get; set; } = true;
        public bool EsDestacado { get; set; } = false;
    }
}
