# Ejemplos de Uso de la API OctoFit Tracker

Este documento proporciona ejemplos prácticos de cómo interactuar con la API de OctoFit Tracker utilizando diferentes herramientas.

## Autenticación

### 1. Registro de Usuario (curl)
```bash
curl -X POST http://localhost:8000/api/register/ \
  -H "Content-Type: application/json" \
  -d '{
    "username": "usuario_ejemplo",
    "email": "usuario@ejemplo.com",
    "password": "contraseña123",
    "confirm_password": "contraseña123"
  }'
```

### 2. Obtener Token JWT (Python)
```python
import requests

response = requests.post(
    'http://localhost:8000/api/token/',
    json={
        'username': 'usuario_ejemplo',
        'password': 'contraseña123'
    }
)

tokens = response.json()
access_token = tokens['access']
refresh_token = tokens['refresh']

# Guardar tokens para futuras peticiones
headers = {
    'Authorization': f'Bearer {access_token}'
}
```

### 3. Refrescar Token (curl)
```bash
curl -X POST http://localhost:8000/api/token/refresh/ \
  -H "Content-Type: application/json" \
  -d '{
    "refresh": "tu_refresh_token"
  }'
```

## Gestión de Perfil

### 1. Obtener Perfil (Python)
```python
import requests

response = requests.get(
    'http://localhost:8000/api/me/profile/',
    headers={'Authorization': f'Bearer {access_token}'}
)

perfil = response.json()
print(f"Nivel actual: {perfil['level']}")
print(f"Puntos: {perfil['points']}")
```

### 2. Actualizar Perfil (curl)
```bash
curl -X PUT http://localhost:8000/api/me/profile/ \
  -H "Authorization: Bearer {tu_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "height": 175,
    "weight": 70,
    "fitness_goal": "GAIN_MUSCLE",
    "activity_level": "MODERATE"
  }'
```

## Gestión de Equipos

### 1. Crear un Equipo (Python)
```python
team_data = {
    'name': 'Equipo Fitness',
    'description': 'Grupo para entrenamiento intenso',
    'is_private': False
}

response = requests.post(
    'http://localhost:8000/api/teams/',
    headers=headers,
    json=team_data
)

team = response.json()
team_id = team['id']
```

### 2. Unirse a un Equipo (curl)
```bash
curl -X POST http://localhost:8000/api/memberships/ \
  -H "Authorization: Bearer {tu_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "team": 1,
    "role": "member"
  }'
```

## Planes de Entrenamiento

### 1. Crear Plan de Entrenamiento (Python)
```python
# Crear plan
plan_data = {
    'name': 'Plan Principiante',
    'description': 'Plan de 4 semanas para principiantes',
    'duration_weeks': 4,
    'difficulty_level': 'BEGINNER'
}

response = requests.post(
    'http://localhost:8000/api/workout-plans/',
    headers=headers,
    json=plan_data
)

plan = response.json()
plan_id = plan['id']

# Añadir ejercicios al plan
exercise_data = {
    'exercise_type': 1,  # ID del tipo de ejercicio
    'sets': 3,
    'reps': 12,
    'duration_minutes': 30,
    'day_of_week': 1  # Lunes
}

requests.post(
    f'http://localhost:8000/api/workout-plans/{plan_id}/exercises/',
    headers=headers,
    json=exercise_data
)
```

## Registro de Actividades

### 1. Registrar una Actividad (curl)
```bash
curl -X POST http://localhost:8000/api/activities/ \
  -H "Authorization: Bearer {tu_token}" \
  -H "Content-Type: application/json" \
  -d '{
    "exercise_type": 1,
    "duration_minutes": 45,
    "calories_burned": 300,
    "date": "2025-10-21",
    "notes": "Buena sesión de entrenamiento"
  }'
```

### 2. Obtener Historial de Actividades (Python)
```python
# Obtener actividades del último mes
from datetime import datetime, timedelta
fecha_inicio = (datetime.now() - timedelta(days=30)).strftime('%Y-%m-%d')

response = requests.get(
    f'http://localhost:8000/api/activities/?date_after={fecha_inicio}',
    headers=headers
)

actividades = response.json()['results']
for actividad in actividades:
    print(f"Fecha: {actividad['date']}")
    print(f"Ejercicio: {actividad['exercise_type']['name']}")
    print(f"Calorías: {actividad['calories_burned']}")
    print("---")
```

## Flujo Completo de Ejemplo

```python
import requests
from datetime import datetime

BASE_URL = 'http://localhost:8000/api'

# 1. Registro de usuario
def register_user():
    response = requests.post(f'{BASE_URL}/register/', json={
        'username': 'nuevo_usuario',
        'email': 'nuevo@ejemplo.com',
        'password': 'contraseña123',
        'confirm_password': 'contraseña123'
    })
    return response.json()

# 2. Obtener token
def get_token():
    response = requests.post(f'{BASE_URL}/token/', json={
        'username': 'nuevo_usuario',
        'password': 'contraseña123'
    })
    return response.json()['access']

# 3. Configurar perfil
def setup_profile(token):
    headers = {'Authorization': f'Bearer {token}'}
    return requests.put(
        f'{BASE_URL}/me/profile/',
        headers=headers,
        json={
            'height': 175,
            'weight': 70,
            'fitness_goal': 'GAIN_MUSCLE',
            'activity_level': 'MODERATE'
        }
    ).json()

# 4. Crear equipo
def create_team(token):
    headers = {'Authorization': f'Bearer {token}'}
    return requests.post(
        f'{BASE_URL}/teams/',
        headers=headers,
        json={
            'name': 'Mi Equipo de Entrenamiento',
            'description': 'Equipo para principiantes',
            'is_private': False
        }
    ).json()

# 5. Registrar actividad
def log_activity(token):
    headers = {'Authorization': f'Bearer {token}'}
    return requests.post(
        f'{BASE_URL}/activities/',
        headers=headers,
        json={
            'exercise_type': 1,
            'duration_minutes': 45,
            'calories_burned': 300,
            'date': datetime.now().strftime('%Y-%m-%d'),
            'notes': 'Primera sesión'
        }
    ).json()

# Ejecutar flujo completo
def main():
    print("1. Registrando usuario...")
    register_user()
    
    print("2. Obteniendo token...")
    token = get_token()
    
    print("3. Configurando perfil...")
    profile = setup_profile(token)
    
    print("4. Creando equipo...")
    team = create_team(token)
    
    print("5. Registrando actividad...")
    activity = log_activity(token)
    
    print("¡Flujo completo ejecutado con éxito!")

if __name__ == '__main__':
    main()
```

## Manejo de Errores

### Ejemplo de manejo de errores en Python
```python
def api_request(method, endpoint, token=None, data=None):
    headers = {}
    if token:
        headers['Authorization'] = f'Bearer {token}'
    
    try:
        response = requests.request(
            method,
            f'http://localhost:8000/api/{endpoint}',
            headers=headers,
            json=data
        )
        response.raise_for_status()
        return response.json()
    except requests.exceptions.HTTPError as e:
        if e.response.status_code == 401:
            print("Error de autenticación. Token inválido o expirado.")
        elif e.response.status_code == 403:
            print("Sin permisos para realizar esta acción.")
        elif e.response.status_code == 404:
            print("Recurso no encontrado.")
        else:
            print(f"Error del servidor: {e.response.json()}")
        return None
    except requests.exceptions.RequestException as e:
        print(f"Error de conexión: {e}")
        return None
```