<template>
  <div>
    <h1 class="text-4xl font-bold mb-8">Productos</h1>

    <!-- Filtros y búsqueda -->
    <div class="card mb-8">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <input
          v-model="searchTerm"
          @input="buscar"
          type="text"
          placeholder="Buscar productos..."
          class="input"
        />
      </div>
    </div>

    <!-- Loading -->
    <LoadingSpinner v-if="productoStore.cargando" message="Cargando productos..." />

    <!-- Error -->
    <Alert
      v-else-if="productoStore.error"
      :message="productoStore.error"
      type="error"
      @close="productoStore.limpiarError()"
    />

    <!-- Productos -->
    <div v-else>
      <div v-if="productoStore.productos.length > 0">
        <div class="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6 mb-8">
          <ProductCard
            v-for="producto in productoStore.productos"
            :key="producto.id"
            :producto="producto"
          />
        </div>

        <!-- Paginación -->
        <div v-if="productoStore.paginacion" class="flex justify-center items-center gap-4">
          <button
            @click="cambiarPagina(productoStore.paginacion.paginaActual - 1)"
            :disabled="!productoStore.paginacion.tienePaginaAnterior"
            class="btn btn-outline"
          >
            Anterior
          </button>

          <span class="text-gray-700">
            Página {{ productoStore.paginacion.paginaActual }} de {{ productoStore.paginacion.totalPaginas }}
          </span>

          <button
            @click="cambiarPagina(productoStore.paginacion.paginaActual + 1)"
            :disabled="!productoStore.paginacion.tienePaginaSiguiente"
            class="btn btn-outline"
          >
            Siguiente
          </button>
        </div>
      </div>

      <p v-else class="text-center text-gray-500 py-12">
        No se encontraron productos
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useProductoStore } from '@/stores'
import ProductCard from '@/components/ProductCard.vue'
import LoadingSpinner from '@/components/LoadingSpinner.vue'
import Alert from '@/components/Alert.vue'

const route = useRoute()
const router = useRouter()
const productoStore = useProductoStore()

const searchTerm = ref('')
let searchTimeout: number

const cargarProductos = async (pagina: number = 1) => {
  const query = route.query.q as string
  
  if (query) {
    searchTerm.value = query
    await productoStore.buscarProductos(query, pagina)
  } else {
    await productoStore.obtenerProductos(pagina)
  }
}

const buscar = () => {
  if (searchTimeout) clearTimeout(searchTimeout)
  
  searchTimeout = window.setTimeout(() => {
    if (searchTerm.value.trim()) {
      router.push({ query: { q: searchTerm.value } })
    } else {
      router.push({ query: {} })
    }
  }, 500)
}

const cambiarPagina = (pagina: number) => {
  cargarProductos(pagina)
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

watch(() => route.query, () => {
  cargarProductos()
})

onMounted(() => {
  cargarProductos()
})
</script>
