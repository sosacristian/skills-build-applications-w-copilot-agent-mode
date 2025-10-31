namespace TiendaModerna.Application.DTOs.Usuario
{
    /// <summary>
    /// DTO de usuario (sin información sensible)
    /// </summary>
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = string.Empty;
        public bool EstaActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimoInicioSesion { get; set; }

        // Dirección predeterminada
        public string? DireccionPredeterminada { get; set; }
        public string? CiudadPredeterminada { get; set; }
        public string? ProvinciaPredeterminada { get; set; }
        public string? CodigoPostalPredeterminado { get; set; }
    }
}
