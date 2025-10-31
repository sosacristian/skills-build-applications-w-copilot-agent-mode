# Script para iniciar el servidor Django de OctoFit Tracker en PowerShell
Write-Host "================================================"
Write-Host "Iniciando servidor Django de OctoFit Tracker"
Write-Host "================================================"

# Cambiar al directorio del script
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location -Path $scriptPath

# Verificar si existe el entorno virtual
if (-not (Test-Path "venv\Scripts\Activate.ps1")) {
    Write-Host "ERROR: El entorno virtual no se encuentra."
    Write-Host "Creando entorno virtual..."
    try {
        python -m venv venv
    }
    catch {
        Write-Host "ERROR: No se pudo crear el entorno virtual."
        Write-Host "Asegúrate de que Python esté instalado correctamente."
        Read-Host -Prompt "Presiona Enter para salir"
        exit
    }
}

# Activar entorno virtual
Write-Host "Activando entorno virtual..."
try {
    . .\venv\Scripts\Activate.ps1
}
catch {
    Write-Host "ERROR: No se pudo activar el entorno virtual."
    Write-Host "Intentando con batch file..."
    cmd /c ".\venv\Scripts\activate.bat && echo Entorno virtual activado correctamente"
}

# Instalar dependencias si es necesario
Write-Host ""
Write-Host "Verificando dependencias..."
$djangoInstalled = pip freeze | Select-String -Pattern "Django"
if (-not $djangoInstalled) {
    Write-Host "Instalando dependencias desde requirements.txt..."
    pip install -r requirements.txt
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERROR: No se pudieron instalar las dependencias."
        Read-Host -Prompt "Presiona Enter para salir"
        exit
    }
}

# Verificar estructura del proyecto
Write-Host ""
Write-Host "Verificando estructura del proyecto..."
if (-not (Test-Path "octofit_tracker\manage.py")) {
    Write-Host "ERROR: No se encuentra el archivo manage.py."
    Write-Host "Ubicación actual: $PWD"
    Read-Host -Prompt "Presiona Enter para salir"
    exit
}

# Comprobar la configuración de Django
Write-Host ""
Write-Host "Ejecutando comprobación de Django..."
python octofit_tracker\manage.py check

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: La comprobación de Django ha fallado."
    Read-Host -Prompt "Presiona Enter para salir"
    exit
}

# Ejecutar migraciones si es necesario
Write-Host ""
Write-Host "Ejecutando migraciones..."
python octofit_tracker\manage.py migrate

if ($LASTEXITCODE -ne 0) {
    Write-Host "ADVERTENCIA: Las migraciones no se han completado correctamente."
}

# Iniciar el servidor
Write-Host ""
Write-Host "Iniciando servidor Django..."
Write-Host ""
python octofit_tracker\manage.py runserver 0.0.0.0:8000

if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: No se pudo iniciar el servidor Django."
    Write-Host "Consejos de solución de problemas:"
    Write-Host "1. Asegúrate de que Python esté instalado y en el PATH"
    Write-Host "2. Verifica que todas las dependencias estén instaladas"
    Write-Host "3. Comprueba que la estructura del proyecto sea correcta"
    Write-Host "4. Revisa los archivos de configuración de Django (settings.py)"
}

Read-Host -Prompt "Presiona Enter para salir"