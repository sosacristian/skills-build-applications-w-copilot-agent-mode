# Manual Técnico - OctoFit Tracker

## Configuración del Entorno de Desarrollo

### 1. Requisitos del Sistema

#### Software Necesario
- Git 2.x o superior
- Docker Desktop
- VS Code (recomendado)
- Python 3.9 o superior
- Node.js 16.x o superior (para frontend)

#### Extensiones VS Code Recomendadas
- Python
- Docker
- ESLint
- Prettier
- Git Graph
- REST Client

### 2. Estructura del Proyecto en Detalle

```text
octofit-tracker/
├── backend/
│   ├── Dockerfile           # Configuración del contenedor
│   ├── requirements.txt     # Dependencias Python
│   └── octofit_tracker/
│       ├── manage.py       # Script de administración Django
│       ├── core/           # Aplicación principal
│       │   ├── models.py   # Modelos de datos
│       │   ├── views.py    # Lógica de negocio
│       │   ├── urls.py     # URLs de la aplicación
│       │   ├── admin.py    # Configuración admin
│       │   └── tests/      # Tests unitarios
│       └── octofit_tracker/
│           ├── settings.py # Configuración Django
│           ├── urls.py     # URLs principales
│           └── wsgi.py     # Configuración WSGI
├── frontend/               # (Próximamente)
├── docker-compose.yml      # Orquestación de servicios
└── docs/                  # Documentación
    ├── api_endpoints.md    # Especificación API
    ├── estructura_proyecto.md
    ├── guia_instalacion.md
    └── modelos_datos.md
```

### 3. Detalle de Archivos Clave

#### backend/Dockerfile
```dockerfile
FROM python:3.9-slim

WORKDIR /app
COPY requirements.txt .
RUN pip install -r requirements.txt

COPY . .
EXPOSE 8000

CMD ["python", "octofit_tracker/manage.py", "runserver", "0.0.0.0:8000"]
```

#### docker-compose.yml
```yaml
version: '3.8'

services:
  backend:
    build: ./backend
    volumes:
      - ./backend:/app
    ports:
      - "8000:8000"
    environment:
      - DEBUG=1
      - DJANGO_ALLOWED_HOSTS=localhost,127.0.0.1
```

#### settings.py (Configuraciones Importantes)
```python
INSTALLED_APPS = [
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    'rest_framework',
    'corsheaders',
    'core',
]

REST_FRAMEWORK = {
    'DEFAULT_AUTHENTICATION_CLASSES': [
        'rest_framework_simplejwt.authentication.JWTAuthentication',
    ],
    'DEFAULT_PERMISSION_CLASSES': [
        'rest_framework.permissions.IsAuthenticated',
    ],
}

CORS_ALLOWED_ORIGINS = [
    "http://localhost:3000",
]
```

## Guías de Desarrollo

### 1. Convenciones de Código

#### Python (Django)
```python
# Ejemplo de modelo
class Activity(models.Model):
    """
    Representa una actividad física realizada por un usuario.
    
    Atributos:
        user (User): Usuario que realizó la actividad
        exercise_type (ExerciseType): Tipo de ejercicio realizado
        duration (int): Duración en minutos
        calories_burned (int): Calorías quemadas estimadas
    """
    user = models.ForeignKey(User, on_delete=models.CASCADE)
    exercise_type = models.ForeignKey(ExerciseType, on_delete=models.PROTECT)
    duration = models.IntegerField(help_text="Duración en minutos")
    calories_burned = models.IntegerField()
    
    def calculate_calories(self):
        """Calcula las calorías quemadas basado en el tipo de ejercicio y duración."""
        return self.exercise_type.calories_per_hour * (self.duration / 60)
```

#### JavaScript (React - Próximamente)
```javascript
// Ejemplo de componente React
const ActivityCard = ({ activity }) => {
  const { duration, caloriesBurned, exerciseType } = activity;
  
  return (
    <Card>
      <CardHeader title={exerciseType.name} />
      <CardContent>
        <Typography>
          Duración: {duration} minutos
        </Typography>
        <Typography>
          Calorías: {caloriesBurned}
        </Typography>
      </CardContent>
    </Card>
  );
};
```

### 2. Manejo de Base de Datos

#### Migraciones
```bash
# Crear nuevas migraciones
python manage.py makemigrations

# Aplicar migraciones
python manage.py migrate

# Revertir migración
python manage.py migrate core 0001_initial
```

#### Comandos SQL Útiles
```sql
-- Consulta de actividades por usuario
SELECT 
    u.username,
    a.date,
    e.name as exercise,
    a.duration,
    a.calories_burned
FROM 
    core_activity a
    JOIN auth_user u ON a.user_id = u.id
    JOIN core_exercisetype e ON a.exercise_type_id = e.id
WHERE 
    u.username = 'usuario1'
ORDER BY 
    a.date DESC;
```

### 3. Testing

