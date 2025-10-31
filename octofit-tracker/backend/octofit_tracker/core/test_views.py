from django.test import TestCase
from django.urls import reverse
from rest_framework.test import APIClient
from rest_framework import status
from django.contrib.auth.models import User
from .models import (
    Profile, Team, TeamMembership, ExerciseType, 
    WorkoutPlan, Exercise, Activity
)
from rest_framework_simplejwt.tokens import RefreshToken

class AuthenticationTest(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.register_url = reverse('register')
        self.token_url = reverse('token_obtain_pair')
        self.user_data = {
            'username': 'testuser',
            'email': 'test@example.com',
            'password': 'testpass123',
            'confirm_password': 'testpass123'
        }

    def test_user_registration(self):
        """Test de registro de usuario"""
        response = self.client.post(self.register_url, self.user_data)
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        self.assertTrue(User.objects.filter(username='testuser').exists())

    def test_user_login(self):
        """Test de login de usuario"""
        # Crear usuario primero
        User.objects.create_user(
            username='testuser',
            password='testpass123'
        )
        
        # Intentar login
        response = self.client.post(self.token_url, {
            'username': 'testuser',
            'password': 'testpass123'
        })
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        self.assertIn('access', response.data)
        self.assertIn('refresh', response.data)

class TeamViewSetTest(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.user = User.objects.create_user(
            username='teamuser',
            password='testpass123'
        )
        self.token = str(RefreshToken.for_user(self.user).access_token)
        self.client.credentials(HTTP_AUTHORIZATION=f'Bearer {self.token}')
        
        self.team_data = {
            'name': 'Test Team',
            'description': 'Test Description',
            'is_private': False
        }

    def test_create_team(self):
        """Test de creación de equipo"""
        response = self.client.post(
            reverse('team-list'),
            self.team_data
        )
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        self.assertEqual(Team.objects.count(), 1)
        self.assertEqual(Team.objects.first().name, 'Test Team')

    def test_join_team(self):
        """Test de unirse a un equipo"""
        team = Team.objects.create(**self.team_data)
        response = self.client.post(
            reverse('membership-list'),
            {'team': team.id, 'role': 'MEMBER'}
        )
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        self.assertTrue(
            TeamMembership.objects.filter(
                user=self.user,
                team=team
            ).exists()
        )

class WorkoutPlanViewSetTest(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.user = User.objects.create_user(
            username='planuser',
            password='testpass123'
        )
        self.token = str(RefreshToken.for_user(self.user).access_token)
        self.client.credentials(HTTP_AUTHORIZATION=f'Bearer {self.token}')
        
        self.plan_data = {
            'name': 'Test Plan',
            'description': 'Test Description',
            'duration_weeks': 4,
            'difficulty_level': 'BEGINNER'
        }

    def test_create_plan(self):
        """Test de creación de plan"""
        response = self.client.post(
            reverse('workout-plan-list'),
            self.plan_data
        )
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        self.assertEqual(WorkoutPlan.objects.count(), 1)
        self.assertEqual(
            WorkoutPlan.objects.first().created_by,
            self.user
        )

class ActivityViewSetTest(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.user = User.objects.create_user(
            username='activityuser',
            password='testpass123'
        )
        self.token = str(RefreshToken.for_user(self.user).access_token)
        self.client.credentials(HTTP_AUTHORIZATION=f'Bearer {self.token}')
        
        self.exercise_type = ExerciseType.objects.create(
            name='Running',
            category='CARDIO',
            difficulty_level='BEGINNER',
            calories_per_hour=600
        )
        
        self.activity_data = {
            'exercise_type': self.exercise_type.id,
            'duration_minutes': 30,
            'calories_burned': 300,
            'date': '2025-10-21',
            'notes': 'Test activity'
        }

    def test_create_activity(self):
        """Test de creación de actividad"""
        response = self.client.post(
            reverse('activity-list'),
            self.activity_data
        )
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        self.assertEqual(Activity.objects.count(), 1)
        self.assertEqual(
            Activity.objects.first().user,
            self.user
        )

    def test_list_own_activities(self):
        """Test de listar actividades propias"""
        # Crear actividad de otro usuario
        other_user = User.objects.create_user(
            username='otheruser',
            password='testpass123'
        )
        Activity.objects.create(
            user=other_user,
            exercise_type=self.exercise_type,
            duration_minutes=30,
            calories_burned=300,
            date='2025-10-21'
        )
        
        # Crear actividad propia
        Activity.objects.create(
            user=self.user,
            exercise_type=self.exercise_type,
            duration_minutes=45,
            calories_burned=450,
            date='2025-10-21'
        )
        
        response = self.client.get(reverse('activity-list'))
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        self.assertEqual(len(response.data['results']), 1)  # Solo ve su actividad