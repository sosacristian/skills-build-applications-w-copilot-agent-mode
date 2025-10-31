import api from './api'
import type { 
  Producto, 
  ProductoLista, 
  PagedResult,
  CrearProductoDto
} from '@/types'

export const productoService = {
  /**
   * Obtener productos paginados
   */
  async obtenerTodos(pagina: number = 1, tamanoPagina: number = 20): Promise<PagedResult<ProductoLista>> {
    const response = await api.get<PagedResult<ProductoLista>>('/productos', {
      params: { pagina, tamanoPagina }
    })
    return response.data
  },

  /**
   * Obtener producto por ID
   */
  async obtenerPorId(id: number): Promise<Producto> {
    const response = await api.get<Producto>(`/productos/${id}`)
    return response.data
  },

  /**
   * Obtener producto por SKU
   */
  async obtenerPorSKU(sku: string): Promise<Producto> {
    const response = await api.get<Producto>(`/productos/sku/${sku}`)
    return response.data
  },

  /**
   * Buscar productos
   */
  async buscar(termino: string, pagina: number = 1, tamanoPagina: number = 20): Promise<PagedResult<ProductoLista>> {
    const response = await api.get<PagedResult<ProductoLista>>('/productos/buscar', {
      params: { termino, pagina, tamanoPagina }
    })
    return response.data
  },

  /**
   * Obtener productos por categoría
   */
  async obtenerPorCategoria(categoriaId: number, pagina: number = 1, tamanoPagina: number = 20): Promise<PagedResult<ProductoLista>> {
    const response = await api.get<PagedResult<ProductoLista>>(`/productos/categoria/${categoriaId}`, {
      params: { pagina, tamanoPagina }
    })
    return response.data
  },

  /**
   * Obtener productos destacados
   */
  async obtenerDestacados(cantidad: number = 10): Promise<ProductoLista[]> {
    const response = await api.get<ProductoLista[]>('/productos/destacados', {
      params: { cantidad }
    })
    return response.data
  },

  /**
   * Obtener productos en oferta
   */
  async obtenerOfertas(cantidad: number = 10): Promise<ProductoLista[]> {
    const response = await api.get<ProductoLista[]>('/productos/ofertas', {
      params: { cantidad }
    })
    return response.data
  },

  /**
   * Crear producto (solo admin)
   */
  async crear(datos: CrearProductoDto): Promise<Producto> {
    const response = await api.post<Producto>('/productos', datos)
    return response.data
  },

  /**
   * Actualizar producto (solo admin)
   */
  async actualizar(id: number, datos: Partial<CrearProductoDto>): Promise<Producto> {
    const response = await api.put<Producto>(`/productos/${id}`, { id, ...datos })
    return response.data
  },

  /**
   * Eliminar producto (solo admin)
   */
  async eliminar(id: number): Promise<void> {
    await api.delete(`/productos/${id}`)
  },

  /**
   * Cambiar estado del producto (solo admin)
   */
  async cambiarEstado(id: number, estaActivo: boolean): Promise<void> {
    await api.patch(`/productos/${id}/estado`, estaActivo, {
      headers: { 'Content-Type': 'application/json' }
    })
  }
}
