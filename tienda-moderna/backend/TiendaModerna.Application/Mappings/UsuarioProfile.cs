using AutoMapper;
using TiendaModerna.Application.DTOs.Usuario;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para Usuario
    /// </summary>
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Usuario -> UsuarioDto (excluye información sensible)
            CreateMap<Usuario, UsuarioDto>();

            // RegistrarUsuarioDto -> Usuario
            CreateMap<RegistrarUsuarioDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Se manejará en el servicio con BCrypt
                .ForMember(dest => dest.Rol, opt => opt.Ignore()) // Se asignará en el servicio
                .ForMember(dest => dest.EstaActivo, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.EmailVerificado, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.TokenVerificacionEmail, opt => opt.Ignore())
                .ForMember(dest => dest.TokenVerificacionExpiracion, opt => opt.Ignore())
                .ForMember(dest => dest.TokenRecuperacion, opt => opt.Ignore())
                .ForMember(dest => dest.TokenRecuperacionExpiracion, opt => opt.Ignore())
                .ForMember(dest => dest.UltimoInicioSesion, opt => opt.Ignore())
                .ForMember(dest => dest.DireccionPredeterminada, opt => opt.Ignore())
                .ForMember(dest => dest.CiudadPredeterminada, opt => opt.Ignore())
                .ForMember(dest => dest.ProvinciaPredeterminada, opt => opt.Ignore())
                .ForMember(dest => dest.CodigoPostalPredeterminado, opt => opt.Ignore())
                .ForMember(dest => dest.Ordenes, opt => opt.Ignore());
        }
    }
}
