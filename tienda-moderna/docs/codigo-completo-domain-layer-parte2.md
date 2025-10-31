# 🎯 Domain Layer - Parte 2 (Órdenes y Usuarios)

## 📁 Entities/Orden.cs

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa una orden de compra.
    /// Gestiona el ciclo completo: carrito → orden → pago → envío.
    /// Principio SRP: Responsable únicamente de representar una orden.
    /// </summary>
    public class Orden
    {
        /// <summary>
        /// Identificador único de la orden
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Número de orden único y legible
        /// Formato sugerido: ORD-20240101-0001
        /// </summary>
        [Required]
        [StringLength(50)]
        public string NumeroOrden { get; set; } = string.Empty;

        /// <summary>
        /// Estado actual de la orden
        /// </summary>
        [Required]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        /// <summary>
        /// Subtotal (suma de productos sin descuentos ni envío)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Total de descuentos aplicados
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDescuentos { get; set; } = 0;

        /// <summary>
        /// Costo de envío
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoEnvio { get; set; } = 0;

        /// <summary>
        /// Total final de la orden
        /// = Subtotal - TotalDescuentos + CostoEnvio
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        /// <summary>
        /// Código de cupón aplicado (si existe)
        /// </summary>
        [StringLength(50)]
        public string? CodigoCupon { get; set; }

        /// <summary>
        /// Notas adicionales del cliente
        /// </summary>
        [StringLength(500)]
        public string? NotasCliente { get; set; }

        /// <summary>
        /// Fecha de creación de la orden
        /// </summary>
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>
        /// Fecha en que se completó el pago
        /// </summary>
        public DateTime? FechaPago { get; set; }

        /// <summary>
        /// Fecha en que se envió la orden
        /// </summary>
        public DateTime? FechaEnvio { get; set; }

        /// <summary>
        /// Fecha en que se entregó la orden
        /// </summary>
        public DateTime? FechaEntrega { get; set; }

        // ============ INFORMACIÓN DE ENVÍO ============

        /// <summary>
        /// Nombre completo de la persona que recibe
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreDestinatario { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de envío - Línea 1
        /// </summary>
        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; } = string.Empty;

        /// <summary>
        /// Dirección de envío - Línea 2 (piso, depto, etc.)
        /// </summary>
        [StringLength(200)]
        public string? DireccionEnvio2 { get; set; }

        /// <summary>
        /// Ciudad de envío
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; } = string.Empty;

        /// <summary>
        /// Provincia/Estado
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Provincia { get; set; } = string.Empty;

        /// <summary>
        /// Código postal
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CodigoPostal { get; set; } = string.Empty;

        /// <summary>
        /// País
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Pais { get; set; } = "Argentina";

        /// <summary>
        /// Teléfono de contacto para la entrega
        /// </summary>
        [Required]
        [StringLength(20)]
        public string TelefonoContacto { get; set; } = string.Empty;

        // ============ INFORMACIÓN DE PAGO ============

        /// <summary>
        /// Método de pago utilizado
        /// Ejemplo: "Tarjeta de Crédito", "Mercado Pago", "Transferencia"
        /// </summary>
        [StringLength(100)]
        public string? MetodoPago { get; set; }

        /// <summary>
        /// ID de la transacción en la pasarela de pago
        /// </summary>
        [StringLength(200)]
        public string? IdTransaccionPago { get; set; }

        // ============ SEGUIMIENTO DE ENVÍO ============

        /// <summary>
        /// Empresa de transporte/courier
        /// </summary>
        [StringLength(100)]
        public string? EmpresaTransporte { get; set; }

        /// <summary>
        /// Código de seguimiento del envío
        /// </summary>
        [StringLength(100)]
        public string? CodigoSeguimiento { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// ID del usuario que realizó la orden
        /// </summary>
        [Required]
        public int UsuarioId { get; set; }

        /// <summary>
        /// Navegación hacia el usuario
        /// </summary>
        public virtual Usuario? Usuario { get; set; }

        /// <summary>
        /// Colección de detalles (productos) de la orden
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Calcula el total de la orden
        /// </summary>
        public void CalcularTotal()
        {
            Total = Subtotal - TotalDescuentos + CostoEnvio;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Genera un número de orden único
        /// Formato: ORD-YYYYMMDD-####
        /// </summary>
        public static string GenerarNumeroOrden(int contadorDiario)
        {
            var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
            return $"ORD-{fecha}-{contadorDiario:D4}";
        }

        /// <summary>
        /// Cambia el estado de la orden
        /// </summary>
        public void CambiarEstado(EstadoOrden nuevoEstado)
        {
            Estado = nuevoEstado;
            FechaActualizacion = DateTime.UtcNow;

            // Actualizar fechas según el estado
            switch (nuevoEstado)
            {
                case EstadoOrden.Pagada:
                    FechaPago = DateTime.UtcNow;
                    break;
                case EstadoOrden.Enviada:
                    FechaEnvio = DateTime.UtcNow;
                    break;
                case EstadoOrden.Entregada:
                    FechaEntrega = DateTime.UtcNow;
                    break;
            }
        }

        /// <summary>
        /// Verifica si la orden puede ser cancelada
        /// </summary>
        public bool PuedeCancelarse()
        {
            return Estado == EstadoOrden.Pendiente || Estado == EstadoOrden.Pagada;
        }

        /// <summary>
        /// Obtiene la dirección completa de envío
        /// </summary>
        public string ObtenerDireccionCompleta()
        {
            var direccion = DireccionEnvio;
            if (!string.IsNullOrEmpty(DireccionEnvio2))
                direccion += $", {DireccionEnvio2}";

            return $"{direccion}, {Ciudad}, {Provincia} ({CodigoPostal}), {Pais}";
        }

        /// <summary>
        /// Obtiene la cantidad total de productos en la orden
        /// </summary>
        public int ObtenerCantidadTotalProductos()
        {
            return Detalles?.Sum(d => d.Cantidad) ?? 0;
        }
    }
}
```

---

## 📁 Entities/DetalleOrden.cs

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un item/línea dentro de una orden.
    /// Cada DetalleOrden es un producto específico con su cantidad y precio.
    /// </summary>
    public class DetalleOrden
    {
        /// <summary>
        /// Identificador único del detalle
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Cantidad del producto ordenado
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio unitario en el momento de la compra
        /// Se guarda porque el precio del producto puede cambiar después
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Descuento unitario aplicado en el momento de la compra
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal DescuentoUnitario { get; set; } = 0;

        /// <summary>
        /// Subtotal de esta línea (Cantidad * PrecioUnitario)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// Total de esta línea después de descuentos
        /// = (Cantidad * PrecioUnitario) - (Cantidad * DescuentoUnitario)
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        /// <summary>
        /// Información de la variante seleccionada (talla, color, etc.)
        /// Se guarda como texto porque la variante puede ser eliminada después
        /// </summary>
        [StringLength(200)]
        public string? DetalleVariante { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// ID de la orden a la que pertenece este detalle
        /// </summary>
        [Required]
        public int OrdenId { get; set; }

        /// <summary>
        /// Navegación hacia la orden
        /// </summary>
        public virtual Orden? Orden { get; set; }

        /// <summary>
        /// ID del producto ordenado
        /// </summary>
        [Required]
        public int ProductoId { get; set; }

        /// <summary>
        /// Navegación hacia el producto
        /// </summary>
        public virtual Producto? Producto { get; set; }

        /// <summary>
        /// ID de la variante específica (si aplica)
        /// </summary>
        public int? VarianteId { get; set; }

        /// <summary>
        /// Navegación hacia la variante
        /// </summary>
        public virtual Variante? Variante { get; set; }

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Calcula el subtotal y total de la línea
        /// </summary>
        public void CalcularTotales()
        {
            Subtotal = Cantidad * PrecioUnitario;
            Total = Subtotal - (Cantidad * DescuentoUnitario);
        }

        /// <summary>
        /// Obtiene el precio final unitario (después de descuento)
        /// </summary>
        public decimal ObtenerPrecioFinalUnitario()
        {
            return PrecioUnitario - DescuentoUnitario;
        }
    }
}
```

