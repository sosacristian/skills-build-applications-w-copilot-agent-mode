import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '@/services/auth.service'
import type { Usuario, LoginDto, RegistrarUsuarioDto, AuthResponse } from '@/types'
import { jwtDecode } from 'jwt-decode'

export const useAuthStore = defineStore('auth', () => {
  // State
  const usuario = ref<Usuario | null>(null)
  const token = ref<string | null>(null)
  const cargando = ref(false)
  const error = ref<string | null>(null)

  // Getters
  const estaAutenticado = computed(() => !!token.value && !!usuario.value)
  const esAdmin = computed(() => 
    usuario.value?.rol === 'Administrador' || usuario.value?.rol === 'SuperAdministrador'
  )

  // Actions
  const cargarUsuarioDesdeStorage = () => {
    const tokenGuardado = localStorage.getItem('token')
    const usuarioGuardado = localStorage.getItem('usuario')

    if (tokenGuardado && usuarioGuardado) {
      try {
        // Verificar si el token ha expirado
        const decoded: any = jwtDecode(tokenGuardado)
        const ahora = Date.now() / 1000
        
        if (decoded.exp && decoded.exp > ahora) {
          token.value = tokenGuardado
          usuario.value = JSON.parse(usuarioGuardado)
        } else {
          // Token expirado
          cerrarSesion()
        }
      } catch (err) {
        cerrarSesion()
      }
    }
  }

  const registrar = async (datos: RegistrarUsuarioDto): Promise<boolean> => {
    cargando.value = true
    error.value = null

    try {
      const respuesta: AuthResponse = await authService.registrar(datos)
      token.value = respuesta.token
      usuario.value = respuesta.usuario

      // Guardar en localStorage
      localStorage.setItem('token', respuesta.token)
      localStorage.setItem('usuario', JSON.stringify(respuesta.usuario))

      return true
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al registrar usuario'
      return false
    } finally {
      cargando.value = false
    }
  }

  const iniciarSesion = async (datos: LoginDto): Promise<boolean> => {
    cargando.value = true
    error.value = null

    try {
      const respuesta: AuthResponse = await authService.login(datos)
      token.value = respuesta.token
      usuario.value = respuesta.usuario

      // Guardar en localStorage
      localStorage.setItem('token', respuesta.token)
      localStorage.setItem('usuario', JSON.stringify(respuesta.usuario))

      return true
    } catch (err: any) {
      error.value = err.response?.data?.error || 'Error al iniciar sesión'
      return false
    } finally {
      cargando.value = false
    }
  }

  const cerrarSesion = () => {
    usuario.value = null
    token.value = null
    error.value = null
    localStorage.removeItem('token')
    localStorage.removeItem('usuario')
  }

  const limpiarError = () => {
    error.value = null
  }

  return {
    // State
    usuario,
    token,
    cargando,
    error,
    // Getters
    estaAutenticado,
    esAdmin,
    // Actions
    cargarUsuarioDesdeStorage,
    registrar,
    iniciarSesion,
    cerrarSesion,
    limpiarError
  }
})
