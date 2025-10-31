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
