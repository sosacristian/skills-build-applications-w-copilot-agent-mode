namespace TiendaModerna.Application.DTOs.Marca
{
    /// <summary>
    /// DTO de marca
    /// </summary>
    public class MarcaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? UrlLogo { get; set; }
        public string? SitioWeb { get; set; }
        public bool EstaActiva { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Estadísticas
        public int CantidadProductos { get; set; }
    }
}
