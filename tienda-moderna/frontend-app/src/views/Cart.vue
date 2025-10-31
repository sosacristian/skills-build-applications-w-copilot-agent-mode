<template>
  <div class="max-w-4xl mx-auto">
    <h1 class="text-4xl font-bold mb-8">Carrito de Compras</h1>

    <div v-if="carritoStore.items.length === 0" class="card text-center py-12">
      <p class="text-gray-500 mb-4">Tu carrito está vacío</p>
      <RouterLink to="/productos" class="btn btn-primary">
        Ver Productos
      </RouterLink>
    </div>

    <div v-else class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Items del carrito -->
      <div class="lg:col-span-2 space-y-4">
        <div
          v-for="item in carritoStore.items"
          :key="item.producto.id"
          class="card flex gap-4"
        >
          <img
            :src="item.producto.imagenPrincipal"
            :alt="item.producto.nombre"
            class="w-24 h-24 object-cover rounded"
          />
          
          <div class="flex-1">
            <h3 class="font-semibold text-lg">{{ item.producto.nombre }}</h3>
            <p class="text-gray-600 text-sm">{{ item.producto.nombreMarca }}</p>
            <p class="text-primary-600 font-bold mt-2">
              ${{ item.producto.precioFinal.toLocaleString('es-AR') }}
            </p>

            <div class="flex items-center gap-2 mt-2">
              <button
                @click="carritoStore.actualizarCantidad(item.producto.id, item.cantidad - 1)"
                class="btn btn-outline w-8 h-8 text-sm"
              >
                -
              </button>
              <span class="w-12 text-center">{{ item.cantidad }}</span>
              <button
                @click="carritoStore.actualizarCantidad(item.producto.id, item.cantidad + 1)"
                class="btn btn-outline w-8 h-8 text-sm"
              >
                +
              </button>
              <button
                @click="carritoStore.eliminarProducto(item.producto.id)"
                class="btn bg-red-100 text-red-600 hover:bg-red-200 ml-auto"
              >
                Eliminar
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- Resumen -->
      <div class="lg:col-span-1">
        <div class="card sticky top-4">
          <h2 class="text-2xl font-bold mb-4">Resumen</h2>
          
          <div class="space-y-2 mb-4">
            <div class="flex justify-between">
              <span>Subtotal:</span>
              <span>${{ carritoStore.subtotal.toLocaleString('es-AR') }}</span>
            </div>
            <div v-if="carritoStore.totalDescuentos > 0" class="flex justify-between text-green-600">
              <span>Descuentos:</span>
              <span>-${{ carritoStore.totalDescuentos.toLocaleString('es-AR') }}</span>
            </div>
            <div class="border-t pt-2 flex justify-between font-bold text-lg">
              <span>Total:</span>
              <span class="text-primary-600">${{ carritoStore.subtotal.toLocaleString('es-AR') }}</span>
            </div>
          </div>

          <RouterLink to="/checkout" class="btn btn-primary w-full">
            Proceder al Pago
          </RouterLink>

          <button
            @click="carritoStore.vaciarCarrito()"
            class="btn btn-outline w-full mt-2"
          >
            Vaciar Carrito
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useCarritoStore } from '@/stores'

const carritoStore = useCarritoStore()
</script>
