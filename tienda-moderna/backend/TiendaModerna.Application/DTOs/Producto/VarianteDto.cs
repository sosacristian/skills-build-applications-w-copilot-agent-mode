namespace TiendaModerna.Application.DTOs.Producto
{
    /// <summary>
    /// DTO para variantes de producto (tallas, colores, etc.)
    /// </summary>
    public class VarianteDto
    {
        public int Id { get; set; }
        public string CodigoSKU { get; set; } = string.Empty;
        public string? Talla { get; set; }
        public string? Color { get; set; }
        public string? Material { get; set; }
        public decimal AjustePrecio { get; set; }
        public int CantidadStock { get; set; }
        public bool EstaActiva { get; set; }
        
        // Propiedades calculadas
        public bool TieneStock => CantidadStock > 0;
        public string DescripcionCompleta
        {
            get
            {
                var partes = new List<string>();
                if (!string.IsNullOrEmpty(Talla)) partes.Add($"Talla {Talla}");
                if (!string.IsNullOrEmpty(Color)) partes.Add(Color);
                if (!string.IsNullOrEmpty(Material)) partes.Add(Material);
                return string.Join(" - ", partes);
            }
        }
    }
}
