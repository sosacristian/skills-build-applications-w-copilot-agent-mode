<template>
  <div class="max-w-2xl mx-auto">
    <div class="card">
      <h1 class="text-4xl font-bold mb-8 text-center">Mi Perfil</h1>

      <div v-if="authStore.usuario" class="space-y-6">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Nombre</label>
          <p class="text-lg">{{ authStore.usuario.nombre }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Email</label>
          <p class="text-lg">{{ authStore.usuario.email }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Rol</label>
          <span class="badge badge-info">{{ authStore.usuario.rol }}</span>
        </div>

        <div class="pt-6 border-t">
          <RouterLink to="/mis-ordenes" class="btn btn-outline w-full mb-2">
            Ver Mis Órdenes
          </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useAuthStore } from '@/stores'

const authStore = useAuthStore()
</script>
