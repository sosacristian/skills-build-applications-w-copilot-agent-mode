namespace TiendaModerna.Application.DTOs.Producto
{
    /// <summary>
    /// DTO para imágenes de productos
    /// </summary>
    public class ImagenDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? TextoAlternativo { get; set; }
        public int Orden { get; set; }
        public bool EsPrincipal { get; set; }
    }
}
