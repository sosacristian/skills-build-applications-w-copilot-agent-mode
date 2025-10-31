import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { ItemCarrito, ProductoLista } from '@/types'

export const useCarritoStore = defineStore('carrito', () => {
  // State
  const items = ref<ItemCarrito[]>([])

  // Getters
  const cantidadTotal = computed(() => 
    items.value.reduce((total, item) => total + item.cantidad, 0)
  )

  const subtotal = computed(() =>
    items.value.reduce((total, item) => total + (item.producto.precioFinal * item.cantidad), 0)
  )

  const totalDescuentos = computed(() =>
    items.value.reduce((total, item) => {
      const descuento = (item.producto.precioBase - item.producto.precioFinal) * item.cantidad
      return total + descuento
    }, 0)
  )

  // Actions
  const cargarDesdeStorage = () => {
    const carritoGuardado = localStorage.getItem('carrito')
    if (carritoGuardado) {
      try {
        items.value = JSON.parse(carritoGuardado)
      } catch (err) {
        console.error('Error al cargar carrito:', err)
        items.value = []
      }
    }
  }

  const guardarEnStorage = () => {
    localStorage.setItem('carrito', JSON.stringify(items.value))
  }

  const agregarProducto = (producto: ProductoLista, cantidad: number = 1) => {
    const itemExistente = items.value.find(item => item.producto.id === producto.id)

    if (itemExistente) {
      itemExistente.cantidad += cantidad
    } else {
      items.value.push({ producto, cantidad })
    }

    guardarEnStorage()
  }

  const actualizarCantidad = (productoId: number, cantidad: number) => {
    const item = items.value.find(item => item.producto.id === productoId)
    if (item) {
      if (cantidad <= 0) {
        eliminarProducto(productoId)
      } else {
        item.cantidad = cantidad
        guardarEnStorage()
      }
    }
  }

  const eliminarProducto = (productoId: number) => {
    items.value = items.value.filter(item => item.producto.id !== productoId)
    guardarEnStorage()
  }

  const vaciarCarrito = () => {
    items.value = []
    localStorage.removeItem('carrito')
  }

  return {
    // State
    items,
    // Getters
    cantidadTotal,
    subtotal,
    totalDescuentos,
    // Actions
    cargarDesdeStorage,
    agregarProducto,
    actualizarCantidad,
    eliminarProducto,
    vaciarCarrito
  }
})
