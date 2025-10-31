<template>
  <div class="max-w-4xl mx-auto">
    <LoadingSpinner v-if="ordenStore.cargando" message="Cargando orden..." />

    <Alert
      v-else-if="ordenStore.error"
      :message="ordenStore.error"
      type="error"
      @close="() => router.push({ name: 'mis-ordenes' })"
    />

    <div v-else-if="orden">
      <div class="flex justify-between items-start mb-8">
        <div>
          <h1 class="text-4xl font-bold">Orden #{{ orden.numeroOrden }}</h1>
          <p class="text-gray-600 mt-2">{{ formatearFecha(orden.fechaCreacion) }}</p>
        </div>
        <span :class="badgeEstado(orden.estado)">
          {{ orden.estado }}
        </span>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Detalles de la orden -->
        <div class="lg:col-span-2">
          <div class="card mb-6">
            <h2 class="text-2xl font-bold mb-4">Productos</h2>
            <div class="space-y-4">
              <div
                v-for="detalle in orden.detalles"
                :key="detalle.id"
                class="flex gap-4 border-b pb-4 last:border-b-0"
              >
                <div class="flex-1">
                  <h3 class="font-semibold">{{ detalle.nombreProducto }}</h3>
                  <p class="text-sm text-gray-600">Cantidad: {{ detalle.cantidad }}</p>
                  <p class="text-primary-600 font-bold mt-1">
                    ${{ detalle.precioUnitario.toLocaleString('es-AR') }}
                  </p>
                </div>
                <div class="text-right">
                  <p class="font-bold">
                    ${{ detalle.subtotal.toLocaleString('es-AR') }}
                  </p>
                </div>
              </div>
            </div>
          </div>

          <div class="card">
            <h2 class="text-2xl font-bold mb-4">Información de Envío</h2>
            <p class="text-gray-700">{{ orden.direccionEnvio }}</p>
          </div>
        </div>

        <!-- Resumen -->
        <div class="lg:col-span-1">
          <div class="card sticky top-4">
            <h2 class="text-2xl font-bold mb-4">Resumen</h2>
            
            <div class="space-y-2 mb-4">
              <div class="flex justify-between">
                <span>Subtotal:</span>
                <span>${{ orden.subtotal.toLocaleString('es-AR') }}</span>
              </div>
              <div class="flex justify-between">
                <span>Descuentos:</span>
                <span>-${{ orden.descuentos.toLocaleString('es-AR') }}</span>
              </div>
              <div class="border-t pt-2 flex justify-between font-bold text-lg">
                <span>Total:</span>
                <span class="text-primary-600">${{ orden.total.toLocaleString('es-AR') }}</span>
              </div>
            </div>

            <button
              v-if="orden.estado === 'Pendiente'"
              @click="cancelar"
              :disabled="ordenStore.cargando"
              class="btn bg-red-600 text-white hover:bg-red-700 w-full"
            >
              Cancelar Orden
            </button>

            <RouterLink to="/mis-ordenes" class="btn btn-outline w-full mt-2">
              Volver a Mis Órdenes
            </RouterLink>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { RouterLink, useRoute, useRouter } from 'vue-router'
import { useOrdenStore } from '@/stores'
import LoadingSpinner from '@/components/LoadingSpinner.vue'
import Alert from '@/components/Alert.vue'

const route = useRoute()
const router = useRouter()
const ordenStore = useOrdenStore()

const orden = computed(() => ordenStore.ordenActual)

const formatearFecha = (fecha: string) => {
  return new Date(fecha).toLocaleDateString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const badgeEstado = (estado: string) => {
  const clases: Record<string, string> = {
    Pendiente: 'badge badge-warning',
    Confirmada: 'badge badge-info',
    EnProceso: 'badge badge-info',
    Enviada: 'badge badge-info',
    Entregada: 'badge badge-success',
    Cancelada: 'badge badge-danger'
  }
  return clases[estado] || 'badge'
}

const cancelar = async () => {
  if (!orden.value) return
  
  const confirmado = confirm('¿Estás seguro de que deseas cancelar esta orden?')
  if (confirmado) {
    const exito = await ordenStore.cancelarOrden(orden.value.id)
    if (exito) {
      await ordenStore.obtenerOrdenPorId(orden.value.id)
    }
  }
}

onMounted(() => {
  const id = Number(route.params.id)
  ordenStore.obtenerOrdenPorId(id)
})
</script>
