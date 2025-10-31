namespace TiendaModerna.Application.DTOs.Producto
{
    /// <summary>
    /// DTO simplificado para listados de productos.
    /// Contiene solo información esencial para mejorar el rendimiento.
    /// </summary>
    public class ProductoListaDto
    {
        public int Id { get; set; }
        public string CodigoSKU { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioBase { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal PrecioFinal { get; set; }
        public int CantidadStock { get; set; }
        public bool EsDestacado { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? MarcaNombre { get; set; }
        public string? ImagenPrincipalUrl { get; set; }
        public bool TieneStock => CantidadStock > 0;
    }
}
