<template>
  <div>
    <!-- Hero Section -->
    <section class="bg-gradient-to-r from-primary-600 to-primary-800 text-white py-20 mb-12 rounded-lg">
      <div class="text-center">
        <h1 class="text-5xl font-bold mb-4">Bienvenido a Tienda Moderna</h1>
        <p class="text-xl mb-8">Descubre las mejores marcas y productos de calidad</p>
        <RouterLink to="/productos" class="btn bg-white text-primary-600 hover:bg-gray-100 text-lg px-8 py-3">
          Ver Productos
        </RouterLink>
      </div>
    </section>

    <!-- Productos Destacados -->
    <section class="mb-16">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-3xl font-bold">Productos Destacados</h2>
        <RouterLink to="/productos" class="text-primary-600 hover:text-primary-700 font-medium">
          Ver todos →
        </RouterLink>
      </div>

      <LoadingSpinner v-if="productoStore.cargando" message="Cargando productos..." />
      
      <div v-else-if="productoStore.destacados.length > 0" class="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
        <ProductCard
          v-for="producto in productoStore.destacados"
          :key="producto.id"
          :producto="producto"
        />
      </div>

      <p v-else class="text-center text-gray-500 py-8">
        No hay productos destacados disponibles
      </p>
    </section>

    <!-- Ofertas -->
    <section class="mb-16">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-3xl font-bold">Ofertas Especiales</h2>
        <RouterLink to="/productos?ofertas=true" class="text-primary-600 hover:text-primary-700 font-medium">
          Ver todas →
        </RouterLink>
      </div>

      <div v-if="productoStore.ofertas.length > 0" class="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
        <ProductCard
          v-for="producto in productoStore.ofertas"
          :key="producto.id"
          :producto="producto"
        />
      </div>

      <p v-else class="text-center text-gray-500 py-8">
        No hay ofertas disponibles en este momento
      </p>
    </section>

    <!-- Categorías -->
    <section>
      <h2 class="text-3xl font-bold mb-6">Comprar por Categoría</h2>
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div
          v-for="categoria in categorias"
          :key="categoria.nombre"
          class="card text-center hover:shadow-lg transition-shadow cursor-pointer"
          @click="() => router.push({ name: 'productos', query: { categoria: categoria.nombre } })"
        >
          <div class="text-4xl mb-2">{{ categoria.icono }}</div>
          <h3 class="font-semibold">{{ categoria.nombre }}</h3>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useProductoStore } from '@/stores'
import ProductCard from '@/components/ProductCard.vue'
import LoadingSpinner from '@/components/LoadingSpinner.vue'

const router = useRouter()
const productoStore = useProductoStore()

const categorias = [
  { nombre: 'Remeras', icono: '👕' },
  { nombre: 'Pantalones', icono: '👖' },
  { nombre: 'Camperas', icono: '🧥' },
  { nombre: 'Zapatillas', icono: '👟' }
]

onMounted(async () => {
  await Promise.all([
    productoStore.obtenerDestacados(8),
    productoStore.obtenerOfertas(4)
  ])
})
</script>
