using AutoMapper;
using TiendaModerna.Application.DTOs.Categoria;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para Categoria
    /// </summary>
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            // Categoria -> CategoriaDto
            CreateMap<Categoria, CategoriaDto>()
                .ForMember(dest => dest.CategoriaPadreNombre, opt => opt.MapFrom(src => src.CategoriaPadre != null ? src.CategoriaPadre.Nombre : null))
                .ForMember(dest => dest.SubCategorias, opt => opt.MapFrom(src => src.SubCategorias))
                .ForMember(dest => dest.CantidadProductos, opt => opt.MapFrom(src => src.Productos != null ? src.Productos.Count : 0));

            // CrearCategoriaDto -> Categoria
            CreateMap<CrearCategoriaDto, Categoria>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.CategoriaPadre, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategorias, opt => opt.Ignore())
                .ForMember(dest => dest.Productos, opt => opt.Ignore());
        }
    }
}
