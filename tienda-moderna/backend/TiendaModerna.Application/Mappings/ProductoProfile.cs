using AutoMapper;
using TiendaModerna.Application.DTOs.Producto;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para Producto
    /// </summary>
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            // Producto -> ProductoDto
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : null))
                .ForMember(dest => dest.MarcaNombre, opt => opt.MapFrom(src => src.Marca != null ? src.Marca.Nombre : null));

            // Producto -> ProductoListaDto
            CreateMap<Producto, ProductoListaDto>()
                .ForMember(dest => dest.ImagenPrincipalUrl, opt => opt.MapFrom(src => 
                    src.Imagenes != null && src.Imagenes.Any(i => i.EsPrincipal) 
                        ? src.Imagenes.First(i => i.EsPrincipal).Url 
                        : src.Imagenes != null && src.Imagenes.Any() 
                            ? src.Imagenes.First().Url 
                            : null))
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria != null ? src.Categoria.Nombre : null))
                .ForMember(dest => dest.MarcaNombre, opt => opt.MapFrom(src => src.Marca != null ? src.Marca.Nombre : null));

            // CrearProductoDto -> Producto
            CreateMap<CrearProductoDto, Producto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.Marca, opt => opt.Ignore())
                .ForMember(dest => dest.Variantes, opt => opt.Ignore())
                .ForMember(dest => dest.Imagenes, opt => opt.Ignore());

            // ActualizarProductoDto -> Producto
            CreateMap<ActualizarProductoDto, Producto>()
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                .ForMember(dest => dest.Marca, opt => opt.Ignore())
                .ForMember(dest => dest.Variantes, opt => opt.Ignore())
                .ForMember(dest => dest.Imagenes, opt => opt.Ignore());

            // Variante -> VarianteDto
            CreateMap<Variante, VarianteDto>();

            // Imagen -> ImagenDto
            CreateMap<Imagen, ImagenDto>();
        }
    }
}
