import api from './api'
import type { 
  LoginDto, 
  RegistrarUsuarioDto, 
  AuthResponse, 
  Usuario 
} from '@/types'

export const authService = {
  /**
   * Registrar un nuevo usuario
   */
  async registrar(datos: RegistrarUsuarioDto): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/usuarios/registrar', datos)
    return response.data
  },

  /**
   * Iniciar sesión
   */
  async login(datos: LoginDto): Promise<AuthResponse> {
    const response = await api.post<AuthResponse>('/usuarios/login', datos)
    return response.data
  },

  /**
   * Obtener usuario por ID
   */
  async obtenerUsuario(id: number): Promise<Usuario> {
    const response = await api.get<Usuario>(`/usuarios/${id}`)
    return response.data
  },

  /**
   * Verificar si un email existe
   */
  async emailExiste(email: string): Promise<boolean> {
    const response = await api.get<{ existe: boolean }>(`/usuarios/email-existe/${email}`)
    return response.data.existe
  },

  /**
   * Solicitar recuperación de contraseña
   */
  async solicitarRecuperacionPassword(email: string): Promise<void> {
    await api.post('/usuarios/recuperar-password', JSON.stringify(email), {
      headers: { 'Content-Type': 'application/json' }
    })
  },

  /**
   * Restablecer contraseña con token
   */
  async restablecerPassword(token: string, passwordNueva: string): Promise<void> {
    await api.post('/usuarios/restablecer-password', { token, passwordNueva })
  },

  /**
   * Verificar email con token
   */
  async verificarEmail(token: string): Promise<void> {
    await api.get(`/usuarios/verificar-email/${token}`)
  }
}
