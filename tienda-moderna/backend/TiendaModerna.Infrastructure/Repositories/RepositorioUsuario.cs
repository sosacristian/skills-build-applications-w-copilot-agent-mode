using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Enums;
using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio específico para Usuarios.
    /// 
    /// Maneja autenticación, recuperación de contraseñas y gestión de usuarios.
    /// </summary>
    public class RepositorioUsuario : RepositorioGenerico<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(TiendaContext contexto) : base(contexto)
        {
        }

        // ============ Métodos Especializados ============

        /// <summary>
        /// Obtiene un usuario por email.
        /// </summary>
        public async Task<Usuario?> ObtenerPorEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Verifica si un email ya está registrado.
        /// </summary>
        public async Task<bool> EmailExisteAsync(string email, int? idExcluir = null)
        {
            var query = _dbSet.Where(u => u.Email == email);

            if (idExcluir.HasValue)
            {
                query = query.Where(u => u.Id != idExcluir.Value);
            }

            return await query.AnyAsync();
        }

        /// <summary>
        /// Obtiene un usuario por token de verificación.
        /// </summary>
        public async Task<Usuario?> ObtenerPorTokenVerificacionAsync(string token)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.TokenVerificacionEmail == token &&
                                                        u.TokenVerificacionExpiracion > DateTime.UtcNow);
        }

        /// <summary>
        /// Obtiene un usuario por token de recuperación.
        /// </summary>
        public async Task<Usuario?> ObtenerPorTokenRecuperacionAsync(string token)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.TokenRecuperacion == token &&
                                                        u.TokenRecuperacionExpiracion > DateTime.UtcNow);
        }

        /// <summary>
        /// Obtiene todos los administradores del sistema.
        /// </summary>
        public async Task<IEnumerable<Usuario>> ObtenerAdministradoresAsync()
        {
            return await _dbSet
                .Where(u => u.Rol == RolUsuario.Administrador && u.EstaActivo)
                .OrderBy(u => u.NombreCompleto)
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los clientes más activos (por cantidad de órdenes).
        /// </summary>
        public async Task<IEnumerable<Usuario>> ObtenerClientesTopAsync(int cantidad = 10)
        {
            return await _dbSet
                .Where(u => u.Rol == RolUsuario.Cliente && u.EstaActivo)
                .Include(u => u.Ordenes)
                .OrderByDescending(u => u.Ordenes.Count(o => o.Estado == EstadoOrden.Entregada))
                .Take(cantidad)
                .ToListAsync();
        }
    }
}
