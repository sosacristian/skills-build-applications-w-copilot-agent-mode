from django.db import models
from django.contrib.auth.models import User
from django.db.models.signals import post_save
from django.dispatch import receiver
from django.utils import timezone

class Profile(models.Model):
    FITNESS_GOALS = [
        ('weight_loss', 'Pérdida de peso'),
        ('muscle_gain', 'Ganancia muscular'),
        ('endurance', 'Resistencia'),
        ('flexibility', 'Flexibilidad'),
        ('general_fitness', 'Fitness general'),
    ]

    ACTIVITY_LEVELS = [
        ('sedentary', 'Sedentario'),
        ('light', 'Actividad ligera'),
        ('moderate', 'Actividad moderada'),
        ('very_active', 'Muy activo'),
        ('extra_active', 'Extra activo'),
    ]

    user = models.OneToOneField(User, on_delete=models.CASCADE)
    height = models.FloatField(help_text="Altura en centímetros", null=True)
    weight = models.FloatField(help_text="Peso en kilogramos", null=True)
    birth_date = models.DateField(null=True)
    fitness_goal = models.CharField(max_length=50, choices=FITNESS_GOALS, null=True)
    activity_level = models.CharField(max_length=50, choices=ACTIVITY_LEVELS, null=True)
    bio = models.TextField(max_length=500, blank=True)
    profile_picture = models.ImageField(upload_to='profile_pics', blank=True)
    created_at = models.DateTimeField(default=timezone.now)
    updated_at = models.DateTimeField(auto_now=True)

    def __str__(self):
        return f'Perfil de {self.user.username}'
        
    def calculate_daily_calories(self):
        """Calcula las calorías diarias recomendadas basadas en el nivel de actividad y objetivo"""
        # Implementar fórmula de calorías
        pass

@receiver(post_save, sender=User)
def create_user_profile(sender, instance, created, **kwargs):
    if created:
        Profile.objects.create(user=instance)

@receiver(post_save, sender=User)
def save_user_profile(sender, instance, **kwargs):
    instance.profile.save()

class Team(models.Model):
    name = models.CharField(max_length=100)
    description = models.TextField()
    created_by = models.ForeignKey(User, on_delete=models.PROTECT, related_name='created_teams', null=True)
    is_private = models.BooleanField(default=False)
    created_at = models.DateTimeField(default=timezone.now)
    updated_at = models.DateTimeField(auto_now=True)

    def __str__(self):
        return self.name

    def get_total_points(self):
        """Calcula el total de puntos del equipo"""
        return sum(member.activity_set.aggregate(models.Sum('points'))['points__sum'] or 0 
                  for member in self.members.all())

class TeamMembership(models.Model):
    TEAM_ROLES = [
        ('member', 'Miembro'),
        ('admin', 'Administrador'),
        ('coach', 'Entrenador'),
    ]

    user = models.ForeignKey(User, on_delete=models.CASCADE)
    team = models.ForeignKey(Team, on_delete=models.CASCADE)
    role = models.CharField(max_length=50, choices=TEAM_ROLES)
    joined_at = models.DateTimeField(default=timezone.now)

    class Meta:
        unique_together = ('user', 'team')

    def __str__(self):
        return f'{self.user.username} - {self.team.name} ({self.role})'

class ExerciseType(models.Model):
    EXERCISE_CATEGORIES = [
        ('cardio', 'Cardiovascular'),
        ('strength', 'Fuerza'),
        ('flexibility', 'Flexibilidad'),
        ('balance', 'Equilibrio'),
        ('hiit', 'HIIT'),
    ]

    DIFFICULTY_LEVELS = [
        ('beginner', 'Principiante'),
        ('intermediate', 'Intermedio'),
        ('advanced', 'Avanzado'),
    ]

    name = models.CharField(max_length=100)
    description = models.TextField()
    category = models.CharField(max_length=50, choices=EXERCISE_CATEGORIES)
    calories_per_hour = models.IntegerField()
    difficulty_level = models.CharField(max_length=50, choices=DIFFICULTY_LEVELS)
    instructions = models.TextField()
    video_url = models.URLField(blank=True)
    image = models.ImageField(upload_to='exercise_images', blank=True)

    def __str__(self):
        return self.name

    def get_calories_per_minute(self):
        """Calcula las calorías quemadas por minuto"""
        return self.calories_per_hour / 60

class WorkoutPlan(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)
    name = models.CharField(max_length=100)
    description = models.TextField()
    difficulty_level = models.CharField(
        max_length=50, 
        choices=ExerciseType.DIFFICULTY_LEVELS
    )
    created_at = models.DateTimeField(default=timezone.now)
    updated_at = models.DateTimeField(auto_now=True)

    def __str__(self):
        return f'{self.name} - {self.user.username}'

class Exercise(models.Model):
    workout_plan = models.ForeignKey(WorkoutPlan, on_delete=models.CASCADE, null=True)
    exercise_type = models.ForeignKey(ExerciseType, on_delete=models.PROTECT, null=True)
    sets = models.IntegerField(null=True)
    reps = models.IntegerField(null=True)
    duration = models.IntegerField(help_text="Duración en minutos", null=True, blank=True)
    order = models.IntegerField(null=True)
    notes = models.TextField(blank=True)

    def __str__(self):
        return f'{self.exercise_type.name} - {self.workout_plan.name}'

    class Meta:
        ordering = ['order']

class Activity(models.Model):
    user = models.ForeignKey(User, on_delete=models.CASCADE)
    exercise_type = models.ForeignKey(ExerciseType, on_delete=models.PROTECT, null=True)
    duration = models.IntegerField(help_text="Duración en minutos", null=True)
    calories_burned = models.IntegerField(null=True)
    date = models.DateField(null=True)
    notes = models.TextField(blank=True)
    points = models.IntegerField(default=0)
    created_at = models.DateTimeField(default=timezone.now)

    def save(self, *args, **kwargs):
        if not self.calories_burned:
            self.calories_burned = int(self.exercise_type.calories_per_hour * (self.duration / 60))
        if not self.points:
            # Calcular puntos basados en duración, dificultad y calorías
            difficulty_multiplier = {
                'beginner': 1,
                'intermediate': 1.5,
                'advanced': 2
            }
            self.points = int(
                self.calories_burned * 
                difficulty_multiplier[self.exercise_type.difficulty_level]
            )
        super().save(*args, **kwargs)

    def __str__(self):
        return f'{self.user.username} - {self.exercise_type.name} ({self.date})'

    class Meta:
        verbose_name_plural = "Activities"
        ordering = ['-date', '-created_at']
