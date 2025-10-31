# Script para verificar e instalar .NET 8 SDK

Write-Host "=======================================" -ForegroundColor Cyan
Write-Host "  VERIFICACIÓN DE .NET 8 SDK" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host ""

# Verificar si dotnet está instalado
$dotnetInstalled = Get-Command dotnet -ErrorAction SilentlyContinue

if ($dotnetInstalled) {
    Write-Host "✓ .NET SDK encontrado!" -ForegroundColor Green
    Write-Host ""
    
    # Obtener versión
    $version = dotnet --version
    Write-Host "Versión instalada: $version" -ForegroundColor Yellow
    
    # Verificar si es .NET 8
    if ($version -like "8.*") {
        Write-Host "✓ .NET 8 SDK está instalado correctamente" -ForegroundColor Green
        Write-Host ""
        Write-Host "Puedes continuar con:" -ForegroundColor Cyan
        Write-Host "  cd tienda-moderna/backend" -ForegroundColor White
        Write-Host "  dotnet build" -ForegroundColor White
    } else {
        Write-Host "⚠ Tienes .NET $version, pero necesitas .NET 8" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Descarga .NET 8 SDK desde:" -ForegroundColor Cyan
        Write-Host "  https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor White
    }
} else {
    Write-Host "✗ .NET SDK NO está instalado" -ForegroundColor Red
    Write-Host ""
    Write-Host "PASOS PARA INSTALAR:" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. Abre tu navegador" -ForegroundColor White
    Write-Host "2. Ve a: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    Write-Host "3. Descarga: '.NET 8.0 SDK (v8.0.x) - Windows x64 Installer'" -ForegroundColor White
    Write-Host "4. Ejecuta el instalador" -ForegroundColor White
    Write-Host "5. Reinicia PowerShell" -ForegroundColor White
    Write-Host "6. Vuelve a ejecutar este script para verificar" -ForegroundColor White
    Write-Host ""
    
    # Intentar abrir el navegador
    $respuesta = Read-Host "¿Quieres abrir el navegador ahora? (S/N)"
    if ($respuesta -eq "S" -or $respuesta -eq "s") {
        Start-Process "https://dotnet.microsoft.com/download/dotnet/8.0"
        Write-Host "✓ Navegador abierto. Descarga e instala .NET 8 SDK" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "=======================================" -ForegroundColor Cyan
