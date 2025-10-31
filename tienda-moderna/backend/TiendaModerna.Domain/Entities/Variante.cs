using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una variante de un producto.
    /// Permite gestionar diferentes combinaciones de talla, color, etc.
    /// Ejemplo: "Remera Algodón" puede tener variantes (S, Rojo), (M, Azul), etc.
    /// </summary>
    public class Variante
    {
        /// <summary>
        /// Identificador único de la variante
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// SKU específico de esta variante
        /// Ejemplo: Si el producto es "REM-001", las variantes pueden ser "REM-001-S-R", "REM-001-M-A"
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CodigoSKU { get; set; } = string.Empty;

        /// <summary>
        /// Talla de la variante (S, M, L, XL, etc.)
        /// </summary>
        [StringLength(20)]
        public string? Talla { get; set; }

        /// <summary>
        /// Color de la variante
        /// </summary>
        [StringLength(50)]
        public string? Color { get; set; }

        /// <summary>
        /// Material del producto en esta variante (algodón, poliéster, etc.)
        /// </summary>
        [StringLength(100)]
        public string? Material { get; set; }

        /// <summary>
        /// Ajuste adicional al precio base del producto
        /// Puede ser positivo (variante más cara) o negativo (más barata)
        /// Ejemplo: Tallas especiales pueden tener un ajuste de +500
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal AjustePrecio { get; set; } = 0;

        /// <summary>
        /// Stock específico de esta variante
        /// </summary>
        [Required]
        public int CantidadStock { get; set; } = 0;

        /// <summary>
        /// Indica si esta variante está disponible para la venta
        /// </summary>
        public bool EstaDisponible { get; set; } = true;

        /// <summary>
        /// URL de imagen específica de esta variante
        /// Si es null, usa las imágenes del producto principal
        /// </summary>
        [StringLength(500)]
        public string? UrlImagen { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// ID del producto padre
        /// </summary>
        [Required]
        public int ProductoId { get; set; }

        /// <summary>
        /// Navegación hacia el producto padre
        /// </summary>
        public virtual Producto? Producto { get; set; }

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Calcula el precio final de la variante
        /// Toma el precio del producto y le suma el ajuste
        /// </summary>
        public decimal CalcularPrecioFinal(decimal precioBaseProducto)
        {
            return precioBaseProducto + AjustePrecio;
        }

        /// <summary>
        /// Verifica si la variante tiene stock disponible
        /// </summary>
        public bool TieneStock() => CantidadStock > 0 && EstaDisponible;

        /// <summary>
        /// Genera una descripción legible de la variante
        /// Ejemplo: "Talla M - Color Rojo - Material Algodón"
        /// </summary>
        public string ObtenerDescripcion()
        {
            var partes = new List<string>();

            if (!string.IsNullOrEmpty(Talla))
                partes.Add($"Talla {Talla}");

            if (!string.IsNullOrEmpty(Color))
                partes.Add($"Color {Color}");

            if (!string.IsNullOrEmpty(Material))
                partes.Add($"Material {Material}");

            return string.Join(" - ", partes);
        }
    }
}
