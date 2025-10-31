import { defineStore } from 'pinia'
import { ref } from 'vue'
import { ordenService } from '@/services/orden.service'
import type { Orden, CrearOrdenDto } from '@/types'

export const useOrdenStore = defineStore('orden', () => {
  // State
  const ordenes = ref<Orden[]>([])
  const ordenActual = ref<Orden | null>(null)
  const cargando = ref(false)
  const error = ref<string | null>(null)

  // Actions
  const obtenerMisOrdenes = async () => {
    cargando.value = true
    error.value = null

    try {
      ordenes.value = await ordenService.obtenerMisOrdenes()
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al cargar órdenes'
    } finally {
      cargando.value = false
    }
  }

  const obtenerOrdenPorId = async (id: number) => {
    cargando.value = true
    error.value = null

    try {
      ordenActual.value = await ordenService.obtenerPorId(id)
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al cargar orden'
      ordenActual.value = null
    } finally {
      cargando.value = false
    }
  }

  const crearOrden = async (datos: CrearOrdenDto): Promise<Orden | null> => {
    cargando.value = true
    error.value = null

    try {
      const orden = await ordenService.crear(datos)
      ordenActual.value = orden
      
      // Agregar a la lista de órdenes
      ordenes.value.unshift(orden)
      
      return orden
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al crear orden'
      return null
    } finally {
      cargando.value = false
    }
  }

  const cancelarOrden = async (id: number): Promise<boolean> => {
    cargando.value = true
    error.value = null

    try {
      const ordenCancelada = await ordenService.cancelar(id)
      
      // Actualizar en la lista
      const index = ordenes.value.findIndex((o: Orden) => o.id === id)
      if (index !== -1) {
        ordenes.value[index] = ordenCancelada
      }
      
      // Actualizar orden actual si corresponde
      if (ordenActual.value?.id === id) {
        ordenActual.value = ordenCancelada
      }
      
      return true
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al cancelar orden'
      return false
    } finally {
      cargando.value = false
    }
  }

  const limpiarError = () => {
    error.value = null
  }

  return {
    // State
    ordenes,
    ordenActual,
    cargando,
    error,
    // Actions
    obtenerMisOrdenes,
    obtenerOrdenPorId,
    crearOrden,
    cancelarOrden,
    limpiarError
  }
})
