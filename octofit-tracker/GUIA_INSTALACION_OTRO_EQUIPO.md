# Guía para probar OctoFit Tracker en otro equipo

Esta guía te ayudará a ejecutar la aplicación OctoFit Tracker en un nuevo equipo desde cero.

## Requisitos previos

Antes de comenzar, asegúrate de que el nuevo equipo tenga instalado:

1. **Git** - Para clonar el repositorio
   - [Descargar Git](https://git-scm.com/downloads)

2. **Docker Desktop** - Para ejecutar los contenedores
   - [Descargar Docker Desktop](https://www.docker.com/products/docker-desktop/)
   - Asegúrate de que Docker Desktop esté en ejecución antes de continuar

## Pasos para la instalación

### 1. Clonar el repositorio

Abre una terminal (PowerShell o CMD) y ejecuta:

```powershell
# Clonar el repositorio
git clone https://github.com/sosacristian/skills-build-applications-w-copilot-agent-mode.git

# Navegar al directorio del proyecto
cd skills-build-applications-w-copilot-agent-mode
```

### 2. Iniciar la aplicación con Docker

```powershell
# Navegar a la carpeta del proyecto OctoFit Tracker
cd octofit-tracker

# Iniciar todos los servicios con Docker Compose
docker-compose up
```

Este comando iniciará todos los servicios necesarios:
- Backend (Django)
- Frontend
- Base de datos PostgreSQL

> **Nota**: La primera vez que ejecutes este comando, Docker descargará las imágenes necesarias y construirá los contenedores, lo que puede tomar varios minutos dependiendo de la velocidad de tu conexión a internet.

### 3. Verificar que los servicios están funcionando

Una vez que todos los servicios se hayan iniciado correctamente, podrás acceder a la aplicación a través de:

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:8000/api/
- **Admin Django**: http://localhost:8000/admin/

## Solución de problemas comunes

### Si Docker no puede iniciar los servicios

1. Verifica que Docker Desktop está en ejecución
2. Asegúrate de que los puertos 3000, 8000 y 5432 no estén siendo utilizados por otras aplicaciones
3. Intenta reiniciar Docker Desktop

### Si alguno de los servicios falla al iniciar

Puedes ver los logs específicos de cada servicio:

```powershell
# Ver logs del backend
docker-compose logs backend

# Ver logs del frontend
docker-compose logs frontend

# Ver logs de la base de datos
docker-compose logs db
```

### Errores comunes y soluciones

1. **Error de conexión a la base de datos**:
   - Asegúrate de que el servicio de base de datos está funcionando:
     ```powershell
     docker-compose ps db
     ```

2. **Error en migraciones de Django**:
   - Ejecuta las migraciones manualmente:
     ```powershell
     docker-compose exec backend sh -c "cd octofit_tracker && python manage.py migrate"
     ```

3. **Error de CORS en el frontend**:
   - Verifica la configuración de CORS en el backend:
     ```powershell
     docker-compose exec backend sh -c "cat octofit_tracker/octofit_tracker/settings.py | grep CORS"
     ```

## Detener la aplicación

Cuando hayas terminado de probar la aplicación, puedes detener todos los servicios presionando `Ctrl+C` en la terminal donde se está ejecutando Docker Compose, o ejecutando:

```powershell
docker-compose down
```

Si quieres eliminar también los volúmenes de datos (esto borrará la base de datos):

```powershell
docker-compose down -v
```

## Crear datos de prueba (opcional)

Para crear algunos datos de prueba en la aplicación:

```powershell
# Crear un superusuario de Django
docker-compose exec backend sh -c "cd octofit_tracker && python manage.py createsuperuser"

# Sigue las instrucciones para crear un usuario administrador
```

Luego puedes acceder al panel de administración en http://localhost:8000/admin/ con las credenciales que acabas de crear y añadir datos de prueba.

## Recursos adicionales

Para obtener más información sobre el proyecto, consulta:
- [VERIFICACIONES_Y_SOLUCIONES.md](./VERIFICACIONES_Y_SOLUCIONES.md) - Guía detallada de solución de problemas
- [GUIA_INICIO_RAPIDO.md](./GUIA_INICIO_RAPIDO.md) - Opciones alternativas de inicio
- [docs/api_endpoints.md](../docs/api_endpoints.md) - Documentación de la API