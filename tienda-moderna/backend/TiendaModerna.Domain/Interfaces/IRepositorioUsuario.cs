using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz específica para el repositorio de Usuarios.
    /// </summary>
    public interface IRepositorioUsuario : IRepositorioGenerico<Usuario>
    {
        /// <summary>
        /// Obtiene un usuario por su email
        /// Usado principalmente para autenticación
        /// </summary>
        /// <param name="email">Email del usuario</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<Usuario?> ObtenerPorEmailAsync(string email);

        /// <summary>
        /// Verifica si un email ya está registrado
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <param name="idExcluir">ID de usuario a excluir (para actualizaciones)</param>
        /// <returns>True si el email ya existe</returns>
        Task<bool> EmailExisteAsync(string email, int? idExcluir = null);

        /// <summary>
        /// Obtiene un usuario por su token de verificación de email
        /// </summary>
        /// <param name="token">Token de verificación</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<Usuario?> ObtenerPorTokenVerificacionAsync(string token);

        /// <summary>
        /// Obtiene un usuario por su token de recuperación de contraseña
        /// </summary>
        /// <param name="token">Token de recuperación</param>
        /// <returns>Usuario encontrado o null</returns>
        Task<Usuario?> ObtenerPorTokenRecuperacionAsync(string token);

        /// <summary>
        /// Obtiene todos los administradores del sistema
        /// </summary>
        /// <returns>Colección de usuarios administradores</returns>
        Task<IEnumerable<Usuario>> ObtenerAdministradoresAsync();

        /// <summary>
        /// Obtiene los clientes más activos (por cantidad de órdenes)
        /// </summary>
        /// <param name="cantidad">Cantidad de clientes a retornar</param>
        /// <returns>Colección de clientes top</returns>
        Task<IEnumerable<Usuario>> ObtenerClientesTopAsync(int cantidad = 10);
    }
}
