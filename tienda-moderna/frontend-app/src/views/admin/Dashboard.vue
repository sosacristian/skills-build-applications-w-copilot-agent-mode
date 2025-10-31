<template>
  <div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold mb-8">Panel de Administración</h1>

    <!-- Estadísticas -->
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
      <div class="card bg-gradient-to-br from-blue-500 to-blue-600 text-white">
        <h3 class="text-lg font-semibold mb-2">Productos</h3>
        <p class="text-4xl font-bold">{{ stats.productos }}</p>
        <p class="text-sm opacity-90 mt-2">Total en catálogo</p>
      </div>

      <div class="card bg-gradient-to-br from-green-500 to-green-600 text-white">
        <h3 class="text-lg font-semibold mb-2">Categorías</h3>
        <p class="text-4xl font-bold">{{ stats.categorias }}</p>
        <p class="text-sm opacity-90 mt-2">Activas</p>
      </div>

      <div class="card bg-gradient-to-br from-purple-500 to-purple-600 text-white">
        <h3 class="text-lg font-semibold mb-2">Marcas</h3>
        <p class="text-4xl font-bold">{{ stats.marcas }}</p>
        <p class="text-sm opacity-90 mt-2">Registradas</p>
      </div>

      <div class="card bg-gradient-to-br from-orange-500 to-orange-600 text-white">
        <h3 class="text-lg font-semibold mb-2">Órdenes</h3>
        <p class="text-4xl font-bold">{{ stats.ordenes }}</p>
        <p class="text-sm opacity-90 mt-2">Este mes</p>
      </div>
    </div>

    <!-- Acciones rápidas -->
    <div class="card mb-8">
      <h2 class="text-2xl font-bold mb-4">Acciones Rápidas</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
        <RouterLink
          to="/admin/products"
          class="btn btn-primary flex items-center justify-center gap-2"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" />
          </svg>
          Gestionar Productos
        </RouterLink>

        <RouterLink
          to="/admin/categories"
          class="btn btn-secondary flex items-center justify-center gap-2"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z" />
          </svg>
          Gestionar Categorías
        </RouterLink>

        <RouterLink
          to="/admin/orders"
          class="btn bg-green-600 hover:bg-green-700 text-white flex items-center justify-center gap-2"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
          </svg>
          Ver Órdenes
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { RouterLink } from 'vue-router'
import api from '@/services/api'

const stats = ref({
  productos: 0,
  categorias: 0,
  marcas: 0,
  ordenes: 0
})

onMounted(async () => {
  try {
    // Obtener estadísticas
    const [productos, categorias, marcas] = await Promise.all([
      api.get('/productos?pagina=1&tamanoPagina=1'),
      api.get('/categorias'),
      api.get('/marcas')
    ])

    stats.value = {
      productos: productos.data.total || 0,
      categorias: categorias.data.length || 0,
      marcas: marcas.data.length || 0,
      ordenes: 0 // TODO: implementar endpoint de órdenes
    }
  } catch (error) {
    console.error('Error al cargar estadísticas:', error)
  }
})
</script>