---

## 📁 Entities/Usuario.cs

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TiendaModerna.Domain.Enums;

namespace TiendaModerna.Domain.Entities
{
    /// <summary>
    /// Entidad que representa un usuario del sistema.
    /// Puede ser cliente o administrador según su rol.
    /// Principio SRP: Solo representa un usuario, la autenticación se maneja en otra capa.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único del usuario
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del usuario
        /// </summary>
        [Required]
        [StringLength(200)]
        public string NombreCompleto { get; set; } = string.Empty;

        /// <summary>
        /// Email del usuario (también se usa para login)
        /// </summary>
        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash de la contraseña (NUNCA almacenar contraseña en texto plano)
        /// Se utiliza BCrypt para el hash
        /// </summary>
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Teléfono del usuario
        /// </summary>
        [StringLength(20)]
        public string? Telefono { get; set; }

        /// <summary>
        /// Rol del usuario en el sistema
        /// </summary>
        [Required]
        public RolUsuario Rol { get; set; } = RolUsuario.Cliente;

        /// <summary>
        /// Indica si el usuario está activo
        /// Se puede usar para "borrado lógico"
        /// </summary>
        public bool EstaActivo { get; set; } = true;

        /// <summary>
        /// Indica si el email fue verificado
        /// </summary>
        public bool EmailVerificado { get; set; } = false;

