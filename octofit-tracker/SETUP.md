# Guía de Configuración de OctoFit Tracker

Esta guía proporciona instrucciones detalladas para configurar el entorno de desarrollo de OctoFit Tracker.

## Requisitos Previos

Asegúrate de tener instalado:

- Python 3.8+ (recomendado 3.10)
- pip (gestor de paquetes de Python)
- Git
- PostgreSQL (opcional, SQLite es la opción predeterminada)

## Configuración del Backend

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu_usuario/octofit-tracker.git
cd octofit-tracker
```

### 2. Crear Entorno Virtual

#### En Windows (PowerShell):

```powershell
python -m venv octofit-tracker\backend\venv
octofit-tracker\backend\venv\Scripts\Activate.ps1

# Si hay problemas de permisos:
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
octofit-tracker\backend\venv\Scripts\Activate.ps1
```

#### En Windows (CMD):

```cmd
python -m venv octofit-tracker\backend\venv
octofit-tracker\backend\venv\Scripts\activate.bat
```

#### En Linux/macOS:

```bash
python3 -m venv octofit-tracker/backend/venv
source octofit-tracker/backend/venv/bin/activate
```

### 3. Instalar Dependencias

```bash
pip install -r octofit-tracker/backend/requirements.txt
```

### 4. Variables de Entorno

Crea un archivo `.env` en la carpeta `octofit-tracker/backend/octofit_tracker/`:

```
SECRET_KEY=django-insecure-key-for-development-only
DEBUG=1
DB_ENGINE=django.db.backends.sqlite3
DB_NAME=db.sqlite3
```

Para usar PostgreSQL:

```
DB_ENGINE=django.db.backends.postgresql
DB_NAME=octofit_db
DB_USER=postgres
DB_PASSWORD=tu_contraseña
DB_HOST=localhost
DB_PORT=5432
```

### 5. Migraciones de Base de Datos

```bash
cd octofit-tracker/backend
python octofit_tracker/manage.py makemigrations
python octofit_tracker/manage.py migrate
```

### 6. Crear Superusuario

```bash
python octofit_tracker/manage.py createsuperuser
```

### 7. Cargar Datos Iniciales (opcional)

```bash
python octofit_tracker/manage.py loaddata octofit_tracker/core/fixtures/initial_data.json
```

### 8. Ejecutar Servidor de Desarrollo

```bash
python octofit_tracker/manage.py runserver
```

El servidor estará disponible en `http://127.0.0.1:8000/`.

## Pruebas

### Ejecutar Tests

```bash
python octofit_tracker/manage.py test core.tests
```

### Cobertura de Código (opcional)

```bash
pip install coverage
coverage run --source='.' octofit_tracker/manage.py test core.tests
coverage report
```

## Acceso a la API

### Documentación

- Swagger UI: `http://127.0.0.1:8000/swagger/`
- ReDoc: `http://127.0.0.1:8000/redoc/`

### Ejemplos de Uso

Para interactuar con la API, puedes usar herramientas como:

- [Postman](https://www.postman.com/)
- [Insomnia](https://insomnia.rest/)
- curl (ejemplos en la documentación)

## Solución de Problemas

### Error al activar el entorno virtual en PowerShell

Si encuentras errores relacionados con la política de ejecución en PowerShell:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

### Errores de migración de base de datos

Si encuentras errores de migración:

```bash
python octofit_tracker/manage.py migrate --fake-initial
```

### Errores de dependencias

En caso de conflictos de dependencias:

```bash
pip install --upgrade -r octofit-tracker/backend/requirements.txt
```

## Recursos Adicionales

- [Documentación de Django](https://docs.djangoproject.com/)
- [Documentación de Django REST Framework](https://www.django-rest-framework.org/)
- [Documentación de JWT](https://django-rest-framework-simplejwt.readthedocs.io/)
