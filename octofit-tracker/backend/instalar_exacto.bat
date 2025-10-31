@echo off
echo ================================================
echo Instalando dependencias específicas para OctoFit Tracker
echo ================================================
echo.

cd %~dp0

:: Activar entorno virtual
call venv\Scripts\activate.bat

:: Instalar dependencias exactas
echo.
echo Instalando Django y paquetes básicos...
pip install Django==4.1.7
pip install djangorestframework==3.14.0

echo.
echo Instalando paquetes de autenticación...
pip install djangorestframework-simplejwt==5.3.0
pip install dj-rest-auth==2.2.6
pip install django-allauth==0.51.0

echo.
echo Instalando utilidades y paquetes adicionales...
pip install django-cors-headers==4.5.0
pip install whitenoise==6.5.0
pip install drf-yasg==1.21.7
pip install Pillow==9.5.0
pip install pymongo==3.12.0
pip install sqlparse==0.2.4

echo.
echo Verificando instalación...
pip freeze

echo.
echo Instalación completada. Ahora puedes ejecutar el servidor.
echo.
echo Para iniciar el servidor, ejecuta: iniciar_servidor.bat

pause