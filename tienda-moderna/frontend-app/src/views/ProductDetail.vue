<template>
  <div v-if="productoStore.cargando">
    <LoadingSpinner message="Cargando producto..." />
  </div>

  <div v-else-if="productoStore.error">
    <Alert
      :message="productoStore.error"
      type="error"
      @close="() => router.push({ name: 'productos' })"
    />
  </div>

  <div v-else-if="producto" class="grid grid-cols-1 lg:grid-cols-2 gap-12">
    <!-- Imágenes -->
    <div>
      <div class="sticky top-4">
        <div class="bg-gray-200 rounded-lg overflow-hidden mb-4" style="aspect-ratio: 1/1;">
          <img
            v-if="imagenActual"
            :src="imagenActual.url"
            :alt="producto.nombre"
            class="w-full h-full object-cover"
          />
          <div v-else class="w-full h-full flex items-center justify-center text-gray-400">
            Sin imagen
          </div>
        </div>

        <div v-if="producto.imagenes && producto.imagenes.length > 1" class="grid grid-cols-4 gap-2">
          <div
            v-for="imagen in producto.imagenes"
            :key="imagen.id"
            @click="imagenActual = imagen"
            class="cursor-pointer border-2 rounded overflow-hidden"
            :class="imagenActual?.id === imagen.id ? 'border-primary-600' : 'border-gray-300'"
          >
            <img
              :src="imagen.url"
              :alt="imagen.altText || producto.nombre"
              class="w-full h-full object-cover"
            />
          </div>
        </div>
      </div>
    </div>

    <!-- Información del producto -->
    <div>
      <h1 class="text-4xl font-bold mb-2">{{ producto.nombre }}</h1>
      <p class="text-gray-600 mb-4">{{ producto.marca?.nombre }}</p>

      <div class="flex items-baseline gap-3 mb-6">
        <span class="text-4xl font-bold text-primary-600">
          ${{ precioFinal.toLocaleString('es-AR') }}
        </span>
        <span v-if="tieneDescuento" class="text-xl text-gray-400 line-through">
          ${{ producto.precioBase.toLocaleString('es-AR') }}
        </span>
        <span v-if="tieneDescuento" class="badge badge-danger">
          -{{ porcentajeDescuento }}%
        </span>
      </div>

      <div class="mb-6">
        <h3 class="font-semibold mb-2">Descripción</h3>
        <p class="text-gray-700">{{ producto.descripcion }}</p>
      </div>

      <!-- Variantes -->
      <div v-if="producto.variantes && producto.variantes.length > 0" class="mb-6">
        <h3 class="font-semibold mb-2">Seleccionar variante</h3>
        <div class="grid grid-cols-3 gap-2">
          <button
            v-for="variante in producto.variantes"
            :key="variante.id"
            @click="varianteSeleccionada = variante"
            class="border-2 rounded p-2 text-sm"
            :class="varianteSeleccionada?.id === variante.id 
              ? 'border-primary-600 bg-primary-50' 
              : 'border-gray-300 hover:border-gray-400'"
            :disabled="variante.stock === 0"
          >
            <div>{{ variante.talla }}</div>
            <div class="text-xs text-gray-500">
              {{ variante.stock > 0 ? `Stock: ${variante.stock}` : 'Sin stock' }}
            </div>
          </button>
        </div>
      </div>

      <!-- Stock -->
      <div class="mb-6">
        <span v-if="stockDisponible > 10" class="text-green-600 font-medium">
          ✓ En stock
        </span>
        <span v-else-if="stockDisponible > 0" class="text-yellow-600 font-medium">
          ⚠ Últimas {{ stockDisponible }} unidades
        </span>
        <span v-else class="text-red-600 font-medium">
          ✕ Sin stock
        </span>
      </div>

      <!-- Cantidad -->
      <div class="mb-6">
        <label class="block font-semibold mb-2">Cantidad</label>
        <div class="flex items-center gap-3">
          <button
            @click="cantidad--"
            :disabled="cantidad <= 1"
            class="btn btn-outline w-10 h-10"
          >
            -
          </button>
          <input
            v-model.number="cantidad"
            type="number"
            min="1"
            :max="stockDisponible"
            class="input w-20 text-center"
          />
          <button
            @click="cantidad++"
            :disabled="cantidad >= stockDisponible"
            class="btn btn-outline w-10 h-10"
          >
            +
          </button>
        </div>
      </div>

      <!-- Botones de acción -->
      <div class="flex gap-4">
        <button
          @click="agregarAlCarrito"
          :disabled="stockDisponible === 0"
          class="btn btn-primary flex-1"
        >
          Agregar al Carrito
        </button>
        <button
          @click="comprarAhora"
          :disabled="stockDisponible === 0"
          class="btn btn-outline flex-1"
        >
          Comprar Ahora
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useProductoStore, useCarritoStore } from '@/stores'
import type { Variante, Imagen } from '@/types'
import LoadingSpinner from '@/components/LoadingSpinner.vue'
import Alert from '@/components/Alert.vue'

const route = useRoute()
const router = useRouter()
const productoStore = useProductoStore()
const carritoStore = useCarritoStore()

const varianteSeleccionada = ref<Variante | null>(null)
const imagenActual = ref<Imagen | null>(null)
const cantidad = ref(1)

const producto = computed(() => productoStore.productoActual)

const precioFinal = computed(() => {
  if (!producto.value) return 0
  return varianteSeleccionada.value?.precio || producto.value.precioFinal
})

const tieneDescuento = computed(() => {
  if (!producto.value) return false
  return producto.value.precioBase > precioFinal.value
})

const porcentajeDescuento = computed(() => {
  if (!producto.value || !tieneDescuento.value) return 0
  return Math.round(((producto.value.precioBase - precioFinal.value) / producto.value.precioBase) * 100)
})

const stockDisponible = computed(() => {
  if (!producto.value) return 0
  return varianteSeleccionada.value?.stock || producto.value.stock
})

const agregarAlCarrito = () => {
  if (!producto.value) return
  
  const productoLista = {
    id: producto.value.id,
    nombre: producto.value.nombre,
    descripcion: producto.value.descripcion,
    precioBase: producto.value.precioBase,
    precioFinal: precioFinal.value,
    sku: producto.value.sku,
    stockTotal: stockDisponible.value,
    imagenPrincipal: imagenActual.value?.url || '',
    nombreMarca: producto.value.marca?.nombre || '',
    nombreCategoria: producto.value.categoria?.nombre || '',
    tieneDescuento: tieneDescuento.value,
    porcentajeDescuento: porcentajeDescuento.value,
    activo: producto.value.activo,
    destacado: producto.value.destacado,
    fechaCreacion: producto.value.fechaCreacion
  }

  carritoStore.agregarProducto(productoLista, cantidad.value)
  cantidad.value = 1
}

const comprarAhora = () => {
  agregarAlCarrito()
  router.push({ name: 'carrito' })
}

onMounted(async () => {
  const id = Number(route.params.id)
  await productoStore.obtenerProductoPorId(id)
  
  if (producto.value) {
    if (producto.value.imagenes && producto.value.imagenes.length > 0) {
      imagenActual.value = producto.value.imagenes[0]
    }
    
    if (producto.value.variantes && producto.value.variantes.length > 0) {
      const varianteConStock = producto.value.variantes.find((v: Variante) => v.stock > 0)
      varianteSeleccionada.value = varianteConStock || producto.value.variantes[0]
    }
  }
})
</script>
