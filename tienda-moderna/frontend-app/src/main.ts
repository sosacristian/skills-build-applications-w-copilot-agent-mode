import { createApp } from 'vue'
import { createPinia } from 'pinia'
import router from './router'
import App from './App.vue'
import './style.css'

// Crear aplicación
const app = createApp(App)

// Crear Pinia
const pinia = createPinia()

// Usar plugins
app.use(pinia)
app.use(router)

// Cargar estado inicial de los stores
import { useAuthStore, useCarritoStore } from './stores'

const authStore = useAuthStore()
const carritoStore = useCarritoStore()

// Cargar datos de localStorage
authStore.cargarUsuarioDesdeStorage()
carritoStore.cargarDesdeStorage()

// Montar aplicación
app.mount('#app')
