namespace TiendaModerna.Application.DTOs.Producto
{
    /// <summary>
    /// DTO para mostrar información completa de un producto.
    /// Usado en vistas de detalle y listados.
    /// </summary>
    public class ProductoDto
    {
        public int Id { get; set; }
        public string CodigoSKU { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PrecioFinal { get; set; }
        public int CantidadStock { get; set; }
        public bool EstaActivo { get; set; }
        public bool EsDestacado { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Datos de navegación
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public int? MarcaId { get; set; }
        public string? MarcaNombre { get; set; }

        // Colecciones relacionadas
        public List<VarianteDto>? Variantes { get; set; }
        public List<ImagenDto>? Imagenes { get; set; }

        // Propiedades calculadas
        public bool TieneStock => CantidadStock > 0;
        public bool TieneDescuento => PorcentajeDescuento > 0;
    }
}
