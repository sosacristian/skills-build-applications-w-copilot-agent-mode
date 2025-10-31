@echo off
echo =======================================================
echo       OCTOFIT TRACKER - LANZADOR UNIFICADO
echo =======================================================
echo.
echo Este script te ayudara a iniciar los servicios de OctoFit Tracker
echo Selecciona una de las siguientes opciones:
echo.
echo 1. Iniciar todo con Docker (recomendado)
echo 2. Iniciar solo el backend (Django)
echo 3. Iniciar solo el frontend
echo 4. Iniciar todo localmente (sin Docker)
echo 5. Instalar dependencias
echo 6. Salir
echo.

set /p option="Selecciona una opcion (1-6): "

if "%option%"=="1" (
    echo.
    echo Iniciando todos los servicios con Docker...
    call iniciar_docker.bat
) else if "%option%"=="2" (
    echo.
    echo Iniciando el backend (servidor Django)...
    cd backend
    call iniciar_servidor_completo.bat
    cd ..
) else if "%option%"=="3" (
    echo.
    echo Iniciando el frontend...
    cd frontend
    call iniciar_servidor.bat
    cd ..
) else if "%option%"=="4" (
    echo.
    echo Iniciando todos los servicios localmente...
    echo Primero iniciaremos el backend...
    start cmd /k "cd backend && call iniciar_servidor_completo.bat"
    echo Esperando 5 segundos para iniciar el frontend...
    timeout /t 5 /nobreak
    echo Iniciando el frontend...
    start cmd /k "cd frontend && call iniciar_servidor.bat"
) else if "%option%"=="5" (
    echo.
    echo Instalando dependencias...
    echo Instalando dependencias del backend...
    cd backend
    call instalar_dependencias.bat
    cd ..
    echo Instalando dependencias del frontend...
    cd frontend
    npm install
    cd ..
    echo Todas las dependencias han sido instaladas.
) else if "%option%"=="6" (
    echo.
    echo Saliendo...
    exit
) else (
    echo.
    echo Opcion invalida. Por favor, selecciona una opcion valida.
    echo.
    call iniciar.bat
)