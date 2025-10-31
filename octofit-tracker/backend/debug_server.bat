@echo off
echo Iniciando depuración del servidor Django...
echo.

cd %~dp0
call venv\Scripts\activate.bat

echo.
echo Verificando versiones instaladas:
python --version
pip --version
pip freeze

echo.
echo Verificando estructura de directorios:
dir
dir octofit_tracker

echo.
echo Probando Django:
python octofit_tracker\manage.py check

echo.
echo Intentando iniciar el servidor:
python octofit_tracker\manage.py runserver 0.0.0.0:8000

pause