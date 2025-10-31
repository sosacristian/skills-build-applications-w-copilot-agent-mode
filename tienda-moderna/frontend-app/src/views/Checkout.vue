<template>
  <div class="max-w-4xl mx-auto">
    <h1 class="text-4xl font-bold mb-8">Finalizar Compra</h1>

    <Alert
      v-if="ordenStore.error"
      :message="ordenStore.error"
      type="error"
      @close="ordenStore.limpiarError()"
    />

    <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
      <!-- Formulario -->
      <div class="lg:col-span-2">
        <form @submit.prevent="realizarCompra" class="card space-y-4">
          <h2 class="text-2xl font-bold mb-4">Datos de Envío</h2>

          <div>
            <label class="block text-sm font-medium mb-1">Dirección</label>
            <input v-model="form.direccion" required class="input" />
          </div>

          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium mb-1">Ciudad</label>
              <input v-model="form.ciudad" required class="input" />
            </div>
            <div>
              <label class="block text-sm font-medium mb-1">Código Postal</label>
              <input v-model="form.codigoPostal" required class="input" />
            </div>
          </div>

          <div>
            <label class="block text-sm font-medium mb-1">Teléfono</label>
            <input v-model="form.telefono" type="tel" required class="input" />
          </div>

          <button
            type="submit"
            :disabled="ordenStore.cargando"
            class="btn btn-primary w-full"
          >
            {{ ordenStore.cargando ? 'Procesando...' : 'Confirmar Compra' }}
          </button>
        </form>
      </div>

      <!-- Resumen de la orden -->
      <div class="lg:col-span-1">
        <div class="card sticky top-4">
          <h2 class="text-2xl font-bold mb-4">Resumen</h2>
          
          <div class="space-y-2 mb-4">
            <div
              v-for="item in carritoStore.items"
              :key="item.producto.id"
              class="flex justify-between text-sm"
            >
              <span>{{ item.producto.nombre }} (x{{ item.cantidad }})</span>
              <span>${{ (item.producto.precioFinal * item.cantidad).toLocaleString('es-AR') }}</span>
            </div>
          </div>

          <div class="border-t pt-4 space-y-2">
            <div class="flex justify-between">
              <span>Subtotal:</span>
              <span>${{ carritoStore.subtotal.toLocaleString('es-AR') }}</span>
            </div>
            <div class="flex justify-between font-bold text-lg">
              <span>Total:</span>
              <span class="text-primary-600">${{ carritoStore.subtotal.toLocaleString('es-AR') }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useCarritoStore, useOrdenStore, useAuthStore } from '@/stores'
import Alert from '@/components/Alert.vue'

const router = useRouter()
const carritoStore = useCarritoStore()
const ordenStore = useOrdenStore()
const authStore = useAuthStore()

const form = reactive({
  direccion: '',
  ciudad: '',
  codigoPostal: '',
  telefono: ''
})

const realizarCompra = async () => {
  if (!authStore.usuario) return

  const ordenDto = {
    usuarioId: authStore.usuario.id,
    direccionEnvio: `${form.direccion}, ${form.ciudad}, ${form.codigoPostal}`,
    detalles: carritoStore.items.map((item) => ({
      productoId: item.producto.id,
      cantidad: item.cantidad,
      precioUnitario: item.producto.precioFinal
    }))
  }

  const orden = await ordenStore.crearOrden(ordenDto)

  if (orden) {
    carritoStore.vaciarCarrito()
    router.push({ name: 'orden-detalle', params: { id: orden.id } })
  }
}
</script>
