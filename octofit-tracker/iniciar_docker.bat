@echo off
echo ================================================
echo Iniciando OctoFit Tracker con Docker
echo ================================================
echo.
echo Este script gestionará todos los servicios necesarios para
echo OctoFit Tracker usando Docker Compose.
echo.
echo Requisitos:
echo - Docker instalado y en ejecución
echo - Docker Compose instalado
echo.
echo Servicios disponibles:
echo - Backend de Django (puerto 8000)
echo - Base de datos PostgreSQL (puerto 5432)
echo - Frontend (puerto 3000)
echo.

:menu
echo ------------------------------------------------
echo MENÚ DE OPCIONES
echo ------------------------------------------------
echo 1. Iniciar servicios (en primer plano con logs)
echo 2. Iniciar servicios en segundo plano
echo 3. Ver logs de servicios en ejecución
echo 4. Detener servicios
echo 5. Verificar estado de los servicios
echo 6. Salir
echo.
set /p opcion="Selecciona una opción [1-6]: "

if "%opcion%"=="1" goto iniciar_foreground
if "%opcion%"=="2" goto iniciar_background
if "%opcion%"=="3" goto ver_logs
if "%opcion%"=="4" goto detener
if "%opcion%"=="5" goto estado
if "%opcion%"=="6" goto salir

echo Opción no válida. Inténtalo de nuevo.
pause
goto menu

:iniciar_foreground
echo.
echo Construyendo y levantando los contenedores (modo primer plano)...
docker-compose build
docker-compose up
echo.
echo Los servicios se han detenido.
pause
goto menu

:iniciar_background
echo.
echo Construyendo y levantando los contenedores (modo segundo plano)...
docker-compose build
docker-compose up -d
echo.
echo Los servicios se están ejecutando en segundo plano.
echo.
echo Puedes acceder a:
echo  - Backend: http://localhost:8000/api/
echo  - Frontend: http://localhost:3000/
echo.
pause
goto menu

:ver_logs
echo.
echo Mostrando logs de los servicios (presiona Ctrl+C para salir)...
echo.
docker-compose logs -f
goto menu

:detener
echo.
echo Deteniendo los servicios...
docker-compose down
echo.
echo Servicios detenidos correctamente.
pause
goto menu

:estado
echo.
echo Estado actual de los servicios:
docker-compose ps
echo.
pause
goto menu

:salir
echo.
echo ¡Gracias por usar OctoFit Tracker!
exit