from django.test import TestCase
from django.contrib.auth.models import User
from django.utils import timezone
from datetime import timedelta
from .models import (
    Profile, Team, TeamMembership, ExerciseType, 
    WorkoutPlan, Exercise, Activity
)

class ProfileModelTest(TestCase):
    def setUp(self):
        self.user = User.objects.create_user(
            username='testuser',
            password='testpass123'
        )
        self.profile = Profile.objects.get(user=self.user)

    def test_profile_creation(self):
        """Test que el perfil se crea automáticamente con el usuario"""
        self.assertIsNotNone(self.profile)
        self.assertEqual(self.profile.points, 0)
        self.assertEqual(self.profile.level, 1)

    def test_profile_update(self):
        """Test de actualización de perfil"""
        self.profile.height = 175
        self.profile.weight = 70
        self.profile.fitness_goal = 'GAIN_MUSCLE'
        self.profile.save()
        
        updated_profile = Profile.objects.get(user=self.user)
        self.assertEqual(updated_profile.height, 175)
        self.assertEqual(updated_profile.fitness_goal, 'GAIN_MUSCLE')

class TeamModelTest(TestCase):
    def setUp(self):
        self.user = User.objects.create_user(
            username='teamowner',
            password='testpass123'
        )
        self.team = Team.objects.create(
            name='Test Team',
            description='Test Description',
            is_private=False
        )

    def test_team_creation(self):
        """Test de creación de equipo"""
        self.assertEqual(self.team.name, 'Test Team')
        self.assertFalse(self.team.is_private)

    def test_team_membership(self):
        """Test de membresía de equipo"""
        membership = TeamMembership.objects.create(
            team=self.team,
            user=self.user,
            role='ADMIN'
        )
        self.assertTrue(
            TeamMembership.objects.filter(
                team=self.team,
                user=self.user
            ).exists()
        )
        self.assertEqual(membership.role, 'ADMIN')

class ExerciseTypeModelTest(TestCase):
    def setUp(self):
        self.exercise_type = ExerciseType.objects.create(
            name='Push-ups',
            description='Basic push-ups',
            category='STRENGTH',
            difficulty_level='BEGINNER',
            calories_per_hour=400
        )

    def test_exercise_type_creation(self):
        """Test de creación de tipo de ejercicio"""
        self.assertEqual(self.exercise_type.name, 'Push-ups')
        self.assertEqual(self.exercise_type.category, 'STRENGTH')
        self.assertEqual(self.exercise_type.calories_per_hour, 400)

class WorkoutPlanModelTest(TestCase):
    def setUp(self):
        self.user = User.objects.create_user(
            username='planowner',
            password='testpass123'
        )
        self.exercise_type = ExerciseType.objects.create(
            name='Squats',
            category='STRENGTH',
            difficulty_level='BEGINNER'
        )
        self.plan = WorkoutPlan.objects.create(
            name='Beginner Plan',
            description='Test Plan',
            created_by=self.user,
            duration_weeks=4,
            difficulty_level='BEGINNER'
        )

    def test_workout_plan_creation(self):
        """Test de creación de plan de entrenamiento"""
        self.assertEqual(self.plan.name, 'Beginner Plan')
        self.assertEqual(self.plan.duration_weeks, 4)
        self.assertEqual(self.plan.created_by, self.user)

    def test_exercise_addition(self):
        """Test de adición de ejercicios al plan"""
        exercise = Exercise.objects.create(
            workout_plan=self.plan,
            exercise_type=self.exercise_type,
            sets=3,
            reps=12,
            duration_minutes=30,
            day_of_week=1,
            order=1
        )
        self.assertTrue(
            Exercise.objects.filter(
                workout_plan=self.plan
            ).exists()
        )
        self.assertEqual(exercise.sets, 3)
        self.assertEqual(exercise.reps, 12)

class ActivityModelTest(TestCase):
    def setUp(self):
        self.user = User.objects.create_user(
            username='activityuser',
            password='testpass123'
        )
        self.exercise_type = ExerciseType.objects.create(
            name='Running',
            category='CARDIO',
            difficulty_level='BEGINNER',
            calories_per_hour=600
        )
        self.activity = Activity.objects.create(
            user=self.user,
            exercise_type=self.exercise_type,
            duration_minutes=30,
            calories_burned=300,
            date=timezone.now().date(),
            notes='Test activity'
        )

    def test_activity_creation(self):
        """Test de creación de actividad"""
        self.assertEqual(self.activity.duration_minutes, 30)
        self.assertEqual(self.activity.calories_burned, 300)
        self.assertEqual(self.activity.user, self.user)

    def test_activity_date_validation(self):
        """Test de validación de fecha de actividad"""
        future_date = timezone.now().date() + timedelta(days=1)
        with self.assertRaises(Exception):
            Activity.objects.create(
                user=self.user,
                exercise_type=self.exercise_type,
                duration_minutes=30,
                calories_burned=300,
                date=future_date,
                notes='Future activity'
            )
