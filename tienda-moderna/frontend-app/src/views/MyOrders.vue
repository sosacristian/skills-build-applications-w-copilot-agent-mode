<template>
  <div class="max-w-4xl mx-auto">
    <h1 class="text-4xl font-bold mb-8">Mis Órdenes</h1>

    <LoadingSpinner v-if="ordenStore.cargando" message="Cargando órdenes..." />

    <Alert
      v-else-if="ordenStore.error"
      :message="ordenStore.error"
      type="error"
      @close="ordenStore.limpiarError()"
    />

    <div v-else-if="ordenStore.ordenes.length === 0" class="card text-center py-12">
      <p class="text-gray-500 mb-4">No tienes órdenes todavía</p>
      <RouterLink to="/productos" class="btn btn-primary">
        Comenzar a Comprar
      </RouterLink>
    </div>

    <div v-else class="space-y-4">
      <div
        v-for="orden in ordenStore.ordenes"
        :key="orden.id"
        class="card cursor-pointer hover:shadow-lg transition-shadow"
        @click="router.push({ name: 'orden-detalle', params: { id: orden.id } })"
      >
        <div class="flex justify-between items-start mb-4">
          <div>
            <h3 class="font-bold text-lg">Orden #{{ orden.numeroOrden }}</h3>
            <p class="text-sm text-gray-600">{{ formatearFecha(orden.fechaCreacion) }}</p>
          </div>
          <span :class="badgeEstado(orden.estado)">
            {{ orden.estado }}
          </span>
        </div>

        <div class="text-gray-700">
          <p class="text-sm mb-2">{{ orden.detalles.length }} producto(s)</p>
          <p class="text-2xl font-bold text-primary-600">
            ${{ orden.total.toLocaleString('es-AR') }}
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { useOrdenStore } from '@/stores'
import LoadingSpinner from '@/components/LoadingSpinner.vue'
import Alert from '@/components/Alert.vue'

const router = useRouter()
const ordenStore = useOrdenStore()

const formatearFecha = (fecha: string) => {
  return new Date(fecha).toLocaleDateString('es-AR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
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

onMounted(() => {
  ordenStore.obtenerMisOrdenes()
})
</script>
