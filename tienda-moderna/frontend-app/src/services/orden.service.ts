import api from './api'
import type { 
  Orden, 
  PagedResult,
  CrearOrdenDto
} from '@/types'

export const ordenService = {
  /**
   * Obtener orden por ID
   */
  async obtenerPorId(id: number): Promise<Orden> {
    const response = await api.get<Orden>(`/ordenes/${id}`)
    return response.data
  },

  /**
   * Obtener orden por número
   */
  async obtenerPorNumero(numeroOrden: string): Promise<Orden> {
    const response = await api.get<Orden>(`/ordenes/numero/${numeroOrden}`)
    return response.data
  },

  /**
   * Obtener mis órdenes
   */
  async obtenerMisOrdenes(pagina: number = 1, tamanoPagina: number = 10): Promise<PagedResult<Orden>> {
    const response = await api.get<PagedResult<Orden>>('/ordenes/mis-ordenes', {
      params: { pagina, tamanoPagina }
    })
    return response.data
  },

  /**
   * Crear nueva orden
   */
  async crear(datos: CrearOrdenDto): Promise<Orden> {
    const response = await api.post<Orden>('/ordenes', datos)
    return response.data
  },

  /**
   * Cancelar orden
   */
  async cancelar(id: number): Promise<void> {
    await api.post(`/ordenes/${id}/cancelar`)
  }
}
