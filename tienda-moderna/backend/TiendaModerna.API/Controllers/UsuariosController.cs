using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaModerna.Application.DTOs.Usuario;
using TiendaModerna.Application.Interfaces;

namespace TiendaModerna.API.Controllers
{
    /// <summary>
    /// Controlador para gestión de usuarios y autenticación
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        /// <summary>
        /// Registrar un nuevo usuario
        /// </summary>
        [HttpPost("registrar")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseDto>> Registrar([FromBody] RegistrarUsuarioDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _usuarioService.RegistrarAsync(dto);
                _logger.LogInformation("Usuario registrado: {Email}", dto.Email);
                
                return CreatedAtAction(nameof(ObtenerPorId), new { id = resultado.Usuario.Id }, resultado);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Error al registrar usuario: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar usuario");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Iniciar sesión
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _usuarioService.LoginAsync(dto);
                _logger.LogInformation("Usuario autenticado: {Email}", dto.Email);
                
                return Ok(resultado);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Intento de login fallido: {Message}", ex.Message);
                return Unauthorized(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en login");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtener usuario por ID (requiere autenticación)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDto>> ObtenerPorId(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObtenerPorIdAsync(id);
                if (usuario == null)
                    return NotFound(new { error = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario {Id}", id);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Verificar si un email ya existe
        /// </summary>
        [HttpGet("email-existe/{email}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> EmailExiste(string email)
        {
            try
            {
                var existe = await _usuarioService.EmailExisteAsync(email);
                return Ok(new { existe });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar email");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Solicitar recuperación de contraseña
        /// </summary>
        [HttpPost("recuperar-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SolicitarRecuperacionPassword([FromBody] string email)
        {
            try
            {
                await _usuarioService.SolicitarRecuperacionPasswordAsync(email);
                return Ok(new { mensaje = "Si el email existe, se enviará un correo con instrucciones" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al solicitar recuperación de password");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Restablecer contraseña con token
        /// </summary>
        [HttpPost("restablecer-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RestablecerPassword([FromBody] RestablecerPasswordDto dto)
        {
            try
            {
                var resultado = await _usuarioService.RestablecerPasswordAsync(dto.Token, dto.PasswordNueva);
                if (!resultado)
                    return BadRequest(new { error = "Token inválido o expirado" });

                return Ok(new { mensaje = "Contraseña restablecida exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer password");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Verificar email con token
        /// </summary>
        [HttpGet("verificar-email/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> VerificarEmail(string token)
        {
            try
            {
                var resultado = await _usuarioService.VerificarEmailAsync(token);
                if (!resultado)
                    return BadRequest(new { error = "Token inválido o expirado" });

                return Ok(new { mensaje = "Email verificado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar email");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }

    /// <summary>
    /// DTO para restablecer contraseña
    /// </summary>
    public class RestablecerPasswordDto
    {
        public string Token { get; set; } = string.Empty;
        public string PasswordNueva { get; set; } = string.Empty;
    }
}
