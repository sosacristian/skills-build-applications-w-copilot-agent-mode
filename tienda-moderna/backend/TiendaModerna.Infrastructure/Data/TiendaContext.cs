using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Entities;

namespace TiendaModerna.Infrastructure.Data
{
    /// <summary>
    /// Contexto de base de datos para Tienda Moderna.
    /// 
    /// ¿QUÉ ES UN DbContext?
    /// Es la clase principal de Entity Framework Core que:
    /// - Representa una sesión con la base de datos
    /// - Permite consultar y guardar datos
    /// - Gestiona el seguimiento de cambios (change tracking)
    /// - Coordina transacciones
    /// 
    /// PRINCIPIO: Separation of Concerns
    /// - El Domain no conoce EF Core
    /// - Infrastructure implementa los detalles técnicos
    /// </summary>
    public class TiendaContext : DbContext
    {
        /// <summary>
        /// Constructor que recibe las opciones de configuración.
        /// Usado por Dependency Injection en ASP.NET Core.
        /// </summary>
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
        {
        }

        // ============ DbSets (Tablas de la Base de Datos) ============

        /// <summary>
        /// Tabla de Productos
        /// </summary>
        public DbSet<Producto> Productos { get; set; } = null!;

        /// <summary>
        /// Tabla de Categorías
        /// </summary>
        public DbSet<Categoria> Categorias { get; set; } = null!;

        /// <summary>
        /// Tabla de Variantes de productos
        /// </summary>
        public DbSet<Variante> Variantes { get; set; } = null!;

        /// <summary>
        /// Tabla de Imágenes
        /// </summary>
        public DbSet<Imagen> Imagenes { get; set; } = null!;

        /// <summary>
        /// Tabla de Marcas
        /// </summary>
        public DbSet<Marca> Marcas { get; set; } = null!;

        /// <summary>
        /// Tabla de Órdenes
        /// </summary>
        public DbSet<Orden> Ordenes { get; set; } = null!;

        /// <summary>
        /// Tabla de Detalles de Órdenes (líneas de orden)
        /// </summary>
        public DbSet<DetalleOrden> DetallesOrdenes { get; set; } = null!;

        /// <summary>
        /// Tabla de Usuarios
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        /// <summary>
        /// Configuración del modelo usando Fluent API.
        /// 
        /// ¿POR QUÉ FLUENT API en lugar de Data Annotations?
        /// 1. Mantiene el Domain limpio (sin atributos de EF)
        /// 2. Más expresivo para relaciones complejas
        /// 3. Permite configuración avanzada
        /// 4. Separa la configuración de persistencia del dominio
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== CONFIGURACIÓN SIMPLIFICADA =====
            // Usamos convenciones de EF Core + configuración mínima necesaria

            // Configurar precisión de decimales globalmente
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            // Configurar conversión de enums a string
            modelBuilder.Entity<Orden>()
                .Property(o => o.Estado)
                .HasConversion<string>();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasConversion<string>();

            // Ignorar propiedades calculadas
            modelBuilder.Entity<Producto>()
                .Ignore(p => p.PrecioFinal);

            // Índices únicos importantes
            modelBuilder.Entity<Producto>()
                .HasIndex(p => p.CodigoSKU)
                .IsUnique();

            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            modelBuilder.Entity<Orden>()
                .HasIndex(o => o.NumeroOrden)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Variante>()
                .HasIndex(v => v.CodigoSKU)
                .IsUnique();

            // ===== SEED DATA =====
            // Poblar base de datos con datos iniciales
            DataSeeder.SeedData(modelBuilder);
        }

        /// <summary>
        /// Override de SaveChanges para agregar auditoría automática.
        /// Actualiza FechaActualizacion en todas las entidades modificadas.
        /// </summary>
        public override int SaveChanges()
        {
            ActualizarFechasAuditoria();
            return base.SaveChanges();
        }

        /// <summary>
        /// Override de SaveChangesAsync para agregar auditoría automática.
        /// </summary>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ActualizarFechasAuditoria();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Actualiza automáticamente las fechas de auditoría.
        /// Si una entidad tiene FechaCreacion, la establece al agregarla.
        /// Si tiene FechaActualizacion, la actualiza al modificarla.
        /// </summary>
        private void ActualizarFechasAuditoria()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                // Actualizar FechaActualizacion si existe
                if (entry.Entity.GetType().GetProperty("FechaActualizacion") != null)
                {
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property("FechaActualizacion").CurrentValue = DateTime.UtcNow;
                    }
                }

                // Establecer FechaCreacion si es nueva entidad
                if (entry.Entity.GetType().GetProperty("FechaCreacion") != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("FechaCreacion").CurrentValue = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
