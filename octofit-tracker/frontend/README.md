# OctoFit Tracker - Cliente Frontend de Prueba

Este directorio contiene una interfaz sencilla para probar la API de OctoFit Tracker.

## Características

- Interfaz simple basada en HTML, Bootstrap y JavaScript
- Permite interactuar con todos los endpoints principales de la API
- No requiere instalación de dependencias
- Útil para pruebas y demostración de la funcionalidad

## Uso

### Método 1: Usando un servidor web local

1. Inicia el servidor backend de Django:
   ```
   cd ../backend
   source venv/bin/activate  # En Windows: venv\Scripts\activate
   python octofit_tracker/manage.py runserver
   ```

2. Abre el archivo `index.html` usando un servidor web local.
   Puedes utilizar la extensión "Live Server" de VS Code o cualquier otro servidor web local.

### Método 2: Usando Python como servidor web

1. Inicia el servidor backend de Django como se mostró anteriormente.

2. Abre una nueva terminal y navega a la carpeta frontend:
   ```
   cd octofit-tracker/frontend
   ```

3. Inicia un servidor web simple con Python:
   ```
   # Python 3.x
   python -m http.server 3000
   ```

4. Abre tu navegador y accede a `http://localhost:3000`

## Secciones principales

### 1. Autenticación
- Registro de nuevos usuarios
- Inicio de sesión para obtener un token JWT

### 2. Perfil
- Ver información del perfil actual
- Actualizar datos como altura, peso y objetivos

### 3. Equipos
- Ver lista de equipos disponibles
- Crear nuevos equipos
- Unirse a equipos existentes

### 4. Actividades
- Registrar actividades de ejercicio
- Ver historial de actividades

### 5. Ejercicios
- Ver catálogo de tipos de ejercicio
- Ver planes de entrenamiento disponibles

## Consideraciones CORS

Si encuentras problemas de CORS al acceder a la API desde este cliente, asegúrate de que en el backend:

1. Tienes instalado `django-cors-headers`
2. Has añadido `corsheaders` a las aplicaciones instaladas en `settings.py`
3. Has añadido `corsheaders.middleware.CorsMiddleware` a la lista de middleware
4. Has configurado los orígenes permitidos:

```python
# En settings.py
CORS_ALLOWED_ORIGINS = [
    "http://localhost:3000",
    "http://127.0.0.1:3000",
]
```

## Desarrollo futuro

Esta interfaz es solo para propósitos de prueba. Para una aplicación en producción, se recomienda:

1. Implementar un frontend completo con React, Vue o Angular
2. Usar una librería de gestión de estado como Redux
3. Implementar un sistema de rutas apropiado
4. Mejorar la seguridad del manejo de tokens
5. Añadir validaciones más robustas