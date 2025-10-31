using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una imagen asociada a un producto.
    /// Permite múltiples imágenes por producto con orden de visualización.
    /// </summary>
    public class Imagen
    {
        /// <summary>
        /// Identificador único de la imagen
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// URL de la imagen
        /// Puede ser ruta local o URL de CDN
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Texto alternativo para accesibilidad y SEO
        /// </summary>
        [StringLength(200)]
        public string? TextoAlternativo { get; set; }

        /// <summary>
        /// Orden de visualización en la galería
        /// La imagen con orden 1 es la principal
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Indica si esta es la imagen principal del producto
        /// </summary>
        public bool EsPrincipal { get; set; } = false;

        /// <summary>
        /// Fecha de subida de la imagen
        /// </summary>
        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// ID del producto al que pertenece la imagen
        /// </summary>
        [Required]
        public int ProductoId { get; set; }

        /// <summary>
        /// Navegación hacia el producto
        /// </summary>
        public virtual Producto? Producto { get; set; }
    }
}