        /// <summary>
        /// Token para verificar el email
        /// </summary>
        [StringLength(500)]
        public string? TokenVerificacionEmail { get; set; }

        /// <summary>
        /// Fecha de expiración del token de verificación
        /// </summary>
        public DateTime? TokenVerificacionExpiracion { get; set; }

        /// <summary>
        /// Token para recuperar contraseña
        /// </summary>
        [StringLength(500)]
        public string? TokenRecuperacion { get; set; }

        /// <summary>
        /// Fecha de expiración del token de recuperación
        /// </summary>
        public DateTime? TokenRecuperacionExpiracion { get; set; }

        /// <summary>
        /// Fecha de último inicio de sesión
        /// </summary>
        public DateTime? UltimoInicioSesion { get; set; }

        /// <summary>
        /// Fecha de creación del usuario
        /// </summary>
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de última actualización
        /// </summary>
        public DateTime? FechaActualizacion { get; set; }

        // ============ DIRECCIÓN PREDETERMINADA ============

        /// <summary>
        /// Dirección predeterminada del usuario
        /// </summary>
        [StringLength(200)]
        public string? DireccionPredeterminada { get; set; }

        /// <summary>
        /// Ciudad predeterminada
        /// </summary>
        [StringLength(100)]
        public string? CiudadPredeterminada { get; set; }

        /// <summary>
        /// Provincia predeterminada
        /// </summary>
        [StringLength(100)]
        public string? ProvinciaPredeterminada { get; set; }

        /// <summary>
        /// Código postal predeterminado
        /// </summary>
        [StringLength(20)]
        public string? CodigoPostalPredeterminado { get; set; }

        // ============ PROPIEDADES DE NAVEGACIÓN ============

        /// <summary>
        /// Colección de órdenes realizadas por el usuario
        /// Relación One-to-Many
        /// </summary>
        public virtual ICollection<Orden> Ordenes { get; set; } = new List<Orden>();

        // ============ MÉTODOS DE NEGOCIO ============

        /// <summary>
        /// Verifica si el usuario es administrador
        /// </summary>
        public bool EsAdministrador() => Rol == RolUsuario.Administrador;

        /// <summary>
        /// Verifica si el usuario es cliente
        /// </summary>
        public bool EsCliente() => Rol == RolUsuario.Cliente;

