using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una categoría de productos.
    /// Permite organización jerárquica mediante categorías padre-hijo.
    /// Ejemplo: Ropa > Mujer > Vestidos
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// Identificador único de la categoría
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría
        /// Debe ser único dentro del mismo nivel jerárquico
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción de la categoría
        /// Utilizada para SEO y contexto al usuario
        /// </summary>
        [StringLength(500)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Slug para URL amigable
        /// Ejemplo: "vestidos-mujer" en lugar de "Vestidos de Mujer"
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Slug { get; set; } = string.Empty;

        /// <summary>
        /// URL de la imagen representativa de la categoría
        /// </summary>
        [StringLength(500)]
        public string? UrlImagen { get; set; }

        /// <summary>
        /// Orden de visualización en el sitio
        /// Permite controlar el orden en menús y listados
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Indica si la categoría está activa y visible
        /// </summary>
        public bool EstaActiva { get; set; } = true;

        /// <summary>
        /// Fecha de creación del registro
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // ============ PROPIEDADES DE JERARQUÍA ============

        /// <summary>
        /// ID de la categoría padre (null si es categoría raíz)
        /// Permite construir árboles de categorías
        /// </summary>
        public int? CategoriaPadreId { get; set; }

        /// <summary>
        /// Navegación hacia la categoría padre
        /// </summary>
        public virtual Categoria? CategoriaPadre { get; set; }

        /// <summary>
        /// Colección de subcategorías (hijos)
        /// Relación One-to-Many recursiva
        /// </summary>
        public virtual ICollection<Categoria> SubCategorias { get; set; } = new List<Categoria>();

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// Colección de productos en esta categoría
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Verifica si es una categoría raíz (sin padre)
        /// </summary>
        public bool EsRaiz() => CategoriaPadreId == null;

        /// <summary>
        /// Verifica si tiene subcategorías
        /// </summary>
        public bool TieneSubCategorias() => SubCategorias?.Any() ?? false;

        /// <summary>
        /// Obtiene la ruta completa de la categoría
        /// Ejemplo: "Ropa / Mujer / Vestidos"
        /// </summary>
        public string ObtenerRutaCompleta()
        {
            if (CategoriaPadre == null)
                return Nombre;

            return $"{CategoriaPadre.ObtenerRutaCompleta()} / {Nombre}";
        }

        /// <summary>
        /// Genera un slug a partir del nombre
        /// Convierte "Vestidos de Mujer" en "vestidos-de-mujer"
        /// </summary>
        public void GenerarSlug()
        {
            Slug = Nombre
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n");
        }
    }
}
