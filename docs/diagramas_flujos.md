# Diagramas y Flujos de Usuario - OctoFit Tracker

## Diagramas de Casos de Uso

### 1. Gestión de Usuarios
```mermaid
graph TD
    U[Usuario] 
    A[Administrador]
    
    U -->|Registrarse| R[Registro]
    U -->|Iniciar Sesión| L[Login]
    U -->|Editar Perfil| P[Perfil]
    U -->|Ver Estadísticas| S[Estadísticas]
    
    A -->|Gestionar Usuarios| M[Gestión Usuarios]
    A -->|Ver Estadísticas| AS[Estadísticas Admin]
    A -->|Configurar Sistema| C[Configuración]
```

### 2. Gestión de Actividades
```mermaid
graph TD
    U[Usuario] -->|Registrar Actividad| R[Nueva Actividad]
    U -->|Ver Historial| H[Historial]
    U -->|Editar Actividad| E[Editar]
    U -->|Eliminar Actividad| D[Eliminar]
    U -->|Ver Progreso| P[Progreso]
    
    R -->|Seleccionar Tipo| T[Tipo Ejercicio]
    R -->|Ingresar Duración| DU[Duración]
    R -->|Calcular Calorías| C[Calorías]
```

### 3. Gestión de Equipos
```mermaid
graph TD
    U[Usuario] -->|Crear Equipo| C[Nuevo Equipo]
    U -->|Unirse a Equipo| J[Unirse]
    U -->|Ver Equipos| V[Ver Equipos]
    U -->|Gestionar Miembros| M[Gestión Miembros]
    
    A[Administrador Equipo] -->|Invitar Miembros| I[Invitar]
    A -->|Eliminar Miembros| D[Eliminar]
    A -->|Editar Equipo| E[Editar]
```

## Flujos de Usuario

### 1. Registro e Inicio de Sesión
```mermaid
sequenceDiagram
    participant U as Usuario
    participant F as Frontend
    participant B as Backend
    participant DB as Base de Datos
    
    U->>F: Accede a registro
    F->>B: POST /api/auth/register
    B->>DB: Validar datos
    DB-->>B: OK
    B->>DB: Crear usuario
    DB-->>B: Usuario creado
    B-->>F: 201 Created
    F-->>U: Registro exitoso
    
    U->>F: Iniciar sesión
    F->>B: POST /api/auth/login
    B->>DB: Verificar credenciales
    DB-->>B: OK
    B-->>F: Token JWT
    F-->>U: Redirigir a dashboard
```

### 2. Registro de Actividad
```mermaid
sequenceDiagram
    participant U as Usuario
    participant F as Frontend
    participant B as Backend
    participant DB as Base de Datos
    
    U->>F: Nueva actividad
    F->>B: GET /api/exercises
    B->>DB: Obtener tipos
    DB-->>B: Lista ejercicios
    B-->>F: Tipos ejercicios
    U->>F: Selecciona ejercicio
    U->>F: Ingresa duración
    F->>B: POST /api/activities
    B->>DB: Guardar actividad
    DB-->>B: OK
    B-->>F: 201 Created
    F-->>U: Actividad registrada
```

### 3. Gestión de Equipos
```mermaid
sequenceDiagram
    participant U as Usuario
    participant F as Frontend
    participant B as Backend
    participant DB as Base de Datos
    
    U->>F: Crear equipo
    F->>B: POST /api/teams
    B->>DB: Crear equipo
    DB-->>B: Equipo creado
    B-->>F: 201 Created
    F-->>U: Equipo creado
    
    U->>F: Invitar miembro
    F->>B: POST /api/teams/{id}/invite
    B->>DB: Crear invitación
    DB-->>B: OK
    B-->>F: Invitación enviada
    F-->>U: Confirma envío
```

## Flujo de Datos

### 1. Arquitectura General
```mermaid
graph TD
    subgraph Cliente
        R[React Frontend]
        RS[Redux Store]
    end
    
    subgraph Servidor
        D[Django Backend]
        DRF[Django REST Framework]
    end
    
    subgraph Base de Datos
        SQL[SQLite/PostgreSQL]
    end
    
    R <-->|HTTP/REST| DRF
    R <-->|Estado| RS
    DRF <-->|ORM| D
    D <-->|Queries| SQL
```

### 2. Flujo de Autenticación
```mermaid
graph TD
    subgraph Frontend
        L[Login Form]
        T[Token Storage]
        H[HTTP Client]
    end
    
    subgraph Backend
        A[Auth API]
        J[JWT Service]
        V[Validator]
    end
    
    L -->|Credentials| H
    H -->|POST /auth/login| A
    A -->|Validate| V
    V -->|Generate Token| J
    J -->|JWT Token| A
    A -->|Token Response| H
    H -->|Store Token| T
```

## Estados de Usuario

### 1. Máquina de Estados de Actividad
```mermaid
stateDiagram-v2
    [*] --> EnReposo
    EnReposo --> EnActividad: Iniciar
    EnActividad --> Pausado: Pausar
    Pausado --> EnActividad: Reanudar
    Pausado --> Completado: Finalizar
    EnActividad --> Completado: Finalizar
    Completado --> [*]
```

### 2. Máquina de Estados de Equipo
```mermaid
stateDiagram-v2
    [*] --> Creado
    Creado --> Activo: Miembros > 1
    Activo --> EnCompetencia: Iniciar desafío
    EnCompetencia --> Activo: Finalizar desafío
    Activo --> Inactivo: Sin actividad
    Inactivo --> Activo: Nueva actividad
    Inactivo --> [*]: Eliminar
```

## Modelo de Datos

### 1. Diagrama Entidad-Relación
```mermaid
erDiagram
    USER ||--o{ PROFILE : has
    USER ||--o{ ACTIVITY : logs
    USER ||--o{ TEAM_MEMBERSHIP : has
    TEAM ||--o{ TEAM_MEMBERSHIP : contains
    ACTIVITY ||--|| EXERCISE_TYPE : is_of
    USER ||--o{ WORKOUT_PLAN : has
    WORKOUT_PLAN ||--o{ EXERCISE : contains
    EXERCISE ||--|| EXERCISE_TYPE : is_of
```

### 2. Jerarquía de Componentes Frontend
```mermaid
graph TD
    A[App] --> N[Navbar]
    A --> D[Dashboard]
    A --> P[Profile]
    
    D --> AC[ActivityCard]
    D --> TC[TeamCard]
    D --> SC[StatsCard]
    
    P --> PF[ProfileForm]
    P --> AH[ActivityHistory]
    P --> TL[TeamList]
```

## Procesos de Negocio

### 1. Cálculo de Puntos
```mermaid
graph TD
    A[Actividad Completada] -->|Registrar| B[Calcular Duración]
    B -->|Multiplicar| C[Puntos Base]
    C -->|Aplicar| D[Multiplicadores]
    D -->|Sumar| E[Puntos Usuario]
    D -->|Sumar| F[Puntos Equipo]
    
    D -->|Si Desafío Activo| G[Puntos Desafío]
    G -->|Al Completar| H[Rewards]
```

### 2. Sistema de Logros
```mermaid
graph TD
    A[Nueva Actividad] -->|Verificar| B{Cumple Criterios?}
    B -->|Sí| C[Desbloquear Logro]
    C -->|Notificar| D[Usuario]
    C -->|Actualizar| E[Perfil]
    
    B -->|No| F[Actualizar Progreso]
    F -->|Si Complete| B
```