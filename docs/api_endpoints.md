# Documentación de la API de OctoFit Tracker

## Autenticación

### POST /api/register/
Registro de nuevos usuarios
```json
{
    "username": "string",
    "email": "string",
    "password": "string",
    "confirm_password": "string"
}
```

### POST /api/token/
Obtener token de acceso
```json
{
    "username": "string",
    "password": "string"
}
```

### POST /api/token/refresh/
Refrescar token de acceso
```json
{
    "refresh": "string"
}
```

### POST /api/auth/login/
Inicio de sesión
```json
{
    "username": "string",
    "password": "string"
}
```

## Usuario y Perfil

### GET /api/me/
Obtener información del usuario actual
```json
{
    "id": "integer",
    "username": "string",
    "email": "string",
    "first_name": "string",
    "last_name": "string"
}
```

### GET /api/me/profile/
Obtener perfil detallado del usuario actual
```json
{
    "user": "integer",
    "height": "float",
    "weight": "float",
    "fitness_goal": "string",
    "activity_level": "string",
    "points": "integer",
    "level": "integer"
}
```

### PUT /api/me/profile/
Actualizar perfil de usuario
```json
{
    "height": "float",
    "weight": "float",
    "fitness_goal": "string",
    "activity_level": "string"
}
```

## Actividades

### GET /api/activities/
Listar actividades del usuario

### POST /api/activities/
Registrar nueva actividad
```json
{
    "type": "string",
    "duration": "integer",
    "calories_burned": "integer",
    "date": "date",
    "notes": "string"
}
```

### GET /api/activities/{id}/
Obtener detalles de una actividad específica

### PUT /api/activities/{id}/
Actualizar una actividad existente

## Equipos

### GET /api/teams/
Listar equipos disponibles

### POST /api/teams/
Crear nuevo equipo
```json
{
    "name": "string",
    "description": "string",
    "is_private": "boolean"
}
```

### POST /api/teams/{id}/join/
Unirse a un equipo

### GET /api/teams/{id}/members/
Listar miembros del equipo

## Teams (Equipos)

### GET /api/teams/
Listar todos los equipos
```json
{
    "count": "integer",
    "next": "string",
    "previous": "string",
    "results": [
        {
            "id": "integer",
            "name": "string",
            "description": "string",
            "is_private": "boolean",
            "created_at": "datetime",
            "members_count": "integer",
            "total_points": "integer"
        }
    ]
}
```

### POST /api/teams/
Crear nuevo equipo
```json
{
    "name": "string",
    "description": "string",
    "is_private": "boolean"
}
```

### GET /api/teams/{id}/
Obtener detalles de un equipo

### PUT /api/teams/{id}/
Actualizar información del equipo (solo administradores)

### DELETE /api/teams/{id}/
Eliminar equipo (solo administradores)

## Membresías de Equipo

### GET /api/memberships/
Listar membresías del usuario actual
```json
{
    "results": [
        {
            "id": "integer",
            "team": {
                "id": "integer",
                "name": "string"
            },
            "role": "string",
            "joined_at": "datetime"
        }
    ]
}
```

### POST /api/memberships/
Unirse a un equipo
```json
{
    "team": "integer",
    "role": "string"  // "member" o "admin"
}
```

### DELETE /api/memberships/{id}/
Abandonar un equipo

## Exercise Types (Tipos de Ejercicio)

### GET /api/exercise-types/
Listar tipos de ejercicio disponibles
```json
{
    "results": [
        {
            "id": "integer",
            "name": "string",
            "description": "string",
            "category": "string",
            "difficulty_level": "string",
            "calories_per_hour": "integer"
        }
    ]
}
```

### POST /api/exercise-types/
Crear nuevo tipo de ejercicio (solo administradores)
```json
{
    "name": "string",
    "description": "string",
    "category": "string",
    "difficulty_level": "string",
    "calories_per_hour": "integer"
}
```

## Workout Plans (Planes de Entrenamiento)

### GET /api/workout-plans/
Listar planes de entrenamiento
```json
{
    "results": [
        {
            "id": "integer",
            "name": "string",
            "description": "string",
            "duration_weeks": "integer",
            "difficulty_level": "string",
            "created_by": "integer",
            "exercises_count": "integer"
        }
    ]
}
```

### POST /api/workout-plans/
Crear nuevo plan
```json
{
    "name": "string",
    "description": "string",
    "duration_weeks": "integer",
    "difficulty_level": "string"
}
```

### GET /api/workout-plans/{id}/exercises/
Listar ejercicios de un plan
```json
{
    "results": [
        {
            "id": "integer",
            "exercise_type": "integer",
            "sets": "integer",
            "reps": "integer",
            "duration_minutes": "integer",
            "day_of_week": "integer"
        }
    ]
}
```

## Activities (Actividades)

### GET /api/activities/
Listar actividades del usuario
```json
{
    "results": [
        {
            "id": "integer",
            "exercise_type": {
                "id": "integer",
                "name": "string"
            },
            "duration_minutes": "integer",
            "calories_burned": "integer",
            "date": "date",
            "notes": "string",
            "workout_plan": "integer"
        }
    ]
}
```

### POST /api/activities/
Registrar nueva actividad
```json
{
    "exercise_type": "integer",
    "duration_minutes": "integer",
    "calories_burned": "integer",
    "date": "date",
    "notes": "string",
    "workout_plan": "integer"  // opcional
}
```

## Filtros y Paginación

Todos los endpoints que devuelven listas soportan:

### Paginación
```
?page=1&page_size=10
```

### Filtros Comunes
- Actividades:
  ```
  ?date_after=2025-01-01
  ?date_before=2025-12-31
  ?exercise_type=1
  ```
- Teams:
  ```
  ?search=nombre
  ?is_private=true
  ```
- Exercise Types:
  ```
  ?category=cardio
  ?difficulty_level=beginner
  ```

## Códigos de Estado HTTP

- 200: OK - Solicitud exitosa
- 201: Created - Recurso creado
- 204: No Content - Solicitud exitosa sin contenido (DELETE)
- 400: Bad Request - Error en los datos enviados
- 401: Unauthorized - No autenticado
- 403: Forbidden - No autorizado
- 404: Not Found - Recurso no encontrado
- 500: Internal Server Error - Error del servidor
```json
{
    "difficulty": "string", // "beginner", "intermediate", "advanced"
    "duration": "integer"   // duración deseada en minutos
}
```

## Formatos de Respuesta

Todas las respuestas seguirán este formato base:
```json
{
    "success": "boolean",
    "data": {},
    "message": "string"
}
```

## Códigos de Estado HTTP

- 200: Éxito
- 201: Creado exitosamente
- 400: Error de validación
- 401: No autorizado
- 403: Prohibido
- 404: No encontrado
- 500: Error interno del servidor

## Autenticación

Todas las rutas (excepto login y registro) requieren un token JWT en el encabezado:
```
Authorization: Bearer {token}
```