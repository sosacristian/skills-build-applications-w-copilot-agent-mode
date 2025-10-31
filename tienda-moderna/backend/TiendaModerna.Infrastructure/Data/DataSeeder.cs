using Microsoft.EntityFrameworkCore;
using TiendaModerna.Domain.Entities;
using System;

namespace TiendaModerna.Infrastructure.Data
{
    /// <summary>
    /// Clase para poblar la base de datos con datos iniciales
    /// </summary>
    public static class DataSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categorías
            var categorias = new[]
            {
                new Categoria 
                { 
                    Id = 1, 
                    Nombre = "Remeras", 
                    Descripcion = "Remeras deportivas y casuales", 
                    Slug = "remeras", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Categoria 
                { 
                    Id = 2, 
                    Nombre = "Pantalones", 
                    Descripcion = "Pantalones deportivos y casuales", 
                    Slug = "pantalones", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Categoria 
                { 
                    Id = 3, 
                    Nombre = "Camperas", 
                    Descripcion = "Camperas y abrigos deportivos", 
                    Slug = "camperas", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Categoria 
                { 
                    Id = 4, 
                    Nombre = "Zapatillas", 
                    Descripcion = "Calzado deportivo de alta calidad", 
                    Slug = "zapatillas", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Categoria 
                { 
                    Id = 5, 
                    Nombre = "Accesorios", 
                    Descripcion = "Accesorios deportivos y complementos", 
                    Slug = "accesorios", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                }
            };
            modelBuilder.Entity<Categoria>().HasData(categorias);

            // Seed Marcas
            var marcas = new[]
            {
                new Marca 
                { 
                    Id = 1, 
                    Nombre = "Nike", 
                    Descripcion = "Just Do It", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Marca 
                { 
                    Id = 2, 
                    Nombre = "Adidas", 
                    Descripcion = "Impossible is Nothing", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Marca 
                { 
                    Id = 3, 
                    Nombre = "Puma", 
                    Descripcion = "Forever Faster", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Marca 
                { 
                    Id = 4, 
                    Nombre = "Under Armour", 
                    Descripcion = "I Will", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                },
                new Marca 
                { 
                    Id = 5, 
                    Nombre = "Reebok", 
                    Descripcion = "Be More Human", 
                    EstaActiva = true, 
                    FechaCreacion = DateTime.UtcNow 
                }
            };
            modelBuilder.Entity<Marca>().HasData(marcas);

            // Seed Productos
            var productos = new[]
            {
                new Producto
                {
                    Id = 1, 
                    CodigoSKU = "REM-NIKE-001", 
                    Nombre = "Nike Deportiva Remera", 
                    Descripcion = "Remera deportiva de alta calidad con tecnología Dri-FIT", 
                    PrecioBase = 8500m, 
                    PorcentajeDescuento = 0m, 
                    CantidadStock = 50, 
                    CategoriaId = 1, 
                    MarcaId = 1,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 2, 
                    CodigoSKU = "REM-ADIDAS-001", 
                    Nombre = "Adidas Performance Remera", 
                    Descripcion = "Remera de entrenamiento con tecnología Climacool", 
                    PrecioBase = 7900m, 
                    PorcentajeDescuento = 0m, 
                    CantidadStock = 45, 
                    CategoriaId = 1, 
                    MarcaId = 2,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 3, 
                    CodigoSKU = "PANT-NIKE-001", 
                    Nombre = "Nike Tech Fleece Pantalón", 
                    Descripcion = "Pantalón deportivo con tecnología Fleece para mayor comodidad", 
                    PrecioBase = 15000m, 
                    PorcentajeDescuento = 20m, 
                    CantidadStock = 30, 
                    CategoriaId = 2, 
                    MarcaId = 1,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 4, 
                    CodigoSKU = "ZAP-NIKE-001", 
                    Nombre = "Nike Air Max Zapatillas", 
                    Descripcion = "Zapatillas con amortiguación Air Max para máximo confort", 
                    PrecioBase = 35000m, 
                    PorcentajeDescuento = 0m, 
                    CantidadStock = 25, 
                    CategoriaId = 4, 
                    MarcaId = 1,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 5, 
                    CodigoSKU = "ZAP-ADIDAS-001", 
                    Nombre = "Adidas Ultraboost Zapatillas", 
                    Descripcion = "Zapatillas con tecnología Boost para máxima respuesta", 
                    PrecioBase = 38000m, 
                    PorcentajeDescuento = 20m, 
                    CantidadStock = 20, 
                    CategoriaId = 4, 
                    MarcaId = 2,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 6, 
                    CodigoSKU = "CAMP-PUMA-001", 
                    Nombre = "Puma Performance Campera", 
                    Descripcion = "Campera deportiva cortavientos", 
                    PrecioBase = 22000m, 
                    PorcentajeDescuento = 15m, 
                    CantidadStock = 35, 
                    CategoriaId = 3, 
                    MarcaId = 3,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 7, 
                    CodigoSKU = "ACC-NIKE-001", 
                    Nombre = "Nike Brasilia Mochila", 
                    Descripcion = "Mochila deportiva con múltiples compartimentos", 
                    PrecioBase = 12000m, 
                    PorcentajeDescuento = 0m, 
                    CantidadStock = 60, 
                    CategoriaId = 5, 
                    MarcaId = 1,
                    EstaActivo = true, 
                    EsDestacado = false, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 8, 
                    CodigoSKU = "REM-PUMA-001", 
                    Nombre = "Puma Essential Remera", 
                    Descripcion = "Remera básica de algodón", 
                    PrecioBase = 6500m, 
                    PorcentajeDescuento = 0m, 
                    CantidadStock = 55, 
                    CategoriaId = 1, 
                    MarcaId = 3,
                    EstaActivo = true, 
                    EsDestacado = false, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 9, 
                    CodigoSKU = "ZAP-REEBOK-001", 
                    Nombre = "Reebok Classic Leather", 
                    Descripcion = "Zapatillas clásicas de cuero", 
                    PrecioBase = 18000m, 
                    PorcentajeDescuento = 20m, 
                    CantidadStock = 40, 
                    CategoriaId = 4, 
                    MarcaId = 5,
                    EstaActivo = true, 
                    EsDestacado = true, 
                    FechaCreacion = DateTime.UtcNow
                },
                new Producto
                {
                    Id = 10, 
                    CodigoSKU = "PANT-ADIDAS-001", 
                    Nombre = "Adidas Tiro Pantalón", 
                    Descripcion = "Pantalón de entrenamiento clásico", 
                    PrecioBase = 13000m, 
                    PorcentajeDescuento = 0m, 
                    CantidadStock = 42, 
                    CategoriaId = 2, 
                    MarcaId = 2,
                    EstaActivo = true, 
                    EsDestacado = false, 
                    FechaCreacion = DateTime.UtcNow
                }
            };
            modelBuilder.Entity<Producto>().HasData(productos);
        }
    }
}
