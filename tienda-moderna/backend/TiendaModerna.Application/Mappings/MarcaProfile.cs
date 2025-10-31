using AutoMapper;
using TiendaModerna.Application.DTOs.Marca;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para Marca
    /// </summary>
    public class MarcaProfile : Profile
    {
        public MarcaProfile()
        {
            // Marca -> MarcaDto
            CreateMap<Marca, MarcaDto>()
                .ForMember(dest => dest.CantidadProductos, opt => opt.MapFrom(src => src.Productos != null ? src.Productos.Count : 0));
        }
    }
}
