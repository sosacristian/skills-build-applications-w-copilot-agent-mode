<template>
  <div class="max-w-md mx-auto">
    <div class="card">
      <h1 class="text-3xl font-bold mb-6 text-center">Registrarse</h1>

      <Alert
        v-if="authStore.error"
        :message="authStore.error"
        type="error"
        @close="authStore.limpiarError()"
      />

      <form @submit.prevent="handleSubmit" class="space-y-4">
        <div>
          <label for="nombre" class="block text-sm font-medium text-gray-700 mb-1">
            Nombre Completo
          </label>
          <input
            id="nombre"
            v-model="form.nombreCompleto"
            type="text"
            required
            minlength="3"
            class="input"
            placeholder="Juan Pérez"
          />
        </div>

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
            minlength="6"
            class="input"
            placeholder="••••••••"
          />
        </div>

        <div>
          <label for="confirmPassword" class="block text-sm font-medium text-gray-700 mb-1">
            Confirmar Contraseña
          </label>
          <input
            id="confirmPassword"
            v-model="confirmPassword"
            type="password"
            required
            class="input"
            placeholder="••••••••"
          />
          <p v-if="passwordMismatch" class="text-red-600 text-sm mt-1">
            Las contraseñas no coinciden
          </p>
        </div>

        <button
          type="submit"
          :disabled="authStore.cargando || passwordMismatch"
          class="btn btn-primary w-full"
        >
          {{ authStore.cargando ? 'Registrando...' : 'Registrarse' }}
        </button>
      </form>

      <div class="mt-6 text-center text-sm">
        <p class="text-gray-600">
          ¿Ya tienes cuenta?
          <RouterLink to="/login" class="text-primary-600 hover:text-primary-700 font-medium">
            Inicia sesión aquí
          </RouterLink>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, computed } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useAuthStore } from '@/stores'
import Alert from '@/components/Alert.vue'

const router = useRouter()
const authStore = useAuthStore()

const form = reactive({
  nombreCompleto: '',
  email: '',
  password: ''
})

const confirmPassword = ref('')

const passwordMismatch = computed(() => {
  return confirmPassword.value && form.password !== confirmPassword.value
})

const handleSubmit = async () => {
  if (passwordMismatch.value) return

  const success = await authStore.registrar(form)
  
  if (success) {
    router.push({ name: 'home' })
  }
}
</script>
