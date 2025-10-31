<template>
  <nav class="bg-white shadow-md">
    <div class="container mx-auto px-4">
      <div class="flex justify-between items-center h-16">
        <!-- Logo -->
        <RouterLink to="/" class="flex items-center space-x-2">
          <span class="text-2xl font-bold text-primary-600">Tienda Moderna</span>
        </RouterLink>

        <!-- Barra de búsqueda (desktop) -->
        <div class="hidden md:block flex-1 max-w-xl mx-8">
          <div class="relative">
            <input
              v-model="searchQuery"
              @keyup.enter="buscar"
              type="text"
              placeholder="Buscar productos..."
              class="input pr-10"
            />
            <button
              @click="buscar"
              class="absolute right-2 top-1/2 -translate-y-1/2 text-gray-400 hover:text-primary-600"
            >
              <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </button>
          </div>
        </div>

        <!-- Navegación -->
        <div class="flex items-center space-x-6">
          <RouterLink
            to="/productos"
            class="hidden md:block text-gray-700 hover:text-primary-600 font-medium"
          >
            Productos
          </RouterLink>

          <!-- Carrito -->
          <RouterLink to="/carrito" class="relative">
            <svg class="w-6 h-6 text-gray-700 hover:text-primary-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
            </svg>
            <span
              v-if="carritoStore.cantidadTotal > 0"
              class="absolute -top-2 -right-2 bg-red-500 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center"
            >
              {{ carritoStore.cantidadTotal }}
            </span>
          </RouterLink>

          <!-- Usuario -->
          <div v-if="authStore.estaAutenticado" class="relative">
            <button
              @click="toggleUserMenu"
              class="flex items-center space-x-2 text-gray-700 hover:text-primary-600"
            >
              <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
              </svg>
              <span class="hidden md:block">{{ authStore.usuario?.nombreCompleto }}</span>
            </button>

            <!-- Menú desplegable -->
            <Transition name="dropdown">
              <div
                v-if="showUserMenu"
                class="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg py-2 z-50"
              >
                <RouterLink
                  v-if="authStore.usuario?.rol === 'Administrador'"
                  to="/admin"
                  class="block px-4 py-2 text-purple-600 hover:bg-purple-50 font-semibold"
                  @click="showUserMenu = false"
                >
                  🔧 Panel Admin
                </RouterLink>
                <RouterLink
                  to="/perfil"
                  class="block px-4 py-2 text-gray-700 hover:bg-gray-100"
                  @click="showUserMenu = false"
                >
                  Mi Perfil
                </RouterLink>
                <RouterLink
                  to="/mis-ordenes"
                  class="block px-4 py-2 text-gray-700 hover:bg-gray-100"
                  @click="showUserMenu = false"
                >
                  Mis Órdenes
                </RouterLink>
                <button
                  @click="cerrarSesion"
                  class="block w-full text-left px-4 py-2 text-red-600 hover:bg-gray-100"
                >
                  Cerrar Sesión
                </button>
              </div>
            </Transition>
          </div>

          <!-- Login/Register -->
          <div v-else class="flex items-center space-x-4">
            <RouterLink
              to="/login"
              class="text-gray-700 hover:text-primary-600 font-medium"
            >
              Ingresar
            </RouterLink>
            <RouterLink
              to="/register"
              class="btn btn-primary"
            >
              Registrarse
            </RouterLink>
          </div>

          <!-- Menú móvil -->
          <button
            @click="toggleMobileMenu"
            class="md:hidden text-gray-700"
          >
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
        </div>
      </div>

      <!-- Menú móvil -->
      <Transition name="slide">
        <div v-if="showMobileMenu" class="md:hidden py-4 border-t">
          <div class="mb-4">
            <input
              v-model="searchQuery"
              @keyup.enter="buscar"
              type="text"
              placeholder="Buscar productos..."
              class="input"
            />
          </div>
          <RouterLink
            to="/productos"
            class="block py-2 text-gray-700 hover:text-primary-600"
            @click="showMobileMenu = false"
          >
            Productos
          </RouterLink>
        </div>
      </Transition>
    </div>
  </nav>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore, useCarritoStore } from '@/stores'

const router = useRouter()
const authStore = useAuthStore()
const carritoStore = useCarritoStore()

const searchQuery = ref('')
const showUserMenu = ref(false)
const showMobileMenu = ref(false)

const buscar = () => {
  if (searchQuery.value.trim()) {
    router.push({ name: 'productos', query: { q: searchQuery.value } })
    showMobileMenu.value = false
  }
}

const toggleUserMenu = () => {
  showUserMenu.value = !showUserMenu.value
}

const toggleMobileMenu = () => {
  showMobileMenu.value = !showMobileMenu.value
}

const cerrarSesion = () => {
  authStore.cerrarSesion()
  showUserMenu.value = false
  router.push({ name: 'home' })
}
</script>

<style scoped>
.dropdown-enter-active,
.dropdown-leave-active {
  transition: all 0.2s ease;
}

.dropdown-enter-from,
.dropdown-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}

.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease;
}

.slide-enter-from,
.slide-leave-to {
  opacity: 0;
  transform: translateY(-20px);
}
</style>
