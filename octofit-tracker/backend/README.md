# OctoFit Tracker Backend

## Instrucciones para iniciar el servidor

> **¡NUEVO!** Ahora puedes usar el script unificado `iniciar.bat` en la carpeta raíz del proyecto para seleccionar fácilmente entre todas las opciones de inicio disponibles.

### Opción 1: Usando el script unificado (Recomendado)
1. Navega a la carpeta raíz del proyecto OctoFit
2. Ejecuta `iniciar.bat`
3. Selecciona la opción 2 "Iniciar solo el backend (Django)"

### Opción 2: Usando el script batch (Windows)
1. Haz doble clic en el archivo `iniciar_servidor_completo.bat` o `iniciar_simple.bat`
2. El script verificará e instalará las dependencias necesarias
3. El servidor se iniciará en http://localhost:8000

### Opción 2: Usando PowerShell (Windows)
1. Abre PowerShell como administrador
2. Ejecuta el siguiente comando para permitir la ejecución de scripts:
   ```
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```
3. Navega al directorio del backend
4. Ejecuta el script:
   ```
   .\iniciar_servidor.ps1
   ```

### Opción 3: Usando comandos manuales
1. Abre una terminal/cmd
2. Navega al directorio del backend
3. Activa el entorno virtual:
   ```
   # Windows
   venv\Scripts\activate
   
   # Linux/Mac
   source venv/bin/activate
   ```
4. Instala las dependencias (si es necesario):
   ```
   pip install -r requirements.txt
   ```
5. Ejecuta el servidor:
   ```
   python octofit_tracker/manage.py runserver 0.0.0.0:8000
   ```

## Solución de problemas

### El servidor no inicia
- Verifica que Python esté instalado y en el PATH
- Comprueba que el entorno virtual esté activado
- Instala manualmente las dependencias: `pip install -r requirements.txt`
- Ejecuta las migraciones: `python octofit_tracker/manage.py migrate`

### Error CORS al acceder desde el frontend
- Verifica que el servidor esté configurado para aceptar solicitudes desde el origen del frontend
- La configuración actual permite todas las solicitudes CORS para facilitar el desarrollo

## Endpoints API
El servidor ofrece una API REST con los siguientes endpoints principales:
- `/api/users/` - Gestión de usuarios
- `/api/teams/` - Gestión de equipos
- `/api/activities/` - Registro y consulta de actividades
- `/api/exercise-types/` - Tipos de ejercicios disponibles

Para más detalles, consulta la documentación de la API en `/api/docs/` cuando el servidor esté en ejecución.