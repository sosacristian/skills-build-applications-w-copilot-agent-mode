# 🎯 Domain Layer - Código Completo

## 📁 Entities/Producto.cs

```csharp
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
```

---

## 📁 Entities/Categoria.cs

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
```

---

## 📁 Entities/Variante.cs

```csharp
using System;
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
```

---

## 📁 Entities/Imagen.cs

```csharp
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
```

---

## 📁 Entities/Marca.cs

```csharp
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
```

Continúa en el próximo archivo con Orden, DetalleOrden y Usuario...
