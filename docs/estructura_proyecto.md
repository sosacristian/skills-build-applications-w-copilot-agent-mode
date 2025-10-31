# Documentación del Proyecto OctoFit Tracker

## Estructura del Proyecto

```
octofit-tracker/
├── backend/               # Servidor Django
│   ├── Dockerfile        # Configuración de contenedor para backend
│   ├── requirements.txt  # Dependencias de Python
│   └── octofit_tracker/  # Proyecto Django principal
│       ├── manage.py     # Script de administración Django
│       ├── core/         # Aplicación principal
│       │   ├── models.py # Modelos de datos
│       │   ├── views.py  # Vistas y lógica de negocio
│       │   └── admin.py  # Configuración del panel admin
│       └── octofit_tracker/
│           ├── settings.py # Configuración del proyecto
│           └── urls.py     # Enrutamiento de URLs
├── frontend/             # (Pendiente de implementar)
├── docker-compose.yml    # Orquestación de servicios
└── SETUP.md             # Instrucciones de configuración
```

## Configuración Docker

El proyecto utiliza Docker para la gestión de entornos de desarrollo. La configuración actual incluye:

### Backend (Django)
- Puerto: 8000
- Volumen montado para desarrollo en tiempo real
- Migraciones automáticas al inicio
- Variables de entorno configuradas para desarrollo

## Estado Actual del Proyecto

1. **Backend Django**
   - Configuración básica completada
   - Base de datos SQLite configurada
   - Sistema de autenticación listo para usar
   - Panel de administración disponible en `/admin`

2. **Docker**
   - Contenedor de backend funcionando
   - Volúmenes configurados para desarrollo
   - Puertos mapeados correctamente

3. **Próximos Pasos**
   - Implementar modelos de datos
   - Crear endpoints API REST
   - Desarrollar frontend
   - Configurar autenticación de usuarios

## Acceso al Proyecto

- Panel de administración: http://localhost:8000/admin/
- API (pendiente): http://localhost:8000/api/

Esta documentación se actualizará a medida que el proyecto evolucione.