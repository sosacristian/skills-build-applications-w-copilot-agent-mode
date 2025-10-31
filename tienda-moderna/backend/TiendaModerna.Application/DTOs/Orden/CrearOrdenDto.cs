using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Application.DTOs.Orden
{
    /// <summary>
    /// DTO para crear una nueva orden
    /// </summary>
    public class CrearOrdenDto
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un producto")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
        public List<CrearDetalleOrdenDto> Detalles { get; set; } = new();

        public string? CodigoCupon { get; set; }
        public string? NotasCliente { get; set; }

        // Datos de envío
        [Required(ErrorMessage = "El nombre del destinatario es obligatorio")]
        [StringLength(200)]
        public string NombreDestinatario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección de envío es obligatoria")]
        [StringLength(500)]
        public string DireccionEnvio { get; set; } = string.Empty;

        [StringLength(500)]
        public string? DireccionEnvio2 { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        [StringLength(100)]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        [StringLength(100)]
        public string Provincia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [StringLength(20)]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required(ErrorMessage = "El país es obligatorio")]
        [StringLength(100)]
        public string Pais { get; set; } = "Argentina";

        [Required(ErrorMessage = "El teléfono de contacto es obligatorio")]
        [StringLength(20)]
        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        public string TelefonoContacto { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MetodoPago { get; set; }
    }

    public class CrearDetalleOrdenDto
    {
        [Required]
        public int ProductoId { get; set; }

        public int? VarianteId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1")]
        public int Cantidad { get; set; }
    }
}
