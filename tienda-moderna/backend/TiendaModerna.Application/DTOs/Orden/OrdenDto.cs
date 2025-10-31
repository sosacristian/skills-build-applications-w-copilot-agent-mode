namespace TiendaModerna.Application.DTOs.Orden
{
    /// <summary>
    /// DTO completo de orden
    /// </summary>
    public class OrdenDto
    {
        public int Id { get; set; }
        public string NumeroOrden { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal CostoEnvio { get; set; }
        public decimal Total { get; set; }
        public string? CodigoCupon { get; set; }
        public string? NotasCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }

        // Datos de envío
        public string NombreDestinatario { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;
        public string? DireccionEnvio2 { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string TelefonoContacto { get; set; } = string.Empty;

        // Pago y envío
        public string? MetodoPago { get; set; }
        public string? IdTransaccionPago { get; set; }
        public string? EmpresaTransporte { get; set; }
        public string? CodigoSeguimiento { get; set; }

        // Usuario
        public int UsuarioId { get; set; }
        public string? UsuarioNombreCompleto { get; set; }
        public string? UsuarioEmail { get; set; }

        // Detalles de la orden
        public List<DetalleOrdenDto>? Detalles { get; set; }

        // Propiedades calculadas
        public int CantidadProductos => Detalles?.Sum(d => d.Cantidad) ?? 0;
        public bool EstaPaga => FechaPago.HasValue;
        public bool EstaEnviada => FechaEnvio.HasValue;
        public bool EstaEntregada => FechaEntrega.HasValue;
    }
}
