# Guía de Inicio Rápido OctoFit Tracker

Esta guía proporciona todas las opciones disponibles para iniciar el proyecto OctoFit Tracker, tanto usando Docker como directamente con los scripts locales.

## Opción 1: Iniciar con Docker (Recomendado)

La forma más sencilla y consistente de iniciar todo el entorno de OctoFit Tracker es utilizando Docker y Docker Compose, que configurará automáticamente el backend, frontend y la base de datos.

### Requisitos previos:
- [Docker](https://www.docker.com/products/docker-desktop/) instalado y funcionando
- [Docker Compose](https://docs.docker.com/compose/install/) instalado (viene incluido con Docker Desktop)

### Pasos:

1. Abrir una terminal (PowerShell o CMD) en la carpeta raíz del proyecto

2. Ejecutar el script de inicio de Docker:
   ```powershell
   .\iniciar_docker.bat
   ```

   O directamente con Docker Compose:
   ```powershell
   docker-compose up
   ```

3. Acceder a los servicios:
   - Frontend: [http://localhost:3000](http://localhost:3000)
   - Backend/API: [http://localhost:8000](http://localhost:8000)
   - Endpoints API: [http://localhost:8000/api/](http://localhost:8000/api/)

4. Para detener los servicios:
   Presionar `Ctrl+C` en la terminal o ejecutar:
   ```powershell
   docker-compose down
   ```

## Opción 2: Iniciar servicios por separado (Desarrollo local)

Si prefieres ejecutar los servicios directamente en tu máquina local (útil para desarrollo):

### Backend (Django)

#### Requisitos previos:
- Python 3.8+ instalado
- Dependencias del proyecto instaladas

#### Pasos:

1. **Instalar dependencias** (si aún no lo has hecho):
   ```powershell
   .\backend\instalar_dependencias.bat
   ```
   
   O manualmente:
   ```powershell
   cd backend
   python -m venv venv
   .\venv\Scripts\activate
   pip install -r requirements.txt
   ```

2. **Iniciar el servidor Django**:

   Usando el script de inicio completo:
   ```powershell
   .\backend\iniciar_servidor_completo.bat
   ```
   
   O el script simple:
   ```powershell
   .\backend\iniciar_simple.bat
   ```
   
   O manualmente:
   ```powershell
   cd backend\octofit_tracker
   python manage.py runserver
   ```

3. El servidor estará disponible en [http://localhost:8000](http://localhost:8000)

### Frontend

#### Requisitos previos:
- Node.js y npm instalados

#### Pasos:

1. **Iniciar el servidor de desarrollo**:
   ```powershell
   .\frontend\iniciar_servidor.bat
   ```

   O manualmente:
   ```powershell
   cd frontend
   npm install # solo la primera vez
   npm start
   ```

2. El frontend estará disponible en [http://localhost:3000](http://localhost:3000)

## Solución de problemas

Si encuentras problemas al iniciar los servicios, consulta el documento [docs/solucion_problemas_servidor.md](../docs/solucion_problemas_servidor.md) para encontrar soluciones a problemas comunes.

### Problemas comunes:

1. **Error al iniciar el servidor Django**:
   - Verifica que todas las dependencias estén instaladas con `pip install -r backend/requirements.txt`
   - Comprueba que el entorno virtual esté activado
   - Ejecuta `python manage.py migrate` para asegurar que la base de datos esté actualizada

2. **Conflictos de puertos**:
   - Asegúrate de que los puertos 3000 y 8000 estén disponibles
   - Puedes cambiar los puertos en la configuración si es necesario

3. **Errores de permisos en PowerShell**:
   - Ejecuta PowerShell como administrador
   - Cambia la política de ejecución temporalmente con:
     ```powershell
     Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
     ```

## Opciones adicionales

- **Iniciar solo la base de datos con Docker**:
  ```powershell
  docker-compose up db
  ```

- **Ejecutar las pruebas**:
  ```powershell
  cd backend\octofit_tracker
  python manage.py test
  ```

- **Crear superusuario para el admin**:
  ```powershell
  cd backend\octofit_tracker
  python manage.py createsuperuser
  ```