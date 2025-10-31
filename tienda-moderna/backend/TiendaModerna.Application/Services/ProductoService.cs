using AutoMapper;
using TiendaModerna.Application.DTOs.Common;
using TiendaModerna.Application.DTOs.Producto;
using TiendaModerna.Application.Interfaces;
using TiendaModerna.Domain.Entities;
using TiendaModerna.Domain.Interfaces;

namespace TiendaModerna.Application.Services
{
    /// <summary>
    /// Servicio de productos con lógica de negocio
    /// </summary>
    public class ProductoService : IProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            var producto = await _unitOfWork.Productos.ObtenerCompletoAsync(id);
            return producto != null ? _mapper.Map<ProductoDto>(producto) : null;
        }

        public async Task<ProductoDto?> ObtenerPorSKUAsync(string sku)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorSKUAsync(sku);
            if (producto == null) return null;

            // Cargar relaciones completas si es necesario
            producto = await _unitOfWork.Productos.ObtenerCompletoAsync(producto.Id);
            return _mapper.Map<ProductoDto>(producto);
        }

        public async Task<PagedResult<ProductoListaDto>> ObtenerPaginadoAsync(int pagina, int tamanoPagina)
        {
            var todosProductos = await _unitOfWork.Productos.ObtenerTodosAsync();
            var total = todosProductos.Count();

            var productosPaginados = todosProductos
                .OrderByDescending(p => p.FechaCreacion)
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return new PagedResult<ProductoListaDto>
            {
                Items = _mapper.Map<List<ProductoListaDto>>(productosPaginados),
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina)
            };
        }

        public async Task<PagedResult<ProductoListaDto>> ObtenerPorCategoriaAsync(int categoriaId, int pagina, int tamanoPagina)
        {
            var productosEnumerable = await _unitOfWork.Productos.ObtenerPorCategoriaAsync(categoriaId);
            var productos = productosEnumerable.ToList();
            var total = productos.Count;

            // Paginación manual
            var productosPaginados = productos
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return new PagedResult<ProductoListaDto>
            {
                Items = _mapper.Map<List<ProductoListaDto>>(productosPaginados),
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina)
            };
        }

        public async Task<PagedResult<ProductoListaDto>> ObtenerPorMarcaAsync(int marcaId, int pagina, int tamanoPagina)
        {
            var productosEnumerable = await _unitOfWork.Productos.ObtenerPorMarcaAsync(marcaId);
            var productos = productosEnumerable.ToList();
            var total = productos.Count;

            var productosPaginados = productos
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return new PagedResult<ProductoListaDto>
            {
                Items = _mapper.Map<List<ProductoListaDto>>(productosPaginados),
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina)
            };
        }

        public async Task<List<ProductoListaDto>> ObtenerDestacadosAsync(int cantidad = 10)
        {
            var productos = await _unitOfWork.Productos.ObtenerDestacadosAsync(cantidad);
            return _mapper.Map<List<ProductoListaDto>>(productos);
        }

        public async Task<List<ProductoListaDto>> ObtenerEnOfertaAsync(int cantidad = 10)
        {
            var productosEnumerable = await _unitOfWork.Productos.ObtenerEnOfertaAsync();
            var productos = productosEnumerable.Take(cantidad).ToList();
            return _mapper.Map<List<ProductoListaDto>>(productos);
        }

        public async Task<PagedResult<ProductoListaDto>> BuscarAsync(string termino, int pagina, int tamanoPagina)
        {
            var productosEnumerable = await _unitOfWork.Productos.BuscarPorTerminoAsync(termino);
            var productos = productosEnumerable.ToList();
            var total = productos.Count;

            var productosPaginados = productos
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            return new PagedResult<ProductoListaDto>
            {
                Items = _mapper.Map<List<ProductoListaDto>>(productosPaginados),
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                TotalItems = total,
                TotalPaginas = (int)Math.Ceiling(total / (double)tamanoPagina)
            };
        }

        public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
        {
            // Validar que el SKU no exista
            if (await _unitOfWork.Productos.SKUExisteAsync(dto.CodigoSKU))
            {
                throw new InvalidOperationException($"Ya existe un producto con el SKU '{dto.CodigoSKU}'");
            }

            var producto = _mapper.Map<Producto>(dto);
            await _unitOfWork.Productos.AgregarAsync(producto);
            await _unitOfWork.CompletarAsync();

            // Recargar con relaciones
            var productoCompleto = await _unitOfWork.Productos.ObtenerCompletoAsync(producto.Id);
            return _mapper.Map<ProductoDto>(productoCompleto!);
        }

        public async Task<ProductoDto> ActualizarAsync(ActualizarProductoDto dto)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(dto.Id);
            if (producto == null)
            {
                throw new KeyNotFoundException($"No se encontró el producto con ID {dto.Id}");
            }

            // Validar SKU único (excepto el propio producto)
            if (await _unitOfWork.Productos.SKUExisteAsync(dto.CodigoSKU) && producto.CodigoSKU != dto.CodigoSKU)
            {
                throw new InvalidOperationException($"Ya existe otro producto con el SKU '{dto.CodigoSKU}'");
            }

            _mapper.Map(dto, producto);
            _unitOfWork.Productos.Actualizar(producto);
            await _unitOfWork.CompletarAsync();

            // Recargar con relaciones
            var productoCompleto = await _unitOfWork.Productos.ObtenerCompletoAsync(producto.Id);
            return _mapper.Map<ProductoDto>(productoCompleto!);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(id);
            if (producto == null) return false;

            _unitOfWork.Productos.Eliminar(producto);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int id, bool estaActivo)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(id);
            if (producto == null) return false;

            producto.EstaActivo = estaActivo;
            _unitOfWork.Productos.Actualizar(producto);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> ActualizarStockAsync(int id, int nuevoStock)
        {
            var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(id);
            if (producto == null) return false;

            producto.CantidadStock = nuevoStock;
            _unitOfWork.Productos.Actualizar(producto);
            await _unitOfWork.CompletarAsync();
            return true;
        }

        public async Task<bool> SKUExisteAsync(string sku, int? idExcluir = null)
        {
            if (!await _unitOfWork.Productos.SKUExisteAsync(sku))
                return false;

            if (idExcluir.HasValue)
            {
                var producto = await _unitOfWork.Productos.ObtenerPorSKUAsync(sku);
                return producto != null && producto.Id != idExcluir.Value;
            }

            return true;
        }
    }
}
