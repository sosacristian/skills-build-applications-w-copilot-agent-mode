@echo off
echo ================================================
echo Iniciando Servidor Frontend para OctoFit Tracker
echo ================================================
echo.

cd %~dp0

echo Este servidor resolverá los problemas de CORS que pueden ocurrir
echo al abrir el archivo index.html directamente en el navegador.
echo.
echo El frontend estará disponible en: http://localhost:3000
echo Para detener el servidor, presiona Ctrl+C
echo.

:: Verificar si Node.js está instalado (opción preferida)
where node >nul 2>nul
if %ERRORLEVEL% EQU 0 (
    echo Detectado Node.js. Intentando iniciar con http-server...
    npx --yes http-server . -p 3000 --cors
    if %ERRORLEVEL% EQU 0 goto fin
    echo No se pudo iniciar http-server. Probando con Python...
)

:: Verificar si Python está instalado
python --version > nul 2>&1
if %errorlevel% equ 0 (
    echo Iniciando servidor con Python...
    echo.
    echo IMPORTANTE: Si el backend de Django está corriendo, asegúrate
    echo de que la URL de la API en el frontend esté configurada como:
    echo http://localhost:8000
    echo.
    python -m http.server 3000
    goto fin
) else (
    echo Python no encontrado. Intentando con py...
    py --version > nul 2>&1
    if %errorlevel% equ 0 (
        echo Iniciando servidor con py...
        echo.
        echo IMPORTANTE: Si el backend de Django está corriendo, asegúrate
        echo de que la URL de la API en el frontend esté configurada como:
        echo http://localhost:8000
        echo.
        py -m http.server 3000
        goto fin
    ) else (
        echo No se pudo encontrar ni Python ni Node.js.
        echo.
        echo ADVERTENCIA: Al abrir el archivo directamente puede que la aplicación
        echo no funcione correctamente debido a restricciones de seguridad CORS.
        echo.
        echo Opciones recomendadas:
        echo 1. Instala Python o Node.js
        echo 2. Usa la extensión "Live Server" de VS Code
        echo 3. Usa Docker con el script iniciar_docker.bat
        echo.
        echo Abriendo index.html directamente en el navegador...
        explorer index.html
        pause
    )
)

:fin
pause