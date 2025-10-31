import { defineStore } from 'pinia'
import { ref } from 'vue'
import { productoService } from '@/services/producto.service'
import type { Producto, ProductoLista, PagedResult } from '@/types'

export const useProductoStore = defineStore('producto', () => {
  // State
  const productos = ref<ProductoLista[]>([])
  const productoActual = ref<Producto | null>(null)
  const destacados = ref<ProductoLista[]>([])
  const ofertas = ref<ProductoLista[]>([])
  const paginacion = ref<Omit<PagedResult<ProductoLista>, 'items'> | null>(null)
  const cargando = ref(false)
  const error = ref<string | null>(null)

  // Actions
  const obtenerProductos = async (pagina: number = 1, tamanoPagina: number = 20) => {
    cargando.value = true
    error.value = null

    try {
      const resultado = await productoService.obtenerTodos(pagina, tamanoPagina)
      productos.value = resultado.items
      paginacion.value = {
        paginaActual: resultado.paginaActual,
        totalPaginas: resultado.totalPaginas,
        tamanoPagina: resultado.tamanoPagina,
        totalItems: resultado.totalItems,
        tienePaginaAnterior: resultado.tienePaginaAnterior,
        tienePaginaSiguiente: resultado.tienePaginaSiguiente
      }
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al cargar productos'
    } finally {
      cargando.value = false
    }
  }

  const obtenerProductoPorId = async (id: number) => {
    cargando.value = true
    error.value = null

    try {
      productoActual.value = await productoService.obtenerPorId(id)
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al cargar producto'
      productoActual.value = null
    } finally {
      cargando.value = false
    }
  }

  const buscarProductos = async (termino: string, pagina: number = 1) => {
    cargando.value = true
    error.value = null

    try {
      const resultado = await productoService.buscar(termino, pagina, 20)
      productos.value = resultado.items
      paginacion.value = {
        paginaActual: resultado.paginaActual,
        totalPaginas: resultado.totalPaginas,
        tamanoPagina: resultado.tamanoPagina,
        totalItems: resultado.totalItems,
        tienePaginaAnterior: resultado.tienePaginaAnterior,
        tienePaginaSiguiente: resultado.tienePaginaSiguiente
      }
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al buscar productos'
    } finally {
      cargando.value = false
    }
  }

  const obtenerDestacados = async (cantidad: number = 10) => {
    try {
      destacados.value = await productoService.obtenerDestacados(cantidad)
    } catch (err: any) {
      console.error('Error al cargar productos destacados:', err)
    }
  }

  const obtenerOfertas = async (cantidad: number = 10) => {
    try {
      ofertas.value = await productoService.obtenerOfertas(cantidad)
    } catch (err: any) {
      console.error('Error al cargar ofertas:', err)
    }
  }

  const limpiarError = () => {
    error.value = null
  }

  return {
    // State
    productos,
    productoActual,
    destacados,
    ofertas,
    paginacion,
    cargando,
    error,
    // Actions
    obtenerProductos,
    obtenerProductoPorId,
    buscarProductos,
    obtenerDestacados,
    obtenerOfertas,
    limpiarError
  }
})
