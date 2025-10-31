@echo off
echo ======================================================
echo    INSTALACIÓN DE DEPENDENCIAS OCTOFIT TRACKER
echo ======================================================
echo.
echo Instalando dependencias de Django...
echo.

cd %~dp0

REM Verificar si existe el entorno virtual, si no, crearlo
if not exist "venv\Scripts\activate.bat" (
    echo Creando entorno virtual...
    python -m venv venv
)

echo Activando entorno virtual...
call venv\Scripts\activate.bat

echo.
echo Actualizando pip...
python -m pip install --upgrade pip

echo.
echo Instalando dependencias desde requirements.txt...
pip install -r requirements.txt

echo.
echo Asegurando que todas las dependencias críticas estén instaladas...
pip install Django==4.1.7
pip install djangorestframework==3.14.0
pip install django-cors-headers==4.5.0
pip install djangorestframework-simplejwt
pip install dj-rest-auth==2.2.6
pip install setuptools
pip install wheel

echo.
echo Verificando las dependencias instaladas...
pip freeze

echo.
echo Verificando la instalación de Django...
python -c "import django; print('Django version:', django.get_version())"

echo.
echo Instalación completada. Ya puedes iniciar el servidor con:
echo - iniciar_servidor_completo.bat: para iniciar con todas las configuraciones
echo - iniciar_simple.bat: para una versión simplificada
echo - O usa iniciar.bat en la raíz del proyecto para ver todas las opciones
echo.

pause