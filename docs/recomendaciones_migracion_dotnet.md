# Recomendaciones para Migrar OctoFit Tracker a .NET

Este documento proporciona una guía para implementar OctoFit Tracker utilizando tecnologías .NET en lugar de Django.

## Estructura del Proyecto en .NET

```
OctoFitTracker/
├── OctoFitTracker.API/              # Proyecto API Web (ASP.NET Core)
├── OctoFitTracker.Core/             # Biblioteca de clases (Lógica de negocio y modelos)
├── OctoFitTracker.Infrastructure/    # Biblioteca de clases (Acceso a datos, servicios externos)
├── OctoFitTracker.Tests/            # Proyecto de pruebas (xUnit, NUnit o MSTest)
└── OctoFitTracker.sln               # Solución de Visual Studio
```

## Equivalencias de Tecnologías

| Django/Python           | Equivalente en .NET                      |
|-------------------------|-----------------------------------------|
| Django                  | ASP.NET Core                            |
| Django REST Framework   | ASP.NET Core Web API                    |
| JWT Authentication      | Microsoft.AspNetCore.Authentication.JwtBearer |
| PostgreSQL/SQLite       | Entity Framework Core + SQL Server/PostgreSQL |
| Python venv             | .NET SDK con múltiples versiones        |
| Requirements.txt        | .csproj con dependencias NuGet          |
| Django ORM              | Entity Framework Core                   |

## Cambios en los Modelos de Datos

### 1. Definición de Modelos

**Django:**
```python
class Team(models.Model):
    name = models.CharField(max_length=100)
    description = models.TextField()
    is_private = models.BooleanField(default=False)
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)
```

**C# (.NET):**
```csharp
public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navegación EF Core
    public virtual ICollection<TeamMembership> Memberships { get; set; }
}
```

### 2. Contexto de Base de Datos

```csharp
public class OctoFitDbContext : DbContext
{
    public OctoFitDbContext(DbContextOptions<OctoFitDbContext> options)
        : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMembership> TeamMemberships { get; set; }
    public DbSet<ExerciseType> ExerciseTypes { get; set; }
    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Activity> Activities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de relaciones y restricciones
        modelBuilder.Entity<Profile>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<Profile>(p => p.UserId);
            
        // Más configuraciones...
    }
}
```

## Autenticación y Autorización

### 1. Configuración de JWT

```csharp
// En Program.cs o Startup.cs
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
```

### 2. Generación de Token

```csharp
// En un servicio de autenticación
public string GenerateJwtToken(User user)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
    
    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: credentials
    );
    
    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

## API Controllers

### 1. Controller Básico

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;
    
    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var teams = await _teamService.GetTeamsForUserAsync(userId);
        return Ok(teams);
    }
    
    [HttpPost]
    public async Task<ActionResult<TeamDto>> CreateTeam(CreateTeamDto teamDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var team = await _teamService.CreateTeamAsync(userId, teamDto);
        return CreatedAtAction(nameof(GetTeam), new { id = team.Id }, team);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDto>> GetTeam(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var team = await _teamService.GetTeamByIdAsync(id, userId);
        
        if (team == null)
            return NotFound();
            
        return Ok(team);
    }
    
    // Más endpoints...
}
```

## Servicios y Repositorios

### 1. Interfaz de Servicio

```csharp
public interface ITeamService
{
    Task<IEnumerable<TeamDto>> GetTeamsForUserAsync(int userId);
    Task<TeamDto> GetTeamByIdAsync(int teamId, int userId);
    Task<TeamDto> CreateTeamAsync(int userId, CreateTeamDto teamDto);
    Task<TeamDto> UpdateTeamAsync(int teamId, int userId, UpdateTeamDto teamDto);
    Task DeleteTeamAsync(int teamId, int userId);
}
```

### 2. Implementación de Servicio

```csharp
public class TeamService : ITeamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public TeamService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<TeamDto>> GetTeamsForUserAsync(int userId)
    {
        var userTeams = await _unitOfWork.TeamRepository.GetTeamsByUserIdAsync(userId);
        var publicTeams = await _unitOfWork.TeamRepository.GetPublicTeamsAsync();
        
        var allTeams = userTeams.Union(publicTeams);
        return _mapper.Map<IEnumerable<TeamDto>>(allTeams);
    }
    
    // Más implementaciones...
}
```

### 3. Repositorio

```csharp
public class TeamRepository : ITeamRepository
{
    private readonly OctoFitDbContext _context;
    
    public TeamRepository(OctoFitDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Team>> GetTeamsByUserIdAsync(int userId)
    {
        return await _context.TeamMemberships
            .Where(m => m.UserId == userId)
            .Select(m => m.Team)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Team>> GetPublicTeamsAsync()
    {
        return await _context.Teams
            .Where(t => !t.IsPrivate)
            .ToListAsync();
    }
    
    // Más implementaciones...
}
```

## Validación y DTOs

### 1. DTOs (Data Transfer Objects)

