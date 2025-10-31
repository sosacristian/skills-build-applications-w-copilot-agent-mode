from django.contrib import admin
from django.contrib.auth.admin import UserAdmin as BaseUserAdmin
from django.contrib.auth.models import User
from .models import Profile, Team, TeamMembership, Exercise, ExerciseType, WorkoutPlan, Activity

class ProfileInline(admin.StackedInline):
    model = Profile
    can_delete = False
    verbose_name_plural = 'Profile'

class UserAdmin(BaseUserAdmin):
    inlines = (ProfileInline,)

# Re-registrar UserAdmin
admin.site.unregister(User)
admin.site.register(User, UserAdmin)

@admin.register(Team)
class TeamAdmin(admin.ModelAdmin):
    list_display = ('name', 'created_by', 'is_private', 'created_at')
    search_fields = ('name', 'created_by__username')
    list_filter = ('is_private', 'created_at')

@admin.register(TeamMembership)
class TeamMembershipAdmin(admin.ModelAdmin):
    list_display = ('user', 'team', 'role', 'joined_at')
    list_filter = ('role', 'joined_at')
    search_fields = ('user__username', 'team__name')

@admin.register(ExerciseType)
class ExerciseTypeAdmin(admin.ModelAdmin):
    list_display = ('name', 'category', 'difficulty_level', 'calories_per_hour')
    list_filter = ('category', 'difficulty_level')
    search_fields = ('name', 'description')

@admin.register(WorkoutPlan)
class WorkoutPlanAdmin(admin.ModelAdmin):
    list_display = ('name', 'user', 'difficulty_level', 'created_at')
    list_filter = ('difficulty_level', 'created_at')
    search_fields = ('name', 'user__username')

@admin.register(Exercise)
class ExerciseAdmin(admin.ModelAdmin):
    list_display = ('exercise_type', 'workout_plan', 'sets', 'reps', 'duration', 'order')
    list_filter = ('workout_plan', 'exercise_type__category')
    search_fields = ('exercise_type__name', 'workout_plan__name')
    ordering = ('workout_plan', 'order')

@admin.register(Activity)
class ActivityAdmin(admin.ModelAdmin):
    list_display = ('user', 'exercise_type', 'date', 'duration', 'calories_burned', 'points')
    list_filter = ('date', 'exercise_type__category')
    search_fields = ('user__username', 'exercise_type__name')
    date_hierarchy = 'date'
