using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Application.DTOs.Usuario
{
    /// <summary>
    /// DTO para login de usuario
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; } = string.Empty;
    }
}
