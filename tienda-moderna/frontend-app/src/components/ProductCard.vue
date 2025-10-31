<template>
  <div class="card hover:shadow-lg transition-shadow cursor-pointer" @click="verDetalle">
    <!-- Imagen -->
    <div class="relative pb-[100%] bg-gray-200 rounded-lg overflow-hidden mb-4">
      <img
        v-if="producto.imagenPrincipal"
        :src="producto.imagenPrincipal"
        :alt="producto.nombre"
        class="absolute inset-0 w-full h-full object-cover"
      />
      <div v-else class="absolute inset-0 flex items-center justify-center text-gray-400">
        Sin imagen
      </div>

      <!-- Badge de descuento -->
      <div
        v-if="producto.tieneDescuento && producto.porcentajeDescuento > 0"
        class="absolute top-2 right-2 bg-red-500 text-white px-2 py-1 rounded text-sm font-bold"
      >
        -{{ producto.porcentajeDescuento }}%
      </div>

      <!-- Badge de nuevo -->
      <div
        v-if="esNuevo"
        class="absolute top-2 left-2 bg-green-500 text-white px-2 py-1 rounded text-sm font-bold"
      >
        Nuevo
      </div>
    </div>

    <!-- Información -->
    <div>
      <h3 class="font-semibold text-lg mb-1 line-clamp-2">{{ producto.nombre }}</h3>
      <p class="text-sm text-gray-600 mb-2">{{ producto.nombreMarca }}</p>

      <!-- Precios -->
      <div class="flex items-baseline gap-2 mb-3">
        <span class="text-2xl font-bold text-primary-600">
          ${{ producto.precioFinal.toLocaleString('es-AR') }}
        </span>
        <span
          v-if="producto.tieneDescuento"
          class="text-sm text-gray-400 line-through"
        >
          ${{ producto.precioBase.toLocaleString('es-AR') }}
        </span>
      </div>

      <!-- Stock -->
      <div class="mb-3">
        <span
          v-if="producto.stockTotal > 10"
          class="text-sm text-green-600"
        >
          En stock
        </span>
        <span
          v-else-if="producto.stockTotal > 0"
          class="text-sm text-yellow-600"
        >
          Últimas {{ producto.stockTotal }} unidades
        </span>
        <span
          v-else
          class="text-sm text-red-600"
        >
          Sin stock
        </span>
      </div>

      <!-- Botón de acción -->
      <button
        @click.stop="agregarAlCarrito"
        :disabled="producto.stockTotal === 0"
        class="btn btn-primary w-full"
      >
        Agregar al Carrito
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useCarritoStore } from '@/stores'
import type { ProductoLista } from '@/types'

interface Props {
  producto: ProductoLista
}

const props = defineProps<Props>()
const router = useRouter()
const carritoStore = useCarritoStore()

const esNuevo = computed(() => {
  const fechaCreacion = new Date(props.producto.fechaCreacion)
  const diasDesdeCreacion = (Date.now() - fechaCreacion.getTime()) / (1000 * 60 * 60 * 24)
  return diasDesdeCreacion <= 30
})

const verDetalle = () => {
  router.push({ name: 'producto-detalle', params: { id: props.producto.id } })
}

const agregarAlCarrito = () => {
  carritoStore.agregarProducto(props.producto, 1)
}
</script>

<style scoped>
.line-clamp-2 {
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
</style>
