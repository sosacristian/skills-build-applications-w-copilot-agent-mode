using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un usuario del sistema.
    /// Puede ser cliente o administrador según su rol.
    /// Principio SRP: Solo representa un usuario, la autenticación se maneja en otra capa.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Email del usuario (también se usa para login)
        /// </summary>
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash de la contraseña (NUNCA almacenar contraseña en texto plano)
        /// Se utiliza BCrypt para el hash
        /// </summary>
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono del usuario
        /// </summary>
        [StringLength(20)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Rol del usuario en el sistema
        /// </summary>
        [Required]
        public RolUsuario Rol { get; set; } = RolUsuario.Cliente;

        /// <summary>
        /// Indica si el usuario está activo
        /// Se puede usar para "borrado lógico"
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Indica si el email fue verificado
        /// </summary>
        public bool EmailVerificado { get; set; } = false;

        /// <summary>
        /// Token para verificar el email
        /// </summary>
        [StringLength(500)]
        public string? TokenVerificacionEmail { get; set; }

        /// <summary>
        /// Fecha de expiración del token de verificación
        /// </summary>
        public DateTime? TokenVerificacionExpiracion { get; set; }

        /// <summary>
        /// Token para recuperar contraseña
        /// </summary>
        [StringLength(500)]
        public string? TokenRecuperacion { get; set; }

        /// <summary>
        /// Fecha de expiración del token de recuperación
        /// </summary>
        public DateTime? TokenRecuperacionExpiracion { get; set; }

        /// <summary>
        /// Fecha de último inicio de sesión
        /// </summary>
        public DateTime? UltimoInicioSesion { get; set; }

        /// <summary>
        /// Fecha de creación del usuario
        /// </summary>
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        // ============ DIRECCIÓN PREDETERMINADA ============

        /// <summary>
        /// Dirección predeterminada del usuario
        /// </summary>
        [StringLength(200)]
        public string? DireccionPredeterminada { get; set; }

        /// <summary>
        /// Ciudad predeterminada
        /// </summary>
        [StringLength(100)]
        public string? CiudadPredeterminada { get; set; }

        /// <summary>
        /// Provincia predeterminada
        /// </summary>
        [StringLength(100)]
        public string? ProvinciaPredeterminada { get; set; }

        /// <summary>
        /// Código postal predeterminado
        /// </summary>
        [StringLength(20)]
        public string? CodigoPostalPredeterminado { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// Colección de órdenes realizadas por el usuario
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Verifica si el usuario es administrador
        /// </summary>
        public bool EsAdministrador() => Rol == RolUsuario.Administrador;

        /// <summary>
        /// Verifica si el usuario es cliente
        /// </summary>
        public bool EsCliente() => Rol == RolUsuario.Cliente;

        /// <summary>
        /// Registra el inicio de sesión del usuario
        /// </summary>
        public void RegistrarInicioSesion()
        {
            UltimoInicioSesion = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Genera un token de verificación de email
        /// </summary>
        public void GenerarTokenVerificacionEmail()
        {
            TokenVerificacionEmail = Guid.NewGuid().ToString();
            TokenVerificacionExpiracion = DateTime.UtcNow.AddHours(24);
        }

        /// <summary>
        /// Genera un token de recuperación de contraseña
        /// </summary>
        public void GenerarTokenRecuperacion()
        {
            TokenRecuperacion = Guid.NewGuid().ToString();
            TokenRecuperacionExpiracion = DateTime.UtcNow.AddHours(2);
        }

        /// <summary>
        /// Verifica si el token de verificación de email es válido
        /// </summary>
        public bool TokenVerificacionEsValido(string token)
        {
            return TokenVerificacionEmail == token 
                && TokenVerificacionExpiracion.HasValue 
                && TokenVerificacionExpiracion.Value > DateTime.UtcNow;
        }

        /// <summary>
        /// Verifica si el token de recuperación es válido
        /// </summary>
        public bool TokenRecuperacionEsValido(string token)
        {
            return TokenRecuperacion == token 
                && TokenRecuperacionExpiracion.HasValue 
                && TokenRecuperacionExpiracion.Value > DateTime.UtcNow;
        }

        /// <summary>
        /// Marca el email como verificado
        /// </summary>
        public void VerificarEmail()
        {
            EmailVerificado = true;
            TokenVerificacionEmail = null;
            TokenVerificacionExpiracion = null;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Obtiene la dirección completa predeterminada
        /// </summary>
        public string? ObtenerDireccionCompleta()
        {
            if (string.IsNullOrEmpty(DireccionPredeterminada))
                return null;

            return $"{DireccionPredeterminada}, {CiudadPredeterminada}, {ProvinciaPredeterminada} ({CodigoPostalPredeterminado})";
        }
    }
}
