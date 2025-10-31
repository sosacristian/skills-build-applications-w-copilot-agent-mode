<template>
  <div class="container mx-auto px-4 py-8">
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-3xl font-bold">Gestión de Productos</h1>
      <div class="flex gap-4">
        <button
          @click="descargarPlantilla"
          class="btn btn-secondary flex items-center gap-2"
          :disabled="descargando"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 10v6m0 0l-3-3m3 3l3-3m2 8H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          {{ descargando ? 'Descargando...' : 'Descargar Plantilla' }}
        </button>
        <button
          @click="mostrarImportador = true"
          class="btn btn-primary flex items-center gap-2"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" />
          </svg>
          Importar desde Excel
        </button>
      </div>
    </div>

    <!-- Modal de importación -->
    <div v-if="mostrarImportador" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white rounded-lg p-8 max-w-2xl w-full mx-4 max-h-[90vh] overflow-y-auto">
        <div class="flex justify-between items-center mb-6">
          <h2 class="text-2xl font-bold">Importar Productos desde Excel</h2>
          <button @click="cerrarImportador" class="text-gray-500 hover:text-gray-700">
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <div class="mb-6">
          <label class="block text-sm font-medium text-gray-700 mb-2">
            Seleccionar archivo Excel (.xlsx)
          </label>
          <input
            type="file"
            ref="fileInput"
            accept=".xlsx"
            @change="seleccionarArchivo"
            class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-primary-50 file:text-primary-700 hover:file:bg-primary-100"
          />
        </div>

        <div v-if="archivoSeleccionado" class="mb-6 p-4 bg-gray-50 rounded-lg">
          <p class="text-sm text-gray-700">
            <strong>Archivo:</strong> {{ archivoSeleccionado.name }}
          </p>
          <p class="text-sm text-gray-700">
            <strong>Tamaño:</strong> {{ (archivoSeleccionado.size / 1024).toFixed(2) }} KB
          </p>
        </div>

        <Alert v-if="error" :message="error" type="error" @close="error = ''" class="mb-6" />
        <Alert v-if="exito" :message="exito" type="success" @close="exito = ''" class="mb-6" />

        <!-- Resultado de importación -->
        <div v-if="resultadoImportacion" class="mb-6 p-4 bg-blue-50 rounded-lg">
          <h3 class="font-bold mb-2">Resultado de la Importación:</h3>
          <div class="grid grid-cols-3 gap-4 mb-4">
            <div class="text-center">
              <p class="text-2xl font-bold text-gray-700">{{ resultadoImportacion.totalProcesados }}</p>
              <p class="text-sm text-gray-600">Total</p>
            </div>
            <div class="text-center">
              <p class="text-2xl font-bold text-green-600">{{ resultadoImportacion.exitosos }}</p>
              <p class="text-sm text-gray-600">Exitosos</p>
            </div>
            <div class="text-center">
              <p class="text-2xl font-bold text-red-600">{{ resultadoImportacion.fallidos }}</p>
              <p class="text-sm text-gray-600">Fallidos</p>
            </div>
          </div>

          <div v-if="resultadoImportacion.errores && resultadoImportacion.errores.length > 0" class="mt-4">
            <h4 class="font-semibold mb-2 text-red-600">Errores:</h4>
            <div class="max-h-60 overflow-y-auto">
              <table class="w-full text-sm">
                <thead class="bg-gray-100 sticky top-0">
                  <tr>
                    <th class="px-2 py-1 text-left">Fila</th>
                    <th class="px-2 py-1 text-left">Campo</th>
                    <th class="px-2 py-1 text-left">Error</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(err, idx) in resultadoImportacion.errores" :key="idx" class="border-b">
                    <td class="px-2 py-1">{{ err.fila }}</td>
                    <td class="px-2 py-1">{{ err.campo }}</td>
                    <td class="px-2 py-1">{{ err.mensaje }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <div class="flex justify-end gap-4">
          <button @click="cerrarImportador" class="btn btn-secondary">
            Cancelar
          </button>
          <button
            @click="importarArchivo"
            :disabled="!archivoSeleccionado || importando"
            class="btn btn-primary"
          >
            {{ importando ? 'Importando...' : 'Importar' }}
          </button>
        </div>
      </div>
    </div>

    <!-- Lista de productos -->
    <div class="card">
      <div class="mb-4">
        <input
          v-model="busqueda"
          type="text"
          placeholder="Buscar productos..."
          class="input w-full max-w-md"
        />
      </div>

      <div class="overflow-x-auto">
        <table class="w-full">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">SKU</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Nombre</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Precio</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Stock</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Categoría</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Marca</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Estado</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="producto in productos" :key="producto.id">
              <td class="px-6 py-4 whitespace-nowrap text-sm">{{ producto.codigoSKU }}</td>
              <td class="px-6 py-4 text-sm">{{ producto.nombre }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm">
                ${{ producto.precioBase.toLocaleString('es-AR') }}
                <span v-if="producto.porcentajeDescuento > 0" class="text-green-600 text-xs">
                  (-{{ producto.porcentajeDescuento }}%)
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm">
                <span :class="producto.cantidadStock < 10 ? 'text-red-600 font-semibold' : ''">
                  {{ producto.cantidadStock }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-sm">{{ producto.nombreCategoria }}</td>
              <td class="px-6 py-4 whitespace-nowrap text-sm">{{ producto.nombreMarca }}</td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span :class="producto.estaActivo ? 'badge-success' : 'badge-danger'">
                  {{ producto.estaActivo ? 'Activo' : 'Inactivo' }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Paginación -->
      <div class="flex justify-between items-center mt-6">
        <p class="text-sm text-gray-700">
          Mostrando {{ productos.length }} de {{ total }} productos
        </p>
        <div class="flex gap-2">
          <button
            @click="cambiarPagina(paginaActual - 1)"
            :disabled="paginaActual === 1"
            class="btn btn-secondary"
          >
            Anterior
          </button>
          <button
            @click="cambiarPagina(paginaActual + 1)"
            :disabled="paginaActual * 20 >= total"
            class="btn btn-secondary"
          >
            Siguiente
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '@/services/api'
import Alert from '@/components/Alert.vue'

const productos = ref<any[]>([])
const total = ref(0)
const paginaActual = ref(1)
const busqueda = ref('')

const mostrarImportador = ref(false)
const archivoSeleccionado = ref<File | null>(null)
const fileInput = ref<HTMLInputElement>()
const importando = ref(false)
const descargando = ref(false)
const error = ref('')
const exito = ref('')
const resultadoImportacion = ref<any>(null)

const cargarProductos = async () => {
  try {
    const response = await api.get(`/productos?pagina=${paginaActual.value}&tamanoPagina=20`)
    productos.value = response.data.items || []
    total.value = response.data.total || 0
  } catch (err) {
    console.error('Error al cargar productos:', err)
  }
}

const cambiarPagina = (nuevaPagina: number) => {
  paginaActual.value = nuevaPagina
  cargarProductos()
}

const seleccionarArchivo = (event: Event) => {
  const target = event.target as HTMLInputElement
  if (target.files && target.files.length > 0) {
    archivoSeleccionado.value = target.files[0]
    error.value = ''
    exito.value = ''
    resultadoImportacion.value = null
  }
}

const importarArchivo = async () => {
  if (!archivoSeleccionado.value) return

  importando.value = true
  error.value = ''
  exito.value = ''
  resultadoImportacion.value = null

  try {
    const formData = new FormData()
    formData.append('archivo', archivoSeleccionado.value)

    const response = await api.post('/admin/productos/importar', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })

    resultadoImportacion.value = response.data

    if (response.data.exitosos > 0) {
      exito.value = `Se importaron ${response.data.exitosos} productos correctamente`
      await cargarProductos()
    }

    if (response.data.fallidos > 0) {
      error.value = `${response.data.fallidos} productos fallaron. Revise los errores abajo.`
    }
  } catch (err: any) {
    error.value = err.response?.data?.error || 'Error al importar productos'
    console.error('Error al importar:', err)
  } finally {
    importando.value = false
  }
}

const descargarPlantilla = async () => {
  descargando.value = true
  try {
    const response = await api.get('/admin/productos/plantilla', {
      responseType: 'blob'
    })

    const url = window.URL.createObjectURL(new Blob([response.data]))
    const link = document.createElement('a')
    link.href = url
    link.setAttribute('download', 'plantilla_productos.xlsx')
    document.body.appendChild(link)
    link.click()
    link.remove()
    window.URL.revokeObjectURL(url)
  } catch (err) {
    error.value = 'Error al descargar la plantilla'
    console.error('Error:', err)
  } finally {
    descargando.value = false
  }
}

const cerrarImportador = () => {
  mostrarImportador.value = false
  archivoSeleccionado.value = null
  if (fileInput.value) {
    fileInput.value.value = ''
  }
  error.value = ''
  exito.value = ''
  resultadoImportacion.value = null
}

onMounted(() => {
  cargarProductos()
})
</script>

<style scoped>
.badge-success {
  @apply inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800;
}

.badge-danger {
  @apply inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-red-100 text-red-800;
}
</style>
