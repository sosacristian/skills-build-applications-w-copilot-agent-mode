# Guía de Instalación - OctoFit Tracker

## Requisitos Previos

1. **Docker Desktop**
   - Windows: [Descargar Docker Desktop para Windows](https://www.docker.com/products/docker-desktop)
   - macOS: [Descargar Docker Desktop para Mac](https://www.docker.com/products/docker-desktop)
   - Linux: Seguir las [instrucciones oficiales](https://docs.docker.com/engine/install/)

2. **Git**
   - Descargar e instalar desde [git-scm.com](https://git-scm.com/)

## Pasos de Instalación

1. **Clonar el Repositorio**
   ```bash
   git clone https://github.com/sosacristian/skills-build-applications-w-copilot-agent-mode.git
   cd skills-build-applications-w-copilot-agent-mode
   ```

2. **Configurar Variables de Entorno**
   - Copiar el archivo de ejemplo de variables de entorno:
     ```bash
     cp octofit-tracker/backend/.env.example octofit-tracker/backend/.env
     ```
   - Editar el archivo `.env` con tus configuraciones

3. **Iniciar los Contenedores**
   ```bash
   cd octofit-tracker
   docker compose up -d
   ```

4. **Crear Superusuario (Admin)**
   ```bash
   docker compose exec backend python octofit_tracker/manage.py createsuperuser
   ```
   - Seguir las instrucciones en pantalla para crear el usuario administrador

## Verificación de la Instalación

1. **Comprobar que los servicios están funcionando**
   ```bash
   docker compose ps
   ```
   Deberías ver los contenedores `backend` y `frontend` (cuando se implemente) en estado "Up"

2. **Acceder a la aplicación**
   - Backend: http://localhost:8000/admin/
   - API Docs: http://localhost:8000/api/docs/ (cuando se implemente)
   - Frontend: http://localhost:3000/ (cuando se implemente)

## Comandos Útiles

### Gestión de Contenedores
```bash
# Ver logs de los contenedores
docker compose logs

# Detener los contenedores
docker compose down

# Reconstruir contenedores (después de cambios en Dockerfile)
docker compose up -d --build
```

### Comandos Django
```bash
# Crear migraciones
docker compose exec backend python octofit_tracker/manage.py makemigrations

# Aplicar migraciones
docker compose exec backend python octofit_tracker/manage.py migrate

# Abrir shell de Django
docker compose exec backend python octofit_tracker/manage.py shell
```

## Solución de Problemas

### Los contenedores no inician
1. Verificar que Docker Desktop está ejecutándose
2. Comprobar los logs con `docker compose logs`
3. Asegurarse de que los puertos 8000 y 3000 no están en uso

### Error de permisos en Windows
1. Asegurarse de que Docker Desktop tiene acceso al directorio del proyecto
2. Ejecutar Docker Desktop como administrador

### Problemas de migración de base de datos
1. Eliminar archivo de base de datos SQLite si existe
2. Eliminar archivos de migraciones (excepto \_\_init\_\_.py)
3. Recrear migraciones y aplicarlas

## Desarrollo

### Estructura del Proyecto
```
octofit-tracker/
├── backend/         # API Django
├── frontend/        # Cliente React (pendiente)
└── docs/           # Documentación
```

### Flujo de Trabajo
1. Crear una rama para nuevas características
2. Desarrollar y probar localmente
3. Ejecutar pruebas
4. Crear pull request

## Recursos Adicionales

- [Documentación de Django](https://docs.djangoproject.com/)
- [Documentación de Docker](https://docs.docker.com/)
- [Guía de Contribución](./CONTRIBUTING.md) (pendiente)

## Soporte

Para reportar problemas o solicitar ayuda:
1. Crear un issue en el repositorio
2. Proporcionar logs relevantes
3. Describir los pasos para reproducir el problema