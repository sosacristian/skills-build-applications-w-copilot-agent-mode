using System;
using System.Threading.Tasks;

namespace TiendaModerna.Domain.Interfaces
{
    /// <summary>
    /// Interfaz del patrón Unit of Work.
    /// 
    /// ¿QUÉ ES UNIT OF WORK?
    /// Un patrón que mantiene una lista de objetos afectados por una transacción
    /// de negocio y coordina la escritura de cambios.
    /// 
    /// ¿POR QUÉ LO NECESITAMOS?
    /// 1. Transacciones: Todos los cambios se confirman juntos o ninguno
    /// 2. Consistencia: Evita estados inconsistentes en la BD
    /// 3. Performance: Agrupa múltiples operaciones en una sola transacción
    /// 4. Simplicidad: No hay que llamar SaveChanges en cada repositorio
    /// 
    /// EJEMPLO DE USO:
    /// <code>
    /// var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(1);
    /// producto.CantidadStock -= 5;
    /// 
    /// var orden = new Orden { ... };
    /// await _unitOfWork.Ordenes.AgregarAsync(orden);
    /// 
    /// // Ambos cambios se guardan juntos
    /// await _unitOfWork.CompletarAsync();
    /// </code>
    /// 
    /// Si algo falla, ningún cambio se aplica (rollback automático).
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Repositorio de Productos
        /// </summary>
        IRepositorioProducto Productos { get; }

        /// <summary>
        /// Repositorio de Categorías
        /// </summary>
        IRepositorioCategoria Categorias { get; }

        /// <summary>
        /// Repositorio de Órdenes
        /// </summary>
        IRepositorioOrden Ordenes { get; }

        /// <summary>
        /// Repositorio de Usuarios
        /// </summary>
        IRepositorioUsuario Usuarios { get; }

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos
        /// dentro de una transacción.
        /// </summary>
        /// <returns>Número de registros afectados</returns>
        Task<int> CompletarAsync();

        /// <summary>
        /// Inicia una transacción explícita
        /// Útil cuando necesitas control fino sobre transacciones anidadas
        /// </summary>
        Task IniciarTransaccionAsync();

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        Task ConfirmarTransaccionAsync();

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        Task RevertirTransaccionAsync();
    }
}
