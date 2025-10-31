@echo off
echo ================================================
echo Iniciando servidor Django de OctoFit Tracker
echo ================================================
echo.

cd %~dp0

:: Verificar si existe el entorno virtual
if not exist "venv\Scripts\activate.bat" (
    echo ERROR: El entorno virtual no se encuentra.
    echo Creando entorno virtual...
    python -m venv venv
    
    if %ERRORLEVEL% NEQ 0 (
        echo ERROR: No se pudo crear el entorno virtual.
        echo Asegúrate de que Python esté instalado correctamente.
        goto :error
    )
)

:: Activar entorno virtual
echo Activando entorno virtual...
call venv\Scripts\activate.bat

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: No se pudo activar el entorno virtual.
    goto :error
)

:: Instalar dependencias si es necesario
echo.
echo Verificando dependencias...
pip freeze | findstr "Django"
if %ERRORLEVEL% NEQ 0 (
    echo Instalando dependencias desde requirements.txt...
    pip install -r requirements.txt
    
    if %ERRORLEVEL% NEQ 0 (
        echo ERROR: No se pudieron instalar las dependencias.
        goto :error
    )
)

:: Verificar estructura del proyecto
echo.
echo Verificando estructura del proyecto...
if not exist "octofit_tracker\manage.py" (
    echo ERROR: No se encuentra el archivo manage.py.
    echo Ubicación actual: %CD%
    goto :error
)

:: Comprobar la configuración de Django
echo.
echo Ejecutando comprobación de Django...
python octofit_tracker\manage.py check

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: La comprobación de Django ha fallado.
    goto :error
)

:: Ejecutar migraciones si es necesario
echo.
echo Ejecutando migraciones...
python octofit_tracker\manage.py migrate

if %ERRORLEVEL% NEQ 0 (
    echo ADVERTENCIA: Las migraciones no se han completado correctamente.
)

:: Iniciar el servidor
echo.
echo Iniciando servidor Django...
echo.
python octofit_tracker\manage.py runserver 0.0.0.0:8000

if %ERRORLEVEL% NEQ 0 goto :error
goto :end

:error
echo.
echo ¡Ha ocurrido un error! Por favor, revisa los mensajes anteriores.
echo.
echo Consejos de solución de problemas:
echo 1. Asegúrate de que Python esté instalado y en el PATH
echo 2. Verifica que todas las dependencias estén instaladas
echo 3. Comprueba que la estructura del proyecto sea correcta
echo 4. Revisa los archivos de configuración de Django (settings.py)
echo.

:end
pause