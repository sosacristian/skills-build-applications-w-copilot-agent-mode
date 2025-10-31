# Tienda Moderna - Frontend

Frontend de la aplicación Tienda Moderna construido con Vue 3, TypeScript, Vite y TailwindCSS.

## 🚀 Tecnologías

- **Vue 3.4.21** - Framework progresivo de JavaScript
- **TypeScript 5.4.2** - Tipado estático
- **Vite 5.1.6** - Build tool y dev server
- **Pinia 2.1.7** - State management
- **Vue Router 4.3.0** - Routing
- **Axios 1.6.7** - Cliente HTTP
- **TailwindCSS 3.4.1** - Framework CSS
- **JWT Decode 4.0.0** - Decodificación de tokens JWT

## 📦 Instalación

### Prerequisitos

- Node.js 18+ 
- npm o pnpm

### Pasos

1. Instalar dependencias:
```bash
npm install
```

2. Configurar variables de entorno (crear `.env`):
```env
VITE_API_URL=http://localhost:5000/api
```

3. Iniciar servidor de desarrollo:
```bash
npm run dev
```

La aplicación estará disponible en `http://localhost:5173`

## 📁 Estructura del Proyecto

```
src/
├── assets/          # Recursos estáticos
├── components/      # Componentes reutilizables
│   ├── Navbar.vue
│   ├── Footer.vue
│   ├── ProductCard.vue
│   ├── LoadingSpinner.vue
│   └── Alert.vue
├── router/          # Configuración de rutas
│   └── index.ts
├── services/        # Servicios API
│   ├── api.ts
│   ├── auth.service.ts
│   ├── producto.service.ts
│   └── orden.service.ts
├── stores/          # Stores de Pinia
│   ├── auth.ts
│   ├── carrito.ts
│   ├── producto.ts
│   └── orden.ts
├── types/           # Definiciones TypeScript
│   └── index.ts
├── views/           # Vistas/Páginas
│   ├── Home.vue
│   ├── Login.vue
│   ├── Register.vue
│   ├── ProductList.vue
│   ├── ProductDetail.vue
│   ├── Cart.vue
│   ├── Checkout.vue
│   ├── MyOrders.vue
│   ├── OrderDetail.vue
│   ├── Profile.vue
│   └── NotFound.vue
├── App.vue          # Componente raíz
├── main.ts          # Punto de entrada
└── style.css        # Estilos globales
```

## 🔑 Funcionalidades

### Autenticación
- Registro de usuarios
- Inicio de sesión con JWT
- Gestión de sesión persistente (localStorage)
- Guards de navegación

### Productos
- Listado de productos con paginación
- Búsqueda de productos
- Filtros por categoría
- Detalle de producto con variantes
- Productos destacados y ofertas

### Carrito de Compras
- Agregar/eliminar productos
- Actualizar cantidades
- Persistencia en localStorage
- Cálculo automático de totales y descuentos

### Órdenes
- Proceso de checkout
- Historial de órdenes
- Detalle de orden
- Cancelación de órdenes pendientes

### UI/UX
- Diseño responsive (mobile-first)
- Transiciones y animaciones
- Loading states
- Notificaciones con alerts
- Navegación intuitiva

## 🛠️ Scripts Disponibles

```bash
# Desarrollo
npm run dev

# Build para producción
npm run build

# Preview de build
npm run preview

# Linting
npm run lint
```

## 🔗 Integración con Backend

El frontend consume la API REST del backend .NET:

- **Base URL**: `http://localhost:5000/api`
- **Autenticación**: JWT Bearer Token
- **Timeout**: 10 segundos
- **Interceptores**: 
  - Request: Inyecta token JWT automáticamente
  - Response: Maneja errores 401 (redirección a login)

## 🎨 Personalización

### Colores (TailwindCSS)

Los colores primarios se definen en `tailwind.config.js`:

```javascript
colors: {
  primary: {
    50: '#f0f9ff',
    // ... más tonos
    600: '#2563eb', // Color principal
    // ... más tonos
    900: '#1e3a8a',
  }
}
```

### Componentes Base

Los estilos de componentes base están en `src/style.css` usando `@apply`:

- `.btn` - Botones
- `.btn-primary`, `.btn-secondary`, `.btn-outline` - Variantes
- `.input` - Campos de texto
- `.card` - Tarjetas
- `.badge` - Badges con variantes (success, warning, danger, info)

## 📱 Responsive Design

Breakpoints de TailwindCSS:

- **sm**: 640px
- **md**: 768px
- **lg**: 1024px
- **xl**: 1280px
- **2xl**: 1536px

## 🔐 Seguridad

- Tokens JWT almacenados en localStorage
- Validación de expiración de tokens
- Guards de autenticación en rutas protegidas
- Sanitización de inputs
- CORS configurado en backend

## 📄 Licencia

Este proyecto es parte de Tienda Moderna.
