# Guía para Iniciar y Usar la Aplicación OctoFit Tracker

Esta guía explica paso a paso cómo iniciar y utilizar la aplicación OctoFit Tracker.

## 1. Iniciando la aplicación

### Opción 1: Usando Docker (Recomendado)

Docker proporciona el entorno más consistente y fácil de configurar para ejecutar la aplicación OctoFit Tracker.

1. Navega al directorio raíz del proyecto:
   ```
   cd octofit-tracker
   ```

2. Ejecuta el script de inicio de Docker:
   ```
   iniciar_docker.bat
   ```

3. Selecciona la opción 2 para iniciar los servicios en segundo plano

4. Accede a la aplicación:
   - Backend API: http://localhost:8000/api/
   - Frontend: http://localhost:3000/

### Opción 2: Inicio directo con scripts

1. Ejecuta el script de inicio del servidor backend:
   ```
   cd octofit-tracker\backend
   iniciar_servidor.bat   # Script principal
   ```
   
   Si encuentras problemas, prueba con:
   ```
   iniciar_todo.bat       # Script con instalación de dependencias
   ```

2. Abrir el archivo `index.html` del frontend en tu navegador:
   - Si usas VS Code con la extensión "Live Server", haz clic derecho en el archivo y selecciona "Open with Live Server"
   - O usa el archivo batch para abrir el frontend: 
     ```
     cd octofit-tracker\frontend
     iniciar_servidor.bat
     ```

### Opción 3: Inicio manual (si las opciones anteriores fallan)

1. Abre CMD (no PowerShell)

2. Navega al directorio del backend:
   ```
   cd octofit-tracker\backend
   ```

3. Activa el entorno virtual e inicia el servidor:
   ```
   venv\Scripts\activate
   python octofit_tracker\manage.py runserver 0.0.0.0:8000
   ```

4. Abre el frontend en tu navegador

## 2. Interfaz principal

Al abrir la interfaz, verás:

La interfaz está organizada en pestañas:
- **Autenticación**: Para registro e inicio de sesión
- **Perfil**: Para ver y actualizar tu perfil
- **Equipos**: Para gestionar equipos
- **Actividades**: Para registrar y ver actividades
- **Ejercicios**: Para explorar tipos de ejercicio y planes

En la parte superior hay un área para configurar la URL base de la API y el token JWT.

## 3. Registro e inicio de sesión

Primero debes registrarte:

1. En la pestaña "Autenticación", completa el formulario de registro:
   - Usuario (ej: "usuario_prueba")
   - Email (ej: "prueba@ejemplo.com")
   - Contraseña y confirmación (ej: "contraseña123")

2. Haz clic en "Registrarse"
   - Verás la respuesta de la API en la parte inferior
   - Si es exitoso, verás un mensaje de éxito

3. Luego, inicia sesión con el usuario que creaste:
   - Ingresa el nombre de usuario y contraseña
   - Haz clic en "Iniciar Sesión"
   - La API devolverá un token JWT que se guardará automáticamente

## 4. Gestión del perfil

Una vez iniciada la sesión:

1. Ve a la pestaña "Perfil"
2. Haz clic en "Obtener Perfil" para ver tus datos actuales
3. Completa o actualiza tus datos:
   - Altura (cm)
   - Peso (kg)
   - Objetivo (perder peso, ganar músculo, etc.)
   - Nivel de actividad
4. Haz clic en "Actualizar" para guardar los cambios

## 5. Gestión de equipos

En la pestaña "Equipos" puedes:

1. **Ver equipos**:
   - Haz clic en "Obtener Equipos" para ver los equipos disponibles
   - Verás tanto tus equipos como los públicos

2. **Crear un equipo**:
   - Completa el nombre y descripción
   - Marca la casilla si deseas que sea privado
   - Haz clic en "Crear Equipo"

3. **Unirte a un equipo**:
   - Ingresa el ID del equipo al que deseas unirte
   - Haz clic en "Unirse"

## 6. Registro de actividades

En la pestaña "Actividades":

1. **Ver actividades**:
   - Haz clic en "Obtener Actividades" para ver tu historial

2. **Registrar una actividad**:
   - Selecciona el tipo de ejercicio (se carga automáticamente al iniciar sesión)
   - Ingresa la duración en minutos
   - Ingresa las calorías quemadas
   - Selecciona la fecha (por defecto es hoy)
   - Añade notas opcionales
   - Haz clic en "Registrar"

## 7. Explorar ejercicios

En la pestaña "Ejercicios":

1. **Ver tipos de ejercicio**:
   - Haz clic en "Obtener Tipos" para ver el catálogo de ejercicios

2. **Ver planes de entrenamiento**:
   - Haz clic en "Obtener Planes" para ver los planes disponibles

## 8. Respuestas de la API

Cada vez que realizas una acción:
- La respuesta de la API se muestra en el área "Respuesta de la API" en la parte inferior
- Las respuestas están formateadas en JSON para facilitar su lectura
- Algunos datos también se muestran en el área específica de cada sección

## 9. Solución de problemas comunes

### Error "Python no encontrado" o "No se puede ejecutar este script"
Este es un problema común con PowerShell en Windows:

1. **Solución inmediata**: Ejecutar PowerShell como administrador y cambiar temporalmente la política de ejecución:
   ```powershell
   Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
   ```
   Esto sólo afecta la sesión actual de PowerShell.

2. **Solución permanente para el entorno virtual**:
   ```powershell
   Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
   ```
   Esto permite la ejecución de scripts locales sin restricciones.

3. **Alternativa usando cmd**: Usa el Símbolo del Sistema (cmd.exe) en lugar de PowerShell:
   ```cmd
   cd octofit-tracker\backend
   venv\Scripts\activate.bat
   python octofit_tracker\manage.py runserver
   ```

4. **Usar Python directamente sin activar el entorno**:
   ```powershell
   cd octofit-tracker\backend
   .\venv\Scripts\python.exe .\octofit_tracker\manage.py runserver
   ```
   
5. **Python no está en el PATH**: Si el error indica "no se encontró Python", asegúrate de que Python está instalado y agregado al PATH del sistema, o usa la ruta completa al ejecutable de Python.

### Error "CORS Policy"
Si ves errores relacionados con CORS en la consola del navegador:
1. Asegúrate de que el backend esté ejecutándose
2. Verifica que la URL de la API sea correcta (por defecto: http://localhost:8000/api)
3. Confirma que en el backend esté configurado CORS para permitir peticiones desde tu origen

### Error "401 Unauthorized"
Si ves errores de autorización:
1. Asegúrate de haber iniciado sesión correctamente
2. Verifica que el token JWT se haya guardado en el campo correspondiente
3. El token puede haber expirado - intenta iniciar sesión nuevamente

### No se cargan los datos
Si al hacer clic en los botones no se cargan datos:
1. Abre la consola del navegador (F12) para ver errores específicos
2. Verifica que el backend esté ejecutándose
3. Confirma que la URL de la API sea correcta

## 10. Próximos pasos

Una vez que hayas probado todas las funcionalidades básicas, puedes:

1. Crear varios usuarios para probar las funcionalidades de equipo
2. Crear un plan de entrenamiento completo con varios ejercicios
3. Registrar actividades durante varias fechas para probar las estadísticas
4. Probar las funcionalidades de filtrado añadiendo parámetros a las URLs