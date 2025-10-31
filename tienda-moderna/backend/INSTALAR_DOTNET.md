# 🚨 .NET 8 SDK NO DETECTADO

## ¿Qué hacer?

### Opción 1: Descargar e Instalar (RECOMENDADO)

1. **Descargar .NET 8 SDK**:
   - Ir a: https://dotnet.microsoft.com/download/dotnet/8.0
   - Descargar: ".NET 8.0 SDK (v8.0.xxx)" para Windows x64
   - **NO descargar** solo el "Runtime", necesitas el **SDK completo**

2. **Instalar**:
   - Ejecutar el instalador descargado
   - Seguir el asistente (Next → Next → Install)
   - Esperar a que termine la instalación

3. **Verificar**:
   - **IMPORTANTE**: Cerrar y reabrir PowerShell/VS Code
   - Ejecutar: `dotnet --version`
   - Debería mostrar: `8.0.xxx`

### Opción 2: Instalar via Winget (Windows 10/11)

```powershell
winget install Microsoft.DotNet.SDK.8
```

Después de instalar, **REINICIAR** PowerShell o VS Code.

---

## ✅ Mientras Tanto...

**El código ya está listo:**
- ✅ 27 archivos de código creados
- ✅ Domain Layer completo
- ✅ Estructura de proyectos lista
- ✅ Archivos .csproj configurados

**Solo falta .NET SDK para compilar.**

---

## 🔧 Después de Instalar .NET

Ejecuta estos comandos:

```powershell
# 1. Verificar instalación
dotnet --version

# 2. Ir al directorio backend
cd tienda-moderna/backend

# 3. Restaurar paquetes NuGet
dotnet restore

# 4. Compilar todo
dotnet build

# Si todo está OK, deberías ver:
# Build succeeded.
#     0 Warning(s)
#     0 Error(s)
```

---

## 🆘 Si Sigue Sin Funcionar

### Verificar PATH manualmente:

1. **Abrir Variables de Entorno**:
   - Win + R → `sysdm.cpl` → Enter
   - Pestaña "Opciones avanzadas"
   - Botón "Variables de entorno"

2. **Verificar PATH**:
   - En "Variables del sistema", buscar `Path`
   - Debe contener: `C:\Program Files\dotnet\`

3. **Si no está**:
   - Editar → Nuevo → Agregar: `C:\Program Files\dotnet`
   - OK → OK → OK

4. **Reiniciar PowerShell/VS Code**

---

## 📝 Notas

- La instalación de .NET SDK toma ~5 minutos
- Ocupa ~500 MB de espacio
- Es necesario reiniciar la terminal después de instalar
- VS Code también necesita reiniciarse

---

## ✨ Una Vez Instalado

Avísame y:
1. Verificaremos que compile correctamente
2. Continuaremos con Infrastructure Layer (DbContext, repositorios)
3. Haremos la primera migración de base de datos

---

**¿Necesitas ayuda?**
- Revisa: `docs/CUANDO_INSTALES_DOTNET.md`
- O pregunta cualquier duda
