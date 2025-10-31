namespace TiendaModerna.Application.DTOs.Categoria
{
    /// <summary>
    /// DTO completo de categoría con jerarquía
    /// </summary>
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? UrlImagen { get; set; }
        public int Orden { get; set; }
        public bool EstaActiva { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Jerarquía
        public int? CategoriaPadreId { get; set; }
        public string? CategoriaPadreNombre { get; set; }
        public List<CategoriaDto>? SubCategorias { get; set; }

        // Estadísticas
        public int CantidadProductos { get; set; }
    }
}
