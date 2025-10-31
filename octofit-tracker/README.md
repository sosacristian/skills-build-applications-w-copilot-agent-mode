# OctoFit Tracker

OctoFit Tracker es una aplicación de seguimiento de fitness que permite a los usuarios registrar actividades, unirse a equipos y seguir planes de entrenamiento personalizados.

## Características

- **Autenticación de Usuarios**: Registro, login y gestión de perfiles.
- **Seguimiento de Actividades**: Registro detallado de ejercicios y actividades físicas.
- **Equipos y Competencia**: Creación y gestión de equipos para motivación social.
- **Planes de Entrenamiento**: Creación y seguimiento de rutinas estructuradas.
- **Estadísticas y Progreso**: Visualización del progreso personal a través del tiempo.

## Estructura del Proyecto

```
octofit-tracker/
├── backend/               # API REST Django
│   ├── venv/              # Entorno virtual
│   ├── requirements.txt   # Dependencias Python
│   └── octofit_tracker/   # Proyecto Django
│       ├── core/          # Aplicación principal
│       │   ├── models.py      # Modelos de datos
│       │   ├── views.py       # Vistas API
│       │   ├── serializers.py # Serializers REST
│       │   ├── urls.py        # Endpoints API
│       │   └── tests/         # Tests unitarios
│       └── octofit_tracker/   # Configuración Django
└── frontend/              # Cliente React (en desarrollo)
```

## Requisitos

- Python 3.8+
- Django 4.1.7
- PostgreSQL o SQLite3
- Dependencias en `requirements.txt`

## Instalación

1. Clonar el repositorio:
```bash
git clone https://github.com/tu_usuario/octofit-tracker.git
cd octofit-tracker
```

2. Crear y activar entorno virtual:
```bash
# En Linux/macOS
python3 -m venv backend/venv
source backend/venv/bin/activate

# En Windows
python -m venv backend\venv
backend\venv\Scripts\activate
```

3. Instalar dependencias:
```bash
pip install -r backend/requirements.txt
```

### Problemas comunes en Windows

Si tienes problemas con la activación del entorno virtual en PowerShell debido a restricciones de seguridad, puedes:

1. **Usar los scripts de inicio proporcionados**:
   - Para el backend: Ejecuta `backend\iniciar_servidor.bat`
   - Para el frontend: Ejecuta `frontend\iniciar_servidor.bat`

2. **Cambiar la política de ejecución** (requiere permisos de administrador):
   ```powershell
   Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
   ```

3. **Usar CMD en lugar de PowerShell**:
   ```cmd
   cd backend
   venv\Scripts\activate.bat
   python octofit_tracker\manage.py runserver
   ```

4. Configurar variables de entorno (opcional):
```
# Crear archivo .env en backend/octofit_tracker/
SECRET_KEY=tu_clave_secreta
DEBUG=1
DB_ENGINE=django.db.backends.sqlite3  # o postgresql
DB_NAME=db.sqlite3
```

5. Ejecutar migraciones:
```bash
cd backend
python octofit_tracker/manage.py migrate
```

6. Crear superusuario:
```bash
python octofit_tracker/manage.py createsuperuser
```

7. Iniciar servidor:
```bash
python octofit_tracker/manage.py runserver
```

## API REST

La documentación completa de la API está disponible en:
- `/swagger/` - Documentación Swagger
- `/redoc/` - Documentación ReDoc

### Endpoints principales:

- **Autenticación**:
  - POST `/api/register/` - Registro de usuarios
  - POST `/api/token/` - Obtener token JWT
  - POST `/api/token/refresh/` - Refrescar token

- **Usuarios**:
  - GET `/api/me/` - Perfil del usuario actual
  - PUT `/api/me/profile/` - Actualizar perfil

- **Equipos**:
  - GET/POST `/api/teams/` - Listar/crear equipos
  - GET/PUT/DELETE `/api/teams/{id}/` - Detalles de equipo

- **Actividades**:
  - GET/POST `/api/activities/` - Listar/registrar actividades
  - GET/PUT/DELETE `/api/activities/{id}/` - Detalles de actividad

- **Planes de entrenamiento**:
  - GET/POST `/api/workout-plans/` - Listar/crear planes
  - GET/POST `/api/workout-plans/{id}/exercises/` - Ejercicios del plan

## Pruebas

Para ejecutar las pruebas:

```bash
cd backend
python octofit_tracker/manage.py test core.tests
```

## Contribución

1. Fork el repositorio
2. Crea una rama para tu feature (`git checkout -b feature/amazing-feature`)
3. Commit tus cambios (`git commit -m 'Add some amazing feature'`)
4. Push a tu rama (`git push origin feature/amazing-feature`)
5. Abre un Pull Request

## Licencia

Distribuido bajo la Licencia MIT. Ver `LICENSE` para más información.

## Contacto

Tu Nombre - [@tu_twitter](https://twitter.com/tu_twitter) - email@ejemplo.com

Link del Proyecto: [https://github.com/tu_usuario/octofit-tracker](https://github.com/tu_usuario/octofit-tracker)