using TiendaModerna.Application.DTOs.Usuario;

namespace TiendaModerna.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de usuarios y autenticación
    /// </summary>
    public interface IUsuarioService
    {
        Task<UsuarioDto?> ObtenerPorIdAsync(int id);
        Task<UsuarioDto?> ObtenerPorEmailAsync(string email);
        Task<AuthResponseDto> RegistrarAsync(RegistrarUsuarioDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNueva);
        Task<bool> SolicitarRecuperacionPasswordAsync(string email);
        Task<bool> RestablecerPasswordAsync(string token, string passwordNueva);
        Task<bool> VerificarEmailAsync(string token);
        Task<bool> EmailExisteAsync(string email);
    }
}
