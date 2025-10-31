using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Application.DTOs.Usuario
{
    /// <summary>
    /// DTO para registro de nuevo usuario
    /// </summary>
    public class RegistrarUsuarioDto
    {
        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
        [StringLength(20)]
        public string? Telefono { get; set; }
    }
}
