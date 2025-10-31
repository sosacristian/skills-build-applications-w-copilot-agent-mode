using TiendaModerna.Domain.Interfaces;
using TiendaModerna.Infrastructure.Data;

namespace TiendaModerna.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del patrón Unit of Work.
    /// 
    /// ¿QUÉ ES UNIT OF WORK?
    /// - Coordina el trabajo de múltiples repositorios
    /// - Garantiza que todos los cambios se guarden en una sola transacción
    /// - Evita problemas de concurrencia
    /// - Implementa el principio de atomicidad (todo o nada)
    /// 
    /// BENEFICIOS:
    /// 1. Transacciones: Todos los cambios se confirman juntos
    /// 2. Consistencia: Los datos permanecen consistentes
    /// 3. Rendimiento: Reduce viajes a la base de datos
    /// 4. Simplicidad: API limpia para guardar cambios
    /// 
    /// USO TÍPICO:
    /// ```
    /// // Agregar producto
    /// await _unitOfWork.Productos.AgregarAsync(producto);
    /// 
    /// // Actualizar stock
    /// await _unitOfWork.Productos.ActualizarStockAsync(id, -1);
    /// 
    /// // Guardar TODO en una transacción
    /// await _unitOfWork.GuardarCambiosAsync();
    /// ```
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TiendaContext _contexto;
        private IRepositorioProducto? _productos;
        private IRepositorioCategoria? _categorias;
        private IRepositorioOrden? _ordenes;
        private IRepositorioUsuario? _usuarios;
        private bool _disposed = false;

        public UnitOfWork(TiendaContext contexto)
        {
            _contexto = contexto;
        }

        // ============ Repositorios (Lazy Loading) ============

        /// <summary>
        /// Repositorio de Productos.
        /// Se crea solo cuando se usa por primera vez (lazy loading).
        /// </summary>
        public IRepositorioProducto Productos
        {
            get
            {
                if (_productos == null)
                {
                    _productos = new RepositorioProducto(_contexto);
                }
                return _productos;
            }
        }

        /// <summary>
        /// Repositorio de Categorías.
        /// </summary>
        public IRepositorioCategoria Categorias
        {
            get
            {
                if (_categorias == null)
                {
                    _categorias = new RepositorioCategoria(_contexto);
                }
                return _categorias;
            }
        }

        /// <summary>
        /// Repositorio de Órdenes.
        /// </summary>
        public IRepositorioOrden Ordenes
        {
            get
            {
                if (_ordenes == null)
                {
                    _ordenes = new RepositorioOrden(_contexto);
                }
                return _ordenes;
            }
        }

        /// <summary>
        /// Repositorio de Usuarios.
        /// </summary>
        public IRepositorioUsuario Usuarios
        {
            get
            {
                if (_usuarios == null)
                {
                    _usuarios = new RepositorioUsuario(_contexto);
                }
                return _usuarios;
            }
        }

        // ============ Gestión de Transacciones ============

        /// <summary>
        /// Guarda todos los cambios pendientes en la base de datos.
        /// 
        /// IMPORTANTE: Esta operación es ATÓMICA:
        /// - Si todo sale bien, todos los cambios se confirman
        /// - Si algo falla, NINGÚN cambio se aplica (rollback automático)
        /// 
        /// Retorna el número de entidades afectadas.
        /// </summary>
        public async Task<int> CompletarAsync()
        {
            try
            {
                return await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log del error (en producción usar ILogger)
                Console.WriteLine($"Error al guardar cambios: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Inicia una transacción explícita.
        /// </summary>
        public async Task IniciarTransaccionAsync()
        {
            await _contexto.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Confirma la transacción actual.
        /// </summary>
        public async Task ConfirmarTransaccionAsync()
        {
            if (_contexto.Database.CurrentTransaction != null)
            {
                await _contexto.Database.CurrentTransaction.CommitAsync();
            }
        }

        /// <summary>
        /// Revierte la transacción actual.
        /// </summary>
        public async Task RevertirTransaccionAsync()
        {
            if (_contexto.Database.CurrentTransaction != null)
            {
                await _contexto.Database.CurrentTransaction.RollbackAsync();
            }
        }

        // ============ IDisposable Implementation ============

        /// <summary>
        /// Libera los recursos del contexto.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Liberar recursos administrados
                    _contexto.Dispose();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Libera los recursos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Versión asíncrona de Dispose.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            await _contexto.DisposeAsync();
            _disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}
