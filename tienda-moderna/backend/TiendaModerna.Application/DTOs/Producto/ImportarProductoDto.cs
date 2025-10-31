namespace TiendaModerna.Application.DTOs.Producto
{
    /// <summary>
    /// DTO para resultado de importación de productos desde Excel
    /// </summary>
    public class ResultadoImportacionDto
    {
        public int TotalProcesados { get; set; }
        public int Exitosos { get; set; }
        public int Fallidos { get; set; }
        public List<ErrorImportacionDto> Errores { get; set; } = new();
    }

    /// <summary>
    /// DTO para errores durante importación
    /// </summary>
    public class ErrorImportacionDto
    {
        public int Fila { get; set; }
        public string Campo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
    }
}
