using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una marca de productos.
    /// Permite filtrar y agrupar productos por fabricante/diseñador.
    /// </summary>
    public class Marca
    {
        /// <summary>
        /// Identificador único de la marca
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la marca
        /// Ejemplo: Nike, Adidas, Zara
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción de la marca
        /// Historia, valores, etc.
        /// </summary>
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// URL del logotipo de la marca
        /// </summary>
        [StringLength(500)]
        public string? UrlLogo { get; set; }

        /// <summary>
        /// Sitio web oficial de la marca
        /// </summary>
        [StringLength(200)]
        public string? SitioWeb { get; set; }

        /// <summary>
        /// Indica si la marca está activa
        /// </summary>
        public bool EstaActiva { get; set; } = true;

        /// <summary>
        /// Fecha de creación
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// Colección de productos de esta marca
        /// </summary>
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
