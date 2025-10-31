using AutoMapper;
using TiendaModerna.Application.DTOs.Orden;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para Orden
    /// </summary>
    public class OrdenProfile : Profile
    {
        public OrdenProfile()
        {
            // Orden -> OrdenDto
            CreateMap<Orden, OrdenDto>()
                .ForMember(dest => dest.UsuarioNombreCompleto, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.NombreCompleto : null))
                .ForMember(dest => dest.UsuarioEmail, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Email : null));

            // DetalleOrden -> DetalleOrdenDto
            CreateMap<DetalleOrden, DetalleOrdenDto>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Nombre : string.Empty))
                .ForMember(dest => dest.CodigoSKUProducto, opt => opt.MapFrom(src => src.Producto != null ? src.Producto.CodigoSKU : string.Empty))
                .ForMember(dest => dest.UrlImagenProducto, opt => opt.MapFrom(src => 
                    src.Producto != null && src.Producto.Imagenes != null && src.Producto.Imagenes.Any(i => i.EsPrincipal)
                        ? src.Producto.Imagenes.First(i => i.EsPrincipal).Url
                        : src.Producto != null && src.Producto.Imagenes != null && src.Producto.Imagenes.Any()
                            ? src.Producto.Imagenes.First().Url
                            : null));

            // CrearOrdenDto -> Orden
            CreateMap<CrearOrdenDto, Orden>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NumeroOrden, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.TotalDescuentos, opt => opt.Ignore())
                .ForMember(dest => dest.CostoEnvio, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaPago, opt => opt.Ignore())
                .ForMember(dest => dest.FechaEnvio, opt => opt.Ignore())
                .ForMember(dest => dest.FechaEntrega, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Detalles, opt => opt.Ignore());

            // CrearDetalleOrdenDto -> DetalleOrden
            CreateMap<CrearDetalleOrdenDto, DetalleOrden>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrdenId, opt => opt.Ignore())
                .ForMember(dest => dest.PrecioUnitario, opt => opt.Ignore())
                .ForMember(dest => dest.DescuentoUnitario, opt => opt.Ignore())
                .ForMember(dest => dest.Subtotal, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore())
                .ForMember(dest => dest.DetalleVariante, opt => opt.Ignore())
                .ForMember(dest => dest.Orden, opt => opt.Ignore())
                .ForMember(dest => dest.Producto, opt => opt.Ignore());
        }
    }
}
