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
