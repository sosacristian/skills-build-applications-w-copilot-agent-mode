# Recomendaciones y verificaciones para OctoFit Tracker

## 1. Verificación de servicios Docker

Para asegurarte de que todos los servicios estén funcionando correctamente, ejecuta:

```powershell
docker ps
```

Deberías ver tres contenedores en ejecución:
- octofit-tracker-backend-1
- octofit-tracker-db-1
- octofit-tracker-frontend-1

Si falta alguno, puedes verificar los registros específicos:

```powershell
docker logs octofit-tracker-backend-1
docker logs octofit-tracker-frontend-1
```

## 2. Verificación de puertos

Asegúrate de que los puertos necesarios estén libres:

```powershell
netstat -ano | findstr :8000
netstat -ano | findstr :3000
netstat -ano | findstr :5432
```

Si hay servicios utilizando estos puertos, puedes modificar el archivo `docker-compose.yml` para usar puertos diferentes.

## 3. Problemas comunes y soluciones

### Problema con el backend Django

Si el backend no inicia correctamente, puede ser por:

1. **Problemas de dependencias**: Verifica el Dockerfile del backend para asegurarte de que instala todas las dependencias necesarias.
   - Si aparece un error de `ModuleNotFoundError: No module named 'django_filters'`, asegúrate de que django-filter está incluido en requirements.txt y que está registrado en INSTALLED_APPS en settings.py.

2. **Errores de migración**: Puedes ejecutar las migraciones manualmente:
   ```powershell
   docker-compose exec backend sh -c "cd octofit_tracker && python manage.py migrate"
   ```

3. **Errores de configuración**: Verifica que la variable `DJANGO_ALLOWED_HOSTS` en docker-compose.yml incluye `localhost` y `127.0.0.1`.

4. **Errores de indentación en Python**: Presta atención a los errores de `IndentationError`, que son comunes en Python. Revisa el archivo mencionado en el error y corrige cualquier problema de indentación.

### Problema con el frontend

Si el frontend no carga correctamente:

1. **Verificar que el directorio tenga los archivos necesarios**: Asegúrate de que el directorio `frontend` contenga al menos un archivo index.html.

2. **Problema de CORS**: Verifica que en el backend, `CORS_ALLOWED_ORIGINS` incluye `http://localhost:3000`.

## 4. Reinicio limpio

Si continúas teniendo problemas, un reinicio limpio puede ayudar:

```powershell
# Detener y eliminar contenedores
docker-compose down

# Eliminar volúmenes (opcional - borrará la base de datos)
docker-compose down -v

# Reconstruir e iniciar servicios
docker-compose up --build
```

## 5. Verificación manual de componentes

### Backend
- Accede a http://localhost:8000/api/ - Debería mostrar la interfaz de la API
- Intenta http://localhost:8000/admin/ - Debe mostrar la página de administración

### Frontend
- Accede a http://localhost:3000 - Debería mostrar la interfaz de usuario
- Inspecciona la consola del navegador (F12) para ver si hay errores JavaScript

## 6. Solución alternativa: Inicio manual

Si Docker sigue dando problemas, puedes iniciar los servicios manualmente:

1. **Backend**:
   ```powershell
   cd backend/octofit_tracker
   python manage.py runserver
   ```

2. **Frontend** (asumiendo que es una aplicación React o similar):
   ```powershell
   cd frontend
   npm start
   ```

## 7. Ajustes adicionales para resolver problemas

### Si hay problemas con las imágenes Docker

```powershell
# Eliminar todas las imágenes relacionadas con octofit
docker images | findstr octofit
docker rmi <image-id>

# Reconstruir desde cero
docker-compose build --no-cache
docker-compose up
```

### Si hay problemas de permisos o archivos

```powershell
# Verificar permisos de archivos
cd octofit-tracker
icacls * /T
```

## 8. Resumen de URLs importantes

- **Frontend**: http://localhost:3000
- **API Backend**: http://localhost:8000/api/
- **Admin Django**: http://localhost:8000/admin/
- **Documentación API**: http://localhost:8000/api/docs/ (si está configurado)

## 9. Comandos útiles para desarrollo

```powershell
# Ver logs en tiempo real
docker-compose logs -f

# Ejecutar comando en contenedor específico
docker-compose exec backend bash

# Crear superusuario de Django
docker-compose exec backend sh -c "cd octofit_tracker && python manage.py createsuperuser"

# Reiniciar solo un servicio
docker-compose restart backend
```