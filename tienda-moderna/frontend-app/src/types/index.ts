// Tipos de Usuario
export interface Usuario {
  id: number
  email: string
  nombreCompleto: string
  telefono?: string
  rol: string
  estaActivo: boolean
  fechaCreacion: string
  ultimoInicioSesion?: string
  direccionPredeterminada?: string
  ciudadPredeterminada?: string
  provinciaPredeterminada?: string
  codigoPostalPredeterminado?: string
}

export interface RegistrarUsuarioDto {
  email: string
  password: string
  nombreCompleto: string
  telefono?: string
}

export interface LoginDto {
  email: string
  password: string
}

export interface AuthResponse {
  token: string
  expiracion: string
  usuario: Usuario
}

// Tipos de Producto
export interface Producto {
  id: number
  codigoSKU: string
  nombre: string
  descripcion?: string
  precioBase: number
  porcentajeDescuento: number
  precioFinal: number
  cantidadStock: number
  estaActivo: boolean
  esDestacado: boolean
  categoriaId: number
  categoriaNombre: string
  marcaId?: number
  marcaNombre?: string
  variantes?: Variante[]
  imagenes?: Imagen[]
  fechaCreacion: string
}

export interface ProductoLista {
  id: number
  codigoSKU: string
  nombre: string
  precioBase: number
  porcentajeDescuento: number
  precioFinal: number
  imagenPrincipalUrl?: string
  cantidadStock: number
  estaActivo: boolean
  esDestacado: boolean
  categoriaNombre: string
  marcaNombre?: string
}

export interface Variante {
  id: number
  codigoSKU: string
  talla?: string
  color?: string
  material?: string
  ajustePrecio: number
  cantidadStock: number
}

export interface Imagen {
  id: number
  url: string
  textoAlternativo?: string
  orden: number
  esPrincipal: boolean
  fechaSubida: string
}

export interface CrearProductoDto {
  codigoSKU: string
  nombre: string
  descripcion?: string
  precioBase: number
  porcentajeDescuento: number
  cantidadStock: number
  estaActivo: boolean
  esDestacado: boolean
  categoriaId: number
  marcaId?: number
}

// Tipos de Orden
export interface Orden {
  id: number
  numeroOrden: string
  estado: string
  subtotal: number
  totalDescuentos: number
  costoEnvio: number
  total: number
  codigoCupon?: string
  notasCliente?: string
  fechaCreacion: string
  fechaPago?: string
  fechaEnvio?: string
  fechaEntrega?: string
  nombreDestinatario: string
  direccionEnvio: string
  direccionEnvio2?: string
  ciudad: string
  provincia: string
  codigoPostal: string
  pais: string
  telefonoContacto: string
  metodoPago?: string
  idTransaccionPago?: string
  empresaTransporte?: string
  codigoSeguimiento?: string
  usuarioId: number
  usuarioNombreCompleto?: string
  usuarioEmail?: string
  detalles?: DetalleOrden[]
  cantidadProductos: number
  estaPaga: boolean
  estaEnviada: boolean
  estaEntregada: boolean
}

export interface DetalleOrden {
  id: number
  productoId: number
  nombreProducto: string
  codigoSKUProducto: string
  detalleVariante?: string
  precioUnitario: number
  cantidad: number
  descuentoUnitario: number
  subtotal: number
  total: number
  urlImagenProducto?: string
  totalSinDescuento: number
}

export interface CrearOrdenDto {
  usuarioId: number
  detalles: CrearDetalleOrdenDto[]
  codigoCupon?: string
  notasCliente?: string
  nombreDestinatario: string
  direccionEnvio: string
  direccionEnvio2?: string
  ciudad: string
  provincia: string
  codigoPostal: string
  pais: string
  telefonoContacto: string
  metodoPago?: string
}

export interface CrearDetalleOrdenDto {
  productoId: number
  cantidad: number
}

// Tipo de paginación
export interface PagedResult<T> {
  items: T[]
  paginaActual: number
  totalPaginas: number
  tamanoPagina: number
  totalItems: number
  tienePaginaAnterior: boolean
  tienePaginaSiguiente: boolean
}

// Tipo de Categoría
export interface Categoria {
  id: number
  nombre: string
  descripcion?: string
  slug: string
  urlImagen?: string
  orden: number
  estaActiva: boolean
  fechaCreacion: string
  categoriaPadreId?: number
  categoriaPadreNombre?: string
  subCategorias?: Categoria[]
  cantidadProductos: number
}

// Tipo de Marca
export interface Marca {
  id: number
  nombre: string
  descripcion?: string
  urlLogo?: string
  sitioWeb?: string
  estaActiva: boolean
  fechaCreacion: string
  cantidadProductos: number
}

// Item del carrito (solo en frontend)
export interface ItemCarrito {
  producto: ProductoLista
  cantidad: number
  varianteId?: number
}
