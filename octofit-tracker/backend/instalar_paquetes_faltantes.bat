@echo off
echo Instalando dependencias faltantes...

cd %~dp0
call venv\Scripts\activate.bat

echo Instalando djangorestframework-simplejwt...
pip install djangorestframework-simplejwt

echo Instalando drf-yasg...
pip install drf-yasg

echo Instalando djangorestframework...
pip install djangorestframework

echo Instalando django-cors-headers...
pip install django-cors-headers

echo Instalando whitenoise...
pip install whitenoise

echo.
echo Instalación completada. Ahora ejecuta iniciar_servidor.bat

pause