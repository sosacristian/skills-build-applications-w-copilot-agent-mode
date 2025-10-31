namespace TiendaModerna.Application.DTOs.Usuario
{
    /// <summary>
    /// DTO de respuesta de autenticación con token JWT
    /// </summary>
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
        public UsuarioDto Usuario { get; set; } = null!;
    }
}
