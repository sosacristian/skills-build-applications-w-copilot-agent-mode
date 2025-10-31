using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un producto en el catálogo de la tienda.
    /// Principio SRP: Esta clase solo tiene la responsabilidad de representar un producto.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto (Primary Key)
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Código SKU único del producto (Stock Keeping Unit)
        /// Utilizado para identificar el producto en el inventario
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CodigoSKU { get; set; } = string.Empty;

        /// <summary>
        /// Nombre comercial del producto
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto
        /// Puede incluir características, materiales, cuidados, etc.
        /// </summary>
        [StringLength(2000)]
        public string? Descripcion { get; set; }

        /// <summary>
        /// Precio base del producto antes de descuentos
        /// Utiliza decimal para precisión en cálculos monetarios
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioBase { get; set; }

        /// <summary>
        /// Porcentaje de descuento aplicable (0-100)
        /// </summary>
        [Range(0, 100)]
        public decimal PorcentajeDescuento { get; set; } = 0;

        /// <summary>
        /// Precio final calculado después de aplicar el descuento
        /// Propiedad calculada - no se almacena en BD
        /// </summary>
        [NotMapped]
        public decimal PrecioFinal => PrecioBase * (1 - PorcentajeDescuento / 100);

        /// <summary>
        /// Cantidad disponible en inventario
        /// </summary>
        [Required]
        public int CantidadStock { get; set; }

        /// <summary>
        /// Indica si el producto está visible en el catálogo
        /// Permite ocultar productos sin eliminarlos (Soft Delete alternativo)
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Indica si el producto está marcado como destacado
        /// Útil para mostrar en secciones especiales del sitio
        /// </summary>
        public bool EsDestacado { get; set; } = false;

        /// <summary>
        /// Fecha de creación del registro
        /// Se establece automáticamente al crear el producto
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización del registro
        /// Se actualiza automáticamente en cada modificación
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN (Relaciones) ============

        /// <summary>
        /// ID de la categoría a la que pertenece el producto
        /// Foreign Key hacia Categoria
        /// </summary>
        [Required]
        public int CategoriaId { get; set; }

        /// <summary>
        /// Navegación hacia la categoría del producto
        /// Relación Many-to-One (muchos productos pueden tener una categoría)
        /// </summary>
        public virtual Categoria? Categoria { get; set; }

        /// <summary>
        /// ID de la marca del producto (opcional)
        /// Permite productos sin marca definida
        /// </summary>
        public int? MarcaId { get; set; }

        /// <summary>
        /// Navegación hacia la marca del producto
        /// </summary>
        public virtual Marca? Marca { get; set; }

        /// <summary>
        /// Colección de variantes del producto (tallas, colores, etc.)
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<Variante> Variantes { get; set; } = new List<Variante>();

        /// <summary>
        /// Colección de imágenes asociadas al producto
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<Imagen> Imagenes { get; set; } = new List<Imagen>();

        /// <summary>
        /// Colección de detalles de órdenes que incluyen este producto
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<DetalleOrden> DetallesOrdenes { get; set; } = new List<DetalleOrden>();

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Verifica si el producto tiene stock disponible
        /// </summary>
        public bool TieneStock() => CantidadStock > 0;

        /// <summary>
        /// Verifica si hay stock suficiente para una cantidad solicitada
        /// </summary>
        /// <param name="cantidad">Cantidad solicitada</param>
        /// <returns>True si hay stock suficiente</returns>
        public bool TieneStockSuficiente(int cantidad) => CantidadStock >= cantidad;

        /// <summary>
        /// Reduce el stock del producto
        /// </summary>
        /// <param name="cantidad">Cantidad a reducir</param>
        /// <exception cref="InvalidOperationException">Si no hay stock suficiente</exception>
        public void ReducirStock(int cantidad)
        {
            if (!TieneStockSuficiente(cantidad))
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {CantidadStock}, Solicitado: {cantidad}");

            CantidadStock -= cantidad;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Incrementa el stock del producto
        /// </summary>
        /// <param name="cantidad">Cantidad a agregar</param>
        public void IncrementarStock(int cantidad)
        {
            CantidadStock += cantidad;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Aplica un descuento al producto
        /// </summary>
        /// <param name="porcentaje">Porcentaje de descuento (0-100)</param>
        public void AplicarDescuento(decimal porcentaje)
        {
            if (porcentaje < 0 || porcentaje > 100)
                throw new ArgumentException("El porcentaje debe estar entre 0 y 100");

            PorcentajeDescuento = porcentaje;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Marca el producto como actualizado
        /// Útil para triggers de auditoría
        /// </summary>
        public void MarcarComoActualizado()
        {
            FechaActualizacion = DateTime.UtcNow;
        }
    }
}
