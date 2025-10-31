using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TiendaModerna.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "CategoriaPadreId", "Descripcion", "EstaActiva", "FechaCreacion", "Nombre", "Orden", "Slug", "UrlImagen" },
                values: new object[,]
                {
                    { 1, null, "Remeras deportivas y casuales", true, new DateTime(2025, 10, 31, 16, 59, 3, 412, DateTimeKind.Utc).AddTicks(9518), "Remeras", 0, "remeras", null },
                    { 2, null, "Pantalones deportivos y casuales", true, new DateTime(2025, 10, 31, 16, 59, 3, 412, DateTimeKind.Utc).AddTicks(9520), "Pantalones", 0, "pantalones", null },
                    { 3, null, "Camperas y abrigos deportivos", true, new DateTime(2025, 10, 31, 16, 59, 3, 412, DateTimeKind.Utc).AddTicks(9522), "Camperas", 0, "camperas", null },
                    { 4, null, "Calzado deportivo de alta calidad", true, new DateTime(2025, 10, 31, 16, 59, 3, 412, DateTimeKind.Utc).AddTicks(9523), "Zapatillas", 0, "zapatillas", null },
                    { 5, null, "Accesorios deportivos y complementos", true, new DateTime(2025, 10, 31, 16, 59, 3, 412, DateTimeKind.Utc).AddTicks(9525), "Accesorios", 0, "accesorios", null }
                });

            migrationBuilder.InsertData(
                table: "Marcas",
                columns: new[] { "Id", "Descripcion", "EstaActiva", "FechaCreacion", "Nombre", "SitioWeb", "UrlLogo" },
                values: new object[,]
                {
                    { 1, "Just Do It", true, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(182), "Nike", null, null },
                    { 2, "Impossible is Nothing", true, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(183), "Adidas", null, null },
                    { 3, "Forever Faster", true, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(185), "Puma", null, null },
                    { 4, "I Will", true, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(186), "Under Armour", null, null },
                    { 5, "Be More Human", true, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(187), "Reebok", null, null }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "CantidadStock", "CategoriaId", "CodigoSKU", "Descripcion", "EsDestacado", "EstaActivo", "FechaActualizacion", "FechaCreacion", "MarcaId", "Nombre", "PorcentajeDescuento", "PrecioBase" },
                values: new object[,]
                {
                    { 1, 50, 1, "REM-NIKE-001", "Remera deportiva de alta calidad con tecnología Dri-FIT", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(254), 1, "Nike Deportiva Remera", 0m, 8500m },
                    { 2, 45, 1, "REM-ADIDAS-001", "Remera de entrenamiento con tecnología Climacool", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(257), 2, "Adidas Performance Remera", 0m, 7900m },
                    { 3, 30, 2, "PANT-NIKE-001", "Pantalón deportivo con tecnología Fleece para mayor comodidad", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(300), 1, "Nike Tech Fleece Pantalón", 20m, 15000m },
                    { 4, 25, 4, "ZAP-NIKE-001", "Zapatillas con amortiguación Air Max para máximo confort", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(303), 1, "Nike Air Max Zapatillas", 0m, 35000m },
                    { 5, 20, 4, "ZAP-ADIDAS-001", "Zapatillas con tecnología Boost para máxima respuesta", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(305), 2, "Adidas Ultraboost Zapatillas", 20m, 38000m },
                    { 6, 35, 3, "CAMP-PUMA-001", "Campera deportiva cortavientos", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(308), 3, "Puma Performance Campera", 15m, 22000m },
                    { 7, 60, 5, "ACC-NIKE-001", "Mochila deportiva con múltiples compartimentos", false, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(310), 1, "Nike Brasilia Mochila", 0m, 12000m },
                    { 8, 55, 1, "REM-PUMA-001", "Remera básica de algodón", false, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(312), 3, "Puma Essential Remera", 0m, 6500m },
                    { 9, 40, 4, "ZAP-REEBOK-001", "Zapatillas clásicas de cuero", true, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(314), 5, "Reebok Classic Leather", 20m, 18000m },
                    { 10, 42, 2, "PANT-ADIDAS-001", "Pantalón de entrenamiento clásico", false, true, null, new DateTime(2025, 10, 31, 16, 59, 3, 413, DateTimeKind.Utc).AddTicks(317), 2, "Adidas Tiro Pantalón", 0m, 13000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
