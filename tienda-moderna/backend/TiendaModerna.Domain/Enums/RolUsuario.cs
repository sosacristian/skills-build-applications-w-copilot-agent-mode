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