#### Tests Unitarios
```python
from django.test import TestCase
from django.contrib.auth.models import User
from core.models import Activity, ExerciseType

class ActivityTests(TestCase):
    def setUp(self):
        self.user = User.objects.create_user(
            username='testuser',
            password='testpass123'
        )
        self.exercise_type = ExerciseType.objects.create(
            name='Running',
            calories_per_hour=600
        )

    def test_calculate_calories(self):
        """Test que las calorías se calculan correctamente."""
        activity = Activity.objects.create(
            user=self.user,
            exercise_type=self.exercise_type,
            duration=30
        )
        self.assertEqual(activity.calculate_calories(), 300)
```

#### Tests de API
```python
from rest_framework.test import APITestCase
from rest_framework import status

class ActivityAPITests(APITestCase):
    def setUp(self):
        self.user = User.objects.create_user(
            username='testuser',
            password='testpass123'
        )
        self.client.force_authenticate(user=self.user)

    def test_create_activity(self):
        """Test de creación de actividad vía API."""
        url = '/api/activities/'
        data = {
            'exercise_type': 1,
            'duration': 30,
            'date': '2025-10-20'
        }
        response = self.client.post(url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
```

### 4. Gestión de Dependencias

#### Python
```txt
# requirements.txt
Django==4.1.7
djangorestframework==3.14.0
django-cors-headers==4.5.0
django-allauth==0.51.0
dj-rest-auth==2.2.6
```

#### Node.js (Próximamente)
```json
{
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "@mui/material": "^5.14.0",
    "redux": "^4.2.0",
    "react-redux": "^8.1.0"
  }
}
```

### 5. Seguridad

#### Configuración de JWT
```python
# settings.py
SIMPLE_JWT = {
    'ACCESS_TOKEN_LIFETIME': timedelta(minutes=60),
    'REFRESH_TOKEN_LIFETIME': timedelta(days=1),
    'ROTATE_REFRESH_TOKENS': False,
    'BLACKLIST_AFTER_ROTATION': True,
}
```

#### Middleware de Seguridad
```python
MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'corsheaders.middleware.CorsMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
]

SECURE_SSL_REDIRECT = True  # En producción
SESSION_COOKIE_SECURE = True
CSRF_COOKIE_SECURE = True
```

### 6. Performance

#### Optimización de Queries
```python
# Uso de select_related para relaciones ForeignKey
activities = Activity.objects.select_related('user', 'exercise_type').all()

# Uso de prefetch_related para relaciones ManyToMany
teams = Team.objects.prefetch_related('members').all()
```

#### Caché
```python
# settings.py
CACHES = {
    'default': {
        'BACKEND': 'django.core.cache.backends.redis.RedisCache',
        'LOCATION': 'redis://redis:6379/1',
    }
}

# Vista con caché
from django.views.decorators.cache import cache_page

@cache_page(60 * 15)  # Cache por 15 minutos
def leaderboard_view(request):
    # ... lógica de la vista
```

### 7. Logging

#### Configuración
```python
# settings.py
LOGGING = {
    'version': 1,
    'disable_existing_loggers': False,
    'handlers': {
        'file': {
            'level': 'DEBUG',
            'class': 'logging.FileHandler',
            'filename': 'debug.log',
        },
    },
    'loggers': {
        'django': {
            'handlers': ['file'],
            'level': 'DEBUG',
            'propagate': True,
        },
    },
}
```

#### Uso en Código
```python
import logging
logger = logging.getLogger(__name__)

def some_view(request):
    try:
        # ... lógica
        logger.info('Operación exitosa')
    except Exception as e:
        logger.error(f'Error: {str(e)}')
```

## Guías de Despliegue

### 1. Preparación
```bash
# Construir imágenes
docker compose build

# Verificar configuración
python manage.py check --deploy
```

### 2. Base de Datos
```bash
# Backup
python manage.py dumpdata > backup.json

# Restore
python manage.py loaddata backup.json
```

### 3. Archivos Estáticos
```bash
# Recolectar archivos estáticos
python manage.py collectstatic
```

### 4. Monitoreo
```python
# Middleware de Performance
MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'stats_middleware.StatsMiddleware',  # Personalizado
]
```

## Solución de Problemas

### 1. Problemas Comunes

#### Error de Migración
```bash
# Resetear migraciones
find . -path "*/migrations/*.py" -not -name "__init__.py" -delete
find . -path "*/migrations/*.pyc" -delete
```

#### Error de Permisos Docker
```bash
# Arreglar permisos
sudo chown -R $USER:$USER .
```

### 2. Debugging

#### Django Debug Toolbar
```python
INSTALLED_APPS += ['debug_toolbar']
MIDDLEWARE += ['debug_toolbar.middleware.DebugToolbarMiddleware']
INTERNAL_IPS = ['127.0.0.1']
```

#### Logging Extendido
```python
LOGGING['handlers']['console'] = {
    'level': 'DEBUG',
    'class': 'logging.StreamHandler',
    'formatter': 'verbose'
}
```