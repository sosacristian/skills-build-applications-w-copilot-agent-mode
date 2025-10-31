<template>
  <div class="max-w-md mx-auto">
    <div class="card">
      <h1 class="text-3xl font-bold mb-6 text-center">Iniciar Sesión</h1>

      <Alert
        v-if="authStore.error"
        :message="authStore.error"
        type="error"
        @close="authStore.limpiarError()"
      />

      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label for="email" class="block text-sm font-medium text-gray-700 mb-1">
            Email
          </label>
          <input
            id="email"
            v-model="form.email"
            type="email"
            required
            class="input"
            placeholder="tu@email.com"
          />
        </div>

        <div>
          <label for="password" class="block text-sm font-medium text-gray-700 mb-1">
            Contraseña
          </label>
          <input
            id="password"
            v-model="form.password"
            type="password"
            required
            class="input"
            placeholder="••••••••"
          />
        </div>

        <button
          type="submit"
          :disabled="authStore.cargando"
          class="btn btn-primary w-full"
        >
          {{ authStore.cargando ? 'Ingresando...' : 'Ingresar' }}
        </button>
      </form>

      <div class="mt-6 text-center text-sm">
        <p class="text-gray-600">
          ¿No tienes cuenta?
          <RouterLink to="/register" class="text-primary-600 hover:text-primary-700 font-medium">
            Regístrate aquí
          </RouterLink>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { RouterLink, useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores'
import Alert from '@/components/Alert.vue'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const form = reactive({
  email: '',
  password: ''
})

const handleSubmit = async () => {
  const success = await authStore.iniciarSesion(form)
  
  if (success) {
    const redirect = route.query.redirect as string
    router.push(redirect || { name: 'home' })
  }
}
</script>