```csharp
public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MembersCount { get; set; }
    public int TotalPoints { get; set; }
}

public class CreateTeamDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public bool IsPrivate { get; set; }
}

public class UpdateTeamDto
{
    [StringLength(100)]
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public bool? IsPrivate { get; set; }
}
```

### 2. AutoMapper

```csharp
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Team, TeamDto>()
            .ForMember(dest => dest.MembersCount, opt => 
                opt.MapFrom(src => src.Memberships.Count))
            .ForMember(dest => dest.TotalPoints, opt => 
                opt.MapFrom(src => src.Memberships.Sum(m => m.User.Profile.Points)));
                
        CreateMap<CreateTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
        // Más mapeos...
    }
}
```

## Testing

### 1. Pruebas Unitarias (xUnit)

```csharp
public class TeamServiceTests
{
    private readonly ITeamService _teamService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    
    public TeamServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _teamService = new TeamService(_unitOfWorkMock.Object, _mapperMock.Object);
    }
    
    [Fact]
    public async Task GetTeamsForUserAsync_ReturnsTeams()
    {
        // Arrange
        var userId = 1;
        var teams = new List<Team> { new Team { Id = 1, Name = "Test Team" } };
        var teamDtos = new List<TeamDto> { new TeamDto { Id = 1, Name = "Test Team" } };
        
        _unitOfWorkMock.Setup(uow => uow.TeamRepository.GetTeamsByUserIdAsync(userId))
            .ReturnsAsync(teams);
        _unitOfWorkMock.Setup(uow => uow.TeamRepository.GetPublicTeamsAsync())
            .ReturnsAsync(new List<Team>());
        _mapperMock.Setup(m => m.Map<IEnumerable<TeamDto>>(It.IsAny<IEnumerable<Team>>()))
            .Returns(teamDtos);
            
        // Act
        var result = await _teamService.GetTeamsForUserAsync(userId);
        
        // Assert
        Assert.Equal(teamDtos.Count, result.Count());
    }
    
    // Más pruebas...
}
```

### 2. Pruebas de Integración

```csharp
public class TeamControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public TeamControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Reemplazar servicios reales por mocks si es necesario
            });
        });
        
        _client = _factory.CreateClient();
        
        // Autenticar el cliente
        // ...
    }
    
    [Fact]
    public async Task GetTeams_ReturnsTeamsList()
    {
        // Act
        var response = await _client.GetAsync("/api/teams");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var teams = await response.Content.ReadFromJsonAsync<List<TeamDto>>();
        Assert.NotNull(teams);
    }
    
    // Más pruebas...
}
```

## Recomendaciones Adicionales

### 1. Herramientas y Tecnologías

- **ORM**: Entity Framework Core
- **API Documentation**: Swagger con Swashbuckle
- **Dependency Injection**: Built-in en ASP.NET Core
- **Logging**: Serilog o NLog
- **Validación**: FluentValidation
- **Background Jobs**: Hangfire
- **Autenticación**: Identity + JWT
- **Tests**: xUnit o NUnit con Moq

### 2. Consideraciones de Arquitectura

- Implementar arquitectura limpia (Clean Architecture)
- Usar CQRS con MediatR para separar operaciones de lectura y escritura
- Implementar el patrón Repository y Unit of Work
- Usar servicios para la lógica de negocio
- Implementar validación en múltiples niveles (DTO, dominio)

### 3. Seguridad

- Usar ASP.NET Core Identity para gestión de usuarios
- Implementar políticas de autorización basadas en roles y claims
- Validar todos los datos de entrada
- Aplicar HTTPS en todos los endpoints
- Protegerse contra ataques comunes (CSRF, XSS, inyección SQL)

### 4. Rendimiento

- Implementar caché con IMemoryCache o Redis
- Usar AsNoTracking para consultas de solo lectura
- Implementar paginación en todos los endpoints que devuelven listas
- Optimizar consultas EF Core con Include y filtros adecuados
- Usar Dapper para consultas complejas o de alto rendimiento

### 5. DevOps y Despliegue

- Usar GitHub Actions o Azure DevOps para CI/CD
- Implementar pruebas automatizadas en el pipeline
- Utilizar Docker para containerización
- Desplegar en Azure App Service o Kubernetes
- Configurar entornos de desarrollo, pruebas y producción

## Migración Paso a Paso

1. Crear una solución .NET con los proyectos necesarios
2. Definir los modelos de datos y el contexto de EF Core
3. Implementar la autenticación con Identity y JWT
4. Crear servicios, repositorios e interfaces
5. Implementar los controladores API
6. Configurar la validación y el mapeo
7. Escribir pruebas unitarias y de integración
8. Configurar Swagger para documentación
9. Implementar manejo de errores global
10. Configurar el despliegue y CI/CD

## Conclusión

La migración de un proyecto Django a .NET implica cambios significativos en la estructura y las tecnologías utilizadas, pero los conceptos fundamentales de la arquitectura y la lógica de negocio se mantienen. Aprovecha las fortalezas del ecosistema .NET como tipado fuerte, herramientas de desarrollo avanzadas y la integración con servicios de Microsoft.