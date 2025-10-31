using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiendaModerna.Application.DTOs.Usuario;
using TiendaModerna.Application.Interfaces;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Enums;
using TiendaModerna.Domain.Interfaces;
using BCrypt.Net;

namespace TiendaModerna.Application.Services
{
    /// <summary>
    /// Servicio de usuarios y autenticación
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int id)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(id);
            return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
        }

        public async Task<UsuarioDto?> ObtenerPorEmailAsync(string email)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorEmailAsync(email);
            return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
        }

        public async Task<AuthResponseDto> RegistrarAsync(RegistrarUsuarioDto dto)
        {
            // Validar que el email no exista
            if (await _unitOfWork.Usuarios.EmailExisteAsync(dto.Email))
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            // Si es el primer usuario, hacerlo Administrador
            var totalUsuarios = await _unitOfWork.Usuarios.ContarAsync();
            usuario.Rol = totalUsuarios == 0 ? RolUsuario.Administrador : RolUsuario.Cliente;
            
            usuario.TokenVerificacionEmail = Guid.NewGuid().ToString();
            usuario.TokenVerificacionExpiracion = DateTime.UtcNow.AddDays(7);

            await _unitOfWork.Usuarios.AgregarAsync(usuario);
            await _unitOfWork.CompletarAsync();

            // TODO: Enviar email de verificación con usuario.TokenVerificacionEmail

            var token = GenerarToken(usuario);
            return new AuthResponseDto
            {
                Token = token,
                Expiracion = DateTime.UtcNow.AddHours(24),
                Usuario = _mapper.Map<UsuarioDto>(usuario)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorEmailAsync(dto.Email);
            
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email o contraseña incorrectos");
            }

            if (!usuario.EstaActivo)
            {
                throw new UnauthorizedAccessException("La cuenta está desactivada");
            }

            // Actualizar último acceso
            usuario.UltimoInicioSesion = DateTime.UtcNow;
            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.CompletarAsync();

            var token = GenerarToken(usuario);
            return new AuthResponseDto
            {
                Token = token,
                Expiracion = DateTime.UtcNow.AddHours(24),
                Usuario = _mapper.Map<UsuarioDto>(usuario)
            };
        }

        public async Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNueva)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorIdAsync(usuarioId);
            if (usuario == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(passwordActual, usuario.PasswordHash))
            {
                throw new InvalidOperationException("La contraseña actual es incorrecta");
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordNueva);
            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> SolicitarRecuperacionPasswordAsync(string email)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorEmailAsync(email);
            if (usuario == null) return false;

            usuario.TokenRecuperacion = Guid.NewGuid().ToString();
            usuario.TokenRecuperacionExpiracion = DateTime.UtcNow.AddHours(2);
            
            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.CompletarAsync();

            // TODO: Enviar email con link de recuperación usando usuario.TokenRecuperacionPassword

            return true;
        }

        public async Task<bool> RestablecerPasswordAsync(string token, string passwordNueva)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorTokenRecuperacionAsync(token);
            if (usuario == null || usuario.TokenRecuperacionExpiracion < DateTime.UtcNow)
            {
                throw new InvalidOperationException("El token es inválido o ha expirado");
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordNueva);
            usuario.TokenRecuperacion = null;
            usuario.TokenRecuperacionExpiracion = null;
            
            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> VerificarEmailAsync(string token)
        {
            var usuario = await _unitOfWork.Usuarios.ObtenerPorTokenVerificacionAsync(token);
            if (usuario == null || usuario.TokenVerificacionExpiracion < DateTime.UtcNow)
            {
                return false;
            }

            usuario.EmailVerificado = true;
            usuario.TokenVerificacionEmail = null;
            usuario.TokenVerificacionExpiracion = null;
            
            _unitOfWork.Usuarios.Actualizar(usuario);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _unitOfWork.Usuarios.EmailExisteAsync(email);
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada")));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
