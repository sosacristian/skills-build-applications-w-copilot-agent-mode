import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores'

// Lazy loading de componentes
const Home = () => import('@/views/Home.vue')
const Login = () => import('@/views/Login.vue')
const Register = () => import('@/views/Register.vue')
const ProductList = () => import('@/views/ProductList.vue')
const ProductDetail = () => import('@/views/ProductDetail.vue')
const Cart = () => import('@/views/Cart.vue')
const Checkout = () => import('@/views/Checkout.vue')
const MyOrders = () => import('@/views/MyOrders.vue')
const OrderDetail = () => import('@/views/OrderDetail.vue')
const Profile = () => import('@/views/Profile.vue')
const NotFound = () => import('@/views/NotFound.vue')

// Admin views
const AdminDashboard = () => import('@/views/admin/Dashboard.vue')
const AdminProducts = () => import('@/views/admin/Products.vue')

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'home',
    component: Home,
    meta: { title: 'Inicio' }
  },
  {
    path: '/login',
    name: 'login',
    component: Login,
    meta: { title: 'Iniciar Sesión', requiresGuest: true }
  },
  {
    path: '/register',
    name: 'register',
    component: Register,
    meta: { title: 'Registrarse', requiresGuest: true }
  },
  {
    path: '/productos',
    name: 'productos',
    component: ProductList,
    meta: { title: 'Productos' }
  },
  {
    path: '/productos/:id',
    name: 'producto-detalle',
    component: ProductDetail,
    meta: { title: 'Detalle del Producto' }
  },
  {
    path: '/carrito',
    name: 'carrito',
    component: Cart,
    meta: { title: 'Carrito de Compras' }
  },
  {
    path: '/checkout',
    name: 'checkout',
    component: Checkout,
    meta: { title: 'Finalizar Compra', requiresAuth: true }
  },
  {
    path: '/mis-ordenes',
    name: 'mis-ordenes',
    component: MyOrders,
    meta: { title: 'Mis Órdenes', requiresAuth: true }
  },
  {
    path: '/ordenes/:id',
    name: 'orden-detalle',
    component: OrderDetail,
    meta: { title: 'Detalle de Orden', requiresAuth: true }
  },
  {
    path: '/perfil',
    name: 'perfil',
    component: Profile,
    meta: { title: 'Mi Perfil', requiresAuth: true }
  },
  // Rutas de administración
  {
    path: '/admin',
    name: 'admin',
    component: AdminDashboard,
    meta: { title: 'Panel de Administración', requiresAuth: true, requiresAdmin: true }
  },
  {
    path: '/admin/products',
    name: 'admin-products',
    component: AdminProducts,
    meta: { title: 'Gestión de Productos', requiresAuth: true, requiresAdmin: true }
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'not-found',
    component: NotFound,
    meta: { title: 'Página No Encontrada' }
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      return { top: 0 }
    }
  }
})

// Navigation Guards
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  // Actualizar título de la página
  document.title = `${to.meta.title || 'Tienda'} - Tienda Moderna`

  // Verificar autenticación
  if (to.meta.requiresAuth && !authStore.estaAutenticado) {
    next({
      name: 'login',
      query: { redirect: to.fullPath }
    })
  } 
  // Verificar si requiere ser admin
  else if (to.meta.requiresAdmin) {
    if (!authStore.estaAutenticado) {
      next({
        name: 'login',
        query: { redirect: to.fullPath }
      })
    } else if (authStore.usuario?.rol !== 'Administrador') {
      // Si no es administrador, redirigir a home
      next({ name: 'home' })
    } else {
      next()
    }
  }
  // Verificar si requiere ser invitado (no autenticado)
  else if (to.meta.requiresGuest && authStore.estaAutenticado) {
    next({ name: 'home' })
  } 
  else {
    next()
  }
})

export default router
