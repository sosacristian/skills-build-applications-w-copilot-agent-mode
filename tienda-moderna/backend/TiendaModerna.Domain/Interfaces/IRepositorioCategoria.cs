using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz específica para el repositorio de Categorías.
    /// </summary>
    public interface IRepositorioCategoria : IRepositorioGenerico<Categoria>
    {
        /// <summary>
        /// Obtiene una categoría por su slug
        /// Útil para URLs amigables como /categoria/vestidos-mujer
        /// </summary>
        /// <param name="slug">Slug de la categoría</param>
        /// <returns>Categoría encontrada o null</returns>
        Task<Categoria?> ObtenerPorSlugAsync(string slug);

        /// <summary>
        /// Obtiene las categorías raíz (sin categoría padre)
        /// </summary>
        /// <returns>Colección de categorías raíz</returns>
        Task<IEnumerable<Categoria>> ObtenerCategoriasRaizAsync();

        /// <summary>
        /// Obtiene las subcategorías de una categoría
        /// </summary>
        /// <param name="categoriaPadreId">ID de la categoría padre</param>
        /// <returns>Colección de subcategorías</returns>
        Task<IEnumerable<Categoria>> ObtenerSubCategoriasAsync(int categoriaPadreId);

        /// <summary>
        /// Obtiene el árbol completo de categorías
        /// Incluye todas las relaciones padre-hijo
        /// </summary>
        /// <returns>Colección jerárquica de categorías</returns>
        Task<IEnumerable<Categoria>> ObtenerArbolCompletoAsync();

        /// <summary>
        /// Obtiene una categoría con todos sus productos
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>Categoría con productos o null</returns>
        Task<Categoria?> ObtenerConProductosAsync(int id);

        /// <summary>
        /// Verifica si una categoría tiene productos asociados
        /// Útil antes de eliminar una categoría
        /// </summary>
        /// <param name="id">ID de la categoría</param>
        /// <returns>True si tiene productos</returns>
        Task<bool> TieneProductosAsync(int id);

        /// <summary>
        /// Verifica si un slug ya existe
        /// </summary>
        /// <param name="slug">Slug a verificar</param>
        /// <param name="idExcluir">ID de categoría a excluir (para actualizaciones)</param>
        /// <returns>True si el slug ya existe</returns>
        Task<bool> SlugExisteAsync(string slug, int? idExcluir = null);
    }
}
