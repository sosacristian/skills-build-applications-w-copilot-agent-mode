using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un item/línea dentro de una orden.
    /// Cada DetalleOrden es un producto específico con su cantidad y precio.
    /// </summary>
    public class DetalleOrden
    {
        /// <summary>
        /// Identificador único del detalle
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Cantidad del producto ordenado
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio unitario en el momento de la compra
        /// Se guarda porque el precio del producto puede cambiar después
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Descuento unitario aplicado en el momento de la compra
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal DescuentoUnitario { get; set; } = 0;

        /// <summary>
        /// Subtotal de esta línea (Cantidad * PrecioUnitario)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Total de esta línea después de descuentos
        /// = (Cantidad * PrecioUnitario) - (Cantidad * DescuentoUnitario)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        /// <summary>
        /// Información de la variante seleccionada (talla, color, etc.)
        /// Se guarda como texto porque la variante puede ser eliminada después
        /// </summary>
        [StringLength(200)]
        public string? DetalleVariante { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// ID de la orden a la que pertenece este detalle
        /// </summary>
        [Required]
        public int OrdenId { get; set; }

        /// <summary>
        /// Navegación hacia la orden
        /// </summary>
        public virtual Orden? Orden { get; set; }

        /// <summary>
        /// ID del producto ordenado
        /// </summary>
        [Required]
        public int ProductoId { get; set; }

        /// <summary>
        /// Navegación hacia el producto
        /// </summary>
        public virtual Producto? Producto { get; set; }

        /// <summary>
        /// ID de la variante específica (si aplica)
        /// </summary>
        public int? VarianteId { get; set; }

        /// <summary>
        /// Navegación hacia la variante
        /// </summary>
        public virtual Variante? Variante { get; set; }

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Calcula el subtotal y total de la línea
        /// </summary>
        public void CalcularTotales()
        {
            Subtotal = Cantidad * PrecioUnitario;
            Total = Subtotal - (Cantidad * DescuentoUnitario);
        }

        /// <summary>
        /// Obtiene el precio final unitario (después de descuento)
        /// </summary>
        public decimal ObtenerPrecioFinalUnitario()
        {
            return PrecioUnitario - DescuentoUnitario;
        }
    }
}
