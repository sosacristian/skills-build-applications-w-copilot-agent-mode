:::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: OctoFit Tracker - Servidor Django
:: Este script actualiza todas las dependencias y
:: inicia el servidor Django.
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

@echo off
setlocal EnableDelayedExpansion

echo ================================================
echo Iniciando OctoFit Tracker Backend
echo ================================================

cd %~dp0

:: Activar entorno virtual
echo Activando entorno virtual...
call venv\Scripts\activate.bat

:: Instalar dependencias críticas
echo.
echo Instalando dependencias críticas...
pip install setuptools wheel
pip install Django==4.1.7
pip install djangorestframework==3.14.0
pip install djangorestframework-simplejwt==5.3.0
pip install django-cors-headers==4.5.0
pip install whitenoise==6.5.0
pip install drf-yasg==1.21.7

:: Intentar iniciar el servidor
echo.
echo Iniciando servidor Django...
echo Puedes acceder a él en: http://localhost:8000
echo.
echo Presiona Ctrl+C para detener el servidor
echo.
python octofit_tracker\manage.py runserver 0.0.0.0:8000

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Error al iniciar el servidor.
    echo.
    echo Probando modo de solución de problemas...
    echo.
    echo Verificando configuración de Django:
    python octofit_tracker\manage.py check
    
    echo.
    echo Si continúas teniendo problemas, prueba lo siguiente:
    echo 1. Ejecuta: python -m pip install --upgrade pip
    echo 2. Elimina la carpeta 'venv' y vuelve a crearla: python -m venv venv
    echo 3. Reinstala todas las dependencias: pip install -r requirements.txt
)

pause