namespace TiendaModerna.Application.DTOs.Orden
{
    /// <summary>
    /// DTO para detalle de orden (línea de producto en la orden)
    /// </summary>
    public class DetalleOrdenDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CodigoSKUProducto { get; set; } = string.Empty;
        public string? DetalleVariante { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal DescuentoUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string? UrlImagenProducto { get; set; }

        // Propiedades calculadas
        public decimal TotalSinDescuento => PrecioUnitario * Cantidad;
    }
}