        /// <summary>
        /// Registra el inicio de sesión del usuario
        /// </summary>
        public void RegistrarInicioSesion()
        {
            UltimoInicioSesion = DateTime.UtcNow;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Genera un token de verificación de email
        /// </summary>
        public void GenerarTokenVerificacionEmail()
        {
            TokenVerificacionEmail = Guid.NewGuid().ToString();
            TokenVerificacionExpiracion = DateTime.UtcNow.AddHours(24);
        }

        /// <summary>
        /// Genera un token de recuperación de contraseña
        /// </summary>
        public void GenerarTokenRecuperacion()
        {
            TokenRecuperacion = Guid.NewGuid().ToString();
            TokenRecuperacionExpiracion = DateTime.UtcNow.AddHours(2);
        }

        /// <summary>
        /// Verifica si el token de verificación de email es válido
        /// </summary>
        public bool TokenVerificacionEsValido(string token)
        {
            return TokenVerificacionEmail == token 
                && TokenVerificacionExpiracion.HasValue 
                && TokenVerificacionExpiracion.Value > DateTime.UtcNow;
        }

        /// <summary>
        /// Verifica si el token de recuperación es válido
        /// </summary>
        public bool TokenRecuperacionEsValido(string token)
        {
            return TokenRecuperacion == token 
                && TokenRecuperacionExpiracion.HasValue 
                && TokenRecuperacionExpiracion.Value > DateTime.UtcNow;
        }

        /// <summary>
        /// Marca el email como verificado
        /// </summary>
        public void VerificarEmail()
        {
            EmailVerificado = true;
            TokenVerificacionEmail = null;
            TokenVerificacionExpiracion = null;
            FechaActualizacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Obtiene la dirección completa predeterminada
        /// </summary>
        public string? ObtenerDireccionCompleta()
        {
            if (string.IsNullOrEmpty(DireccionPredeterminada))
                return null;

            return $"{DireccionPredeterminada}, {CiudadPredeterminada}, {ProvinciaPredeterminada} ({CodigoPostalPredeterminado})";
        }
    }
}
```

---

## 📁 Enums/EstadoOrden.cs

```csharp
namespace TiendaModerna.Domain.Enums
{
    /// <summary>
    /// Enumeración de los posibles estados de una orden.
    /// Representa el ciclo de vida completo de una orden.
    /// </summary>
    public enum EstadoOrden
    {
        /// <summary>
        /// Orden creada pero no pagada aún
        /// </summary>
        Pendiente = 1,

        /// <summary>
        /// Pago confirmado, pendiente de preparación
        /// </summary>
        Pagada = 2,

        /// <summary>
        /// Orden en proceso de preparación
        /// </summary>
        EnPreparacion = 3,

        /// <summary>
        /// Orden enviada al cliente
        /// </summary>
        Enviada = 4,

        /// <summary>
        /// Orden entregada exitosamente
        /// </summary>
        Entregada = 5,

        /// <summary>
        /// Orden cancelada (por cliente o admin)
        /// </summary>
        Cancelada = 6,

        /// <summary>
        /// Orden devuelta por el cliente
        /// </summary>
        Devuelta = 7
    }
}
```

---

## 📁 Enums/RolUsuario.cs

```csharp
namespace TiendaModerna.Domain.Enums
{
    /// <summary>
    /// Enumeración de los roles de usuario en el sistema.
    /// Principio de mínimos privilegios: cada usuario tiene solo los permisos necesarios.
    /// </summary>
    public enum RolUsuario
    {
        /// <summary>
        /// Cliente regular - puede comprar productos
        /// </summary>
        Cliente = 1,

        /// <summary>
        /// Administrador - acceso completo al sistema
        /// </summary>
        Administrador = 2,

        /// <summary>
        /// Empleado - acceso limitado para gestión de órdenes
        /// (Opcional, para futuras expansiones)
        /// </summary>
        Empleado = 3
    }
}
```

---

## 📁 Enums/TipoDescuento.cs

```csharp
namespace TiendaModerna.Domain.Enums
{
    /// <summary>
    /// Enumeración de los tipos de descuento que se pueden aplicar.
    /// </summary>
    public enum TipoDescuento
    {
        /// <summary>
        /// Descuento expresado como porcentaje (ej: 20%)
        /// </summary>
        Porcentaje = 1,

        /// <summary>
        /// Descuento expresado como monto fijo (ej: $500)
        /// </summary>
        MontoFijo = 2,

        /// <summary>
        /// 2x1, 3x2, etc.
        /// </summary>
        PorCantidad = 3
    }
}
```

Continúa en el próximo archivo con las Interfaces (Repositories)...
