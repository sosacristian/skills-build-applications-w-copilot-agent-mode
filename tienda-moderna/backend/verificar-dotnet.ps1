# Script para verificar y configurar .NET SDK

Write-Host "==================================" -ForegroundColor Cyan
Write-Host "Verificando .NET SDK Installation" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Buscar instalación de .NET
$dotnetPaths = @(
    "C:\Program Files\dotnet\dotnet.exe",
    "C:\Program Files (x86)\dotnet\dotnet.exe",
    "$env:LOCALAPPDATA\Microsoft\dotnet\dotnet.exe"
)

$dotnetPath = $null
foreach ($path in $dotnetPaths) {
    if (Test-Path $path) {
        $dotnetPath = $path
        Write-Host "✅ .NET SDK encontrado en: $path" -ForegroundColor Green
        break
    }
}

if ($null -eq $dotnetPath) {
    Write-Host "❌ .NET SDK no encontrado" -ForegroundColor Red
    Write-Host ""
    Write-Host "Por favor, descarga e instala .NET 8 SDK desde:" -ForegroundColor Yellow
    Write-Host "https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Después de instalar, cierra y reabre PowerShell." -ForegroundColor Yellow
    exit 1
}

# Obtener la carpeta de .NET
$dotnetFolder = Split-Path $dotnetPath -Parent

# Verificar si está en el PATH
$pathEnv = $env:PATH
if ($pathEnv -notlike "*$dotnetFolder*") {
    Write-Host ""
    Write-Host "⚠️  .NET no está en el PATH del sistema" -ForegroundColor Yellow
    Write-Host "Agregando temporalmente para esta sesión..." -ForegroundColor Yellow
    $env:PATH = "$dotnetFolder;$env:PATH"
    Write-Host "✅ PATH actualizado temporalmente" -ForegroundColor Green
}

Write-Host ""
Write-Host "Verificando versión de .NET..." -ForegroundColor Cyan
& $dotnetPath --version

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ .NET SDK está funcionando correctamente" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "Información completa del SDK:" -ForegroundColor Cyan
    & $dotnetPath --info | Select-Object -First 15
    
    Write-Host ""
    Write-Host "==================================" -ForegroundColor Green
    Write-Host "✅ TODO OK - Puedes continuar" -ForegroundColor Green
    Write-Host "==================================" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "❌ Hubo un problema al ejecutar dotnet" -ForegroundColor Red
}

Write-Host ""
Write-Host "Presiona Enter para continuar..." -ForegroundColor Gray
$null = Read-Host
