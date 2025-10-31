using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una orden de compra.
    /// Gestiona el ciclo completo: carrito → orden → pago → envío.
    /// Principio SRP: Responsable únicamente de representar una orden.
    /// </summary>
    public class Orden
    {
        /// <summary>
        /// Identificador único de la orden
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Número de orden único y legible
        /// Formato sugerido: ORD-20240101-0001
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NumeroOrden { get; set; } = string.Empty;

        /// <summary>
        /// Estado actual de la orden
        /// </summary>
        [Required]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        /// <summary>
        /// Subtotal (suma de productos sin descuentos ni envío)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Total de descuentos aplicados
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDescuentos { get; set; } = 0;

        /// <summary>
        /// Costo de envío
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoEnvio { get; set; } = 0;

        /// <summary>
        /// Total final de la orden
        /// = Subtotal - TotalDescuentos + CostoEnvio
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        /// <summary>
        /// Código de cupón aplicado (si existe)
        /// </summary>
        [StringLength(50)]
        public string? CodigoCupon { get; set; }

        /// <summary>
        /// Notas adicionales del cliente
        /// </summary>
        [StringLength(500)]
        public string? NotasCliente { get; set; }

        /// <summary>
        /// Fecha de creación de la orden
        /// </summary>
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>
        /// Fecha en que se completó el pago
        /// </summary>
        public DateTime? FechaPago { get; set; }

        /// <summary>
        /// Fecha en que se envió la orden
        /// </summary>
        public DateTime? FechaEnvio { get; set; }

        /// <summary>
        /// Fecha en que se entregó la orden
        /// </summary>
        public DateTime? FechaEntrega { get; set; }

        // ============ INFORMACIÓN DE ENVÍO ============

        /// <summary>
        /// Nombre completo de la persona que recibe
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreDestinatario { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de envío - Línea 1
        /// </summary>
        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de envío - Línea 2 (piso, depto, etc.)
        /// </summary>
        [StringLength(200)]
        public string? DireccionEnvio2 { get; set; }

        /// <summary>
        /// Ciudad de envío
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; } = string.Empty;

        /// <summary>
        /// Provincia/Estado
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Provincia { get; set; } = string.Empty;

        /// <summary>
        /// Código postal
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CodigoPostal { get; set; } = string.Empty;

        /// <summary>
        /// País
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Pais { get; set; } = "Argentina";

        /// <summary>
        /// Teléfono de contacto para la entrega
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TelefonoContacto { get; set; } = string.Empty;

        // ============ INFORMACIÓN DE PAGO ============

        /// <summary>
        /// Método de pago utilizado
        /// Ejemplo: "Tarjeta de Crédito", "Mercado Pago", "Transferencia"
        /// </summary>
        [StringLength(100)]
        public string? MetodoPago { get; set; }

        /// <summary>
        /// ID de la transacción en la pasarela de pago
        /// </summary>
        [StringLength(200)]
        public string? IdTransaccionPago { get; set; }

        // ============ SEGUIMIENTO DE ENVÍO ============

        /// <summary>
        /// Empresa de transporte/courier
        /// </summary>
        [StringLength(100)]
        public string? EmpresaTransporte { get; set; }

        /// <summary>
        /// Código de seguimiento del envío
        /// </summary>
        [StringLength(100)]
        public string? CodigoSeguimiento { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// ID del usuario que realizó la orden
        /// </summary>
        [Required]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Navegación hacia el usuario
        /// </summary>
        public virtual Usuario? Usuario { get; set; }

        /// <summary>
        /// Colección de detalles (productos) de la orden
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Calcula el total de la orden
        /// </summary>
        public void CalcularTotal()
        {
            Total = Subtotal - TotalDescuentos + CostoEnvio;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Genera un número de orden único
        /// Formato: ORD-YYYYMMDD-####
        /// </summary>
        public static string GenerarNumeroOrden(int contadorDiario)
        {
            var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
            return $"ORD-{fecha}-{contadorDiario:D4}";
        }

        /// <summary>
        /// Cambia el estado de la orden
        /// </summary>
        public void CambiarEstado(EstadoOrden nuevoEstado)
        {
            Estado = nuevoEstado;
            FechaActualizacion = DateTime.UtcNow;

            // Actualizar fechas según el estado
            switch (nuevoEstado)
            {
                case EstadoOrden.Pagada:
                    FechaPago = DateTime.UtcNow;
                    break;
                case EstadoOrden.Enviada:
                    FechaEnvio = DateTime.UtcNow;
                    break;
                case EstadoOrden.Entregada:
                    FechaEntrega = DateTime.UtcNow;
                    break;
            }
        }

        /// <summary>
        /// Verifica si la orden puede ser cancelada
        /// </summary>
        public bool PuedeCancelarse()
        {
            return Estado == EstadoOrden.Pendiente || Estado == EstadoOrden.Pagada;
        }

        /// <summary>
        /// Obtiene la dirección completa de envío
        /// </summary>
        public string ObtenerDireccionCompleta()
        {
            var direccion = DireccionEnvio;
            if (!string.IsNullOrEmpty(DireccionEnvio2))
                direccion += $", {DireccionEnvio2}";

            return $"{direccion}, {Ciudad}, {Provincia} ({CodigoPostal}), {Pais}";
        }

        /// <summary>
        /// Obtiene la cantidad total de productos en la orden
        /// </summary>
        public int ObtenerCantidadTotalProductos()
        {
            return Detalles?.Sum(d => d.Cantidad) ?? 0;
        }
    }
}
