@echo off
echo Iniciando servidor Django en modo simple...

cd %~dp0
call venv\Scripts\activate.bat

echo.
echo Iniciando servidor...
python octofit_tracker\manage.py runserver 0.0.0.0:8000

pause