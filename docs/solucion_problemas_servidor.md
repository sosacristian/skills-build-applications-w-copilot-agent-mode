# Documentación de Solución de Problemas - OctoFit Tracker

> **¡NUEVO!** Ahora puedes usar el script unificado `iniciar.bat` en la raíz del proyecto para seleccionar fácilmente entre todas las opciones de inicio disponibles.

## Problema: Dificultad para iniciar el servidor Django

Este documento detalla los problemas encontrados al iniciar el servidor Django para la aplicación OctoFit Tracker y las soluciones implementadas.

## 1. Problema de dependencias

### Problema identificado:
- Falta de módulos Python necesarios para el proyecto como `rest_framework_simplejwt`, `drf-yasg` y `whitenoise`
- Errores al cargar módulos como `pkg_resources`

### Soluciones implementadas:
1. **Scripts mejorados de instalación de dependencias**:
   - `instalar_dependencias.bat`: Script básico para instalar desde requirements.txt
   - `instalar_paquetes_faltantes.bat`: Script específico para instalar dependencias que faltaban
   - `instalar_exacto.bat`: Script con versiones específicas para garantizar compatibilidad

2. **Actualizaciones en requirements.txt**:
   - Se verificaron todas las dependencias en settings.py y se aseguró que estuvieran en requirements.txt

## 2. Problemas con PowerShell y permisos

### Problema identificado:
- Restricciones de seguridad en PowerShell que impiden la ejecución de scripts
- Error: "El archivo no está firmado digitalmente. No se puede ejecutar este script en el sistema actual."

### Soluciones implementadas:
1. **Uso de CMD en lugar de PowerShell**: 
   - Los scripts batch se ejecutan más consistentemente desde CMD
   
2. **Script PowerShell con manejo de errores**:
   - `iniciar_servidor.ps1`: Versión PowerShell del script con instrucciones para cambiar la política de ejecución

3. **Instrucciones en el README**:
   - Comandos para ajustar las políticas de seguridad: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser`

## 3. Problemas con el entorno virtual

### Problema identificado:
- Dificultades para activar correctamente el entorno virtual
- Python no disponible en la ruta del sistema

### Soluciones implementadas:
1. **Verificación y creación automática del entorno virtual**:
   ```batch
   if not exist "venv\Scripts\activate.bat" (
       echo Creando entorno virtual...
       python -m venv venv
   )
   ```

2. **Activación explícita del entorno virtual**:
   ```batch
   call venv\Scripts\activate.bat
   ```

3. **Scripts de diagnóstico**:
   - `debug_server.bat`: Muestra información sobre versiones instaladas y estructura de directorios

## 4. Solución Docker (Recomendada)

### Ventajas:
- Entorno consistente independiente del sistema operativo
- Evita problemas de dependencias y permisos
- Configuración única que funciona en cualquier máquina

### Implementación:
1. **Docker Compose actualizado**:
   - Servicios configurados: backend (Django), frontend y base de datos
   - Configuración CORS para permitir comunicación entre servicios
   - Volúmenes para persistencia de datos

2. **Dockerfile optimizado**:
   - Base en Python 3.9
   - Instalación de todas las dependencias necesarias
   - Comandos para migraciones y ejecución del servidor

3. **Script de inicio simplificado**:
   - `iniciar_docker.bat`: Instrucciones paso a paso para iniciar todo el entorno

## Comparación de soluciones

| Solución | Ventajas | Desventajas |
|----------|----------|-------------|
| Scripts batch | Simple, familiar | Problemas con permisos y dependencias |
| PowerShell | Más potente que batch | Restricciones de seguridad |
| Entorno virtual | Aísla dependencias | Requiere configuración adicional |
| Docker | Funciona igual en todos lados | Requiere instalar Docker |

## Recomendación final

Para el desarrollo y pruebas de OctoFit Tracker, recomendamos utilizar la solución Docker por su consistencia y facilidad de configuración. Sin embargo, hemos proporcionado múltiples alternativas para adaptarse a diferentes entornos y preferencias de desarrollo.

Los cambios implementados aseguran que, independientemente del método elegido, se puedan resolver los problemas comunes de configuración y dependencias que afectan al inicio del servidor Django.

## Script Unificado de Inicio

Para facilitar el uso de todas las opciones disponibles, hemos creado un script unificado llamado `iniciar.bat` en la raíz del proyecto. Este script proporciona un menú interactivo que permite seleccionar entre:

1. Iniciar todo con Docker (recomendado)
2. Iniciar solo el backend (Django)
3. Iniciar solo el frontend
4. Iniciar todo localmente (sin Docker)
5. Instalar todas las dependencias

Para usarlo, simplemente ejecuta:
```bash
.\iniciar.bat
```

Y sigue las instrucciones en pantalla. Este script simplifica enormemente la puesta en marcha del proyecto, especialmente para nuevos desarrolladores.