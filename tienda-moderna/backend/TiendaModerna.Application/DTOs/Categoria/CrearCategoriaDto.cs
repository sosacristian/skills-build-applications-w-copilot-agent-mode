using System.ComponentModel.DataAnnotations;

namespace TiendaModerna.Application.DTOs.Categoria
{
    /// <summary>
    /// DTO para crear una nueva categoría
    /// </summary>
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El slug es obligatorio")]
        [StringLength(150, ErrorMessage = "El slug no puede exceder 150 caracteres")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "El slug solo puede contener letras minúsculas, números y guiones")]
        public string Slug { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La URL de imagen no puede exceder 500 caracteres")]
        public string? UrlImagen { get; set; }

        public int Orden { get; set; } = 0;
        public bool EstaActiva { get; set; } = true;
        public int? CategoriaPadreId { get; set; }
    }
}
