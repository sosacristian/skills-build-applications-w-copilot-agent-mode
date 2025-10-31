"""
Tests for the OctoFit Tracker views and authentication.
"""
from django.test import TestCase
from django.urls import reverse
from django.contrib.auth.models import User
from rest_framework.test import APIClient
from rest_framework import status
from core.models import Profile, Team, TeamMembership, ExerciseType, WorkoutPlan, Exercise, Activity
from django.utils import timezone
import json

class AuthenticationTest(TestCase):
    """Tests for authentication endpoints."""
    
    def setUp(self):
        """Set up test data."""
        self.client = APIClient()
        self.register_url = reverse('register')
        self.token_url = '/api/token/'  # Direct URL since it's from external package
        self.refresh_url = '/api/token/refresh/'
        
        # Create a test user
        self.user = User.objects.create_user(
            username='existinguser',
            email='existing@example.com',
            password='testpass123'
        )
    
    def test_user_registration(self):
        """Test user registration."""
        data = {
            'username': 'newuser',
            'email': 'new@example.com',
            'password': 'newpass123',
            'confirm_password': 'newpass123'
        }
        response = self.client.post(self.register_url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        self.assertEqual(User.objects.count(), 2)
        self.assertTrue(User.objects.filter(username='newuser').exists())
    
    def test_duplicate_registration(self):
        """Test registration with existing username."""
        data = {
            'username': 'existinguser',
            'email': 'new@example.com',
            'password': 'newpass123',
            'confirm_password': 'newpass123'
        }
        response = self.client.post(self.register_url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_400_BAD_REQUEST)
    
    def test_token_obtain(self):
        """Test obtaining JWT token."""
        data = {
            'username': 'existinguser',
            'password': 'testpass123'
        }
        response = self.client.post(self.token_url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        self.assertIn('access', response.data)
        self.assertIn('refresh', response.data)
    
    def test_token_refresh(self):
        """Test refreshing JWT token."""
        # First get a token
        token_data = {
            'username': 'existinguser',
            'password': 'testpass123'
        }
        token_response = self.client.post(self.token_url, token_data, format='json')
        
        # Now refresh it
        refresh_data = {
            'refresh': token_response.data['refresh']
        }
        refresh_response = self.client.post(self.refresh_url, refresh_data, format='json')
        self.assertEqual(refresh_response.status_code, status.HTTP_200_OK)
        self.assertIn('access', refresh_response.data)


class TeamViewSetTest(TestCase):
    """Tests for the TeamViewSet."""
    
    def setUp(self):
        """Set up test data."""
        self.client = APIClient()
        self.teams_url = reverse('team-list')
        
        # Create users
        self.user1 = User.objects.create_user(
            username='teamuser1',
            email='user1@example.com',
            password='testpass123'
        )
        self.user2 = User.objects.create_user(
            username='teamuser2',
            email='user2@example.com',
            password='testpass123'
        )
        
        # Create teams
        self.public_team = Team.objects.create(
            name="Public Team",
            description="A public team",
            is_private=False
        )
        self.private_team = Team.objects.create(
            name="Private Team",
            description="A private team",
            is_private=True
        )
        
        # Create memberships
        TeamMembership.objects.create(
            team=self.public_team,
            user=self.user1,
            role="ADMIN"
        )
        TeamMembership.objects.create(
            team=self.private_team,
            user=self.user1,
            role="ADMIN"
        )
        
        # Get token for user1
        token_response = self.client.post('/api/token/', {
            'username': 'teamuser1',
            'password': 'testpass123'
        }, format='json')
        self.token = token_response.data['access']
        self.client.credentials(HTTP_AUTHORIZATION=f'Bearer {self.token}')
    
    def test_list_teams_authenticated(self):
        """Test listing teams as authenticated user."""
        response = self.client.get(self.teams_url)
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        
        # User should see both public and private teams they're a member of
        self.assertEqual(len(response.data['results']), 2)
        team_names = [team['name'] for team in response.data['results']]
        self.assertIn('Public Team', team_names)
        self.assertIn('Private Team', team_names)
    
    def test_list_teams_other_user(self):
        """Test listing teams as another user."""
        # Get token for user2
        token_response = self.client.post('/api/token/', {
            'username': 'teamuser2',
            'password': 'testpass123'
        }, format='json')
        token = token_response.data['access']
        self.client.credentials(HTTP_AUTHORIZATION=f'Bearer {token}')
        
        response = self.client.get(self.teams_url)
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        
        # User should only see public teams
        self.assertEqual(len(response.data['results']), 1)
        self.assertEqual(response.data['results'][0]['name'], 'Public Team')
    
    def test_create_team(self):
        """Test creating a team."""
        data = {
            'name': 'New Team',
            'description': 'A new team',
            'is_private': False
        }
        response = self.client.post(self.teams_url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        
        # Check the team was created
        self.assertEqual(Team.objects.count(), 3)
        new_team = Team.objects.get(name='New Team')
        
        # Check user was added as admin
        self.assertEqual(new_team.memberships.count(), 1)
        self.assertEqual(new_team.memberships.first().user, self.user1)
        self.assertEqual(new_team.memberships.first().role, 'ADMIN')
    
    def test_update_team(self):
        """Test updating a team."""
        data = {
            'name': 'Updated Team',
            'description': 'Updated description',
            'is_private': True
        }
        url = reverse('team-detail', args=[self.public_team.id])
        response = self.client.put(url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        
        # Check team was updated
        self.public_team.refresh_from_db()
        self.assertEqual(self.public_team.name, 'Updated Team')
        self.assertEqual(self.public_team.description, 'Updated description')
        self.assertTrue(self.public_team.is_private)
    
    def test_delete_team(self):
        """Test deleting a team."""
        url = reverse('team-detail', args=[self.public_team.id])
        response = self.client.delete(url)
        self.assertEqual(response.status_code, status.HTTP_204_NO_CONTENT)
        
        # Check team was deleted
        self.assertEqual(Team.objects.count(), 1)
        self.assertFalse(Team.objects.filter(id=self.public_team.id).exists())


class ActivityViewSetTest(TestCase):
    """Tests for the ActivityViewSet."""
    
    def setUp(self):
        """Set up test data."""
        self.client = APIClient()
        self.activities_url = reverse('activity-list')
        
        # Create users
        self.user1 = User.objects.create_user(
            username='actuser1',
            email='act1@example.com',
            password='testpass123'
        )
        self.user2 = User.objects.create_user(
            username='actuser2',
            email='act2@example.com',
            password='testpass123'
        )
        
        # Create exercise type
        self.exercise_type = ExerciseType.objects.create(
            name="Running",
            description="Cardio exercise",
            category="CARDIO",
            difficulty_level="BEGINNER",
            calories_per_hour=500
        )
        
        # Create activities
        self.activity1 = Activity.objects.create(
            user=self.user1,
            exercise_type=self.exercise_type,
            duration_minutes=30,
            calories_burned=200,
            date=timezone.now().date(),
            notes="User 1 activity"
        )
        self.activity2 = Activity.objects.create(
            user=self.user2,
            exercise_type=self.exercise_type,
            duration_minutes=45,
            calories_burned=300,
            date=timezone.now().date(),
            notes="User 2 activity"
        )
        
        # Get token for user1
        token_response = self.client.post('/api/token/', {
            'username': 'actuser1',
            'password': 'testpass123'
        }, format='json')
        self.token = token_response.data['access']
        self.client.credentials(HTTP_AUTHORIZATION=f'Bearer {self.token}')
    
    def test_list_activities(self):
        """Test listing activities."""
        response = self.client.get(self.activities_url)
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        
        # User should only see their own activities
        self.assertEqual(len(response.data['results']), 1)
        self.assertEqual(response.data['results'][0]['notes'], "User 1 activity")
    
    def test_create_activity(self):
        """Test creating an activity."""
        data = {
            'exercise_type': self.exercise_type.id,
            'duration_minutes': 60,
            'calories_burned': 400,
            'date': timezone.now().date().isoformat(),
            'notes': 'New activity'
        }
        response = self.client.post(self.activities_url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_201_CREATED)
        
        # Check activity was created
        self.assertEqual(Activity.objects.filter(user=self.user1).count(), 2)
        new_activity = Activity.objects.get(notes='New activity')
        self.assertEqual(new_activity.user, self.user1)
        self.assertEqual(new_activity.duration_minutes, 60)
    
    def test_update_activity(self):
        """Test updating an activity."""
        data = {
            'exercise_type': self.exercise_type.id,
            'duration_minutes': 40,
            'calories_burned': 250,
            'date': timezone.now().date().isoformat(),
            'notes': 'Updated notes'
        }
        url = reverse('activity-detail', args=[self.activity1.id])
        response = self.client.put(url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        
        # Check activity was updated
        self.activity1.refresh_from_db()
        self.assertEqual(self.activity1.duration_minutes, 40)
        self.assertEqual(self.activity1.calories_burned, 250)
        self.assertEqual(self.activity1.notes, 'Updated notes')
    
    def test_delete_activity(self):
        """Test deleting an activity."""
        url = reverse('activity-detail', args=[self.activity1.id])
        response = self.client.delete(url)
        self.assertEqual(response.status_code, status.HTTP_204_NO_CONTENT)
        
        # Check activity was deleted
        self.assertEqual(Activity.objects.filter(user=self.user1).count(), 0)
    
    def test_no_access_to_others_activity(self):
        """Test that users can't access others' activities."""
        url = reverse('activity-detail', args=[self.activity2.id])
        response = self.client.get(url)
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)
        
        # Try to update
        data = {
            'exercise_type': self.exercise_type.id,
            'duration_minutes': 50,
            'calories_burned': 350,
            'date': timezone.now().date().isoformat(),
            'notes': 'Tried to update'
        }
        response = self.client.put(url, data, format='json')
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)
        
        # Try to delete
        response = self.client.delete(url)
        self.assertEqual(response.status_code, status.HTTP_404_NOT_FOUND)
        
        # Check activity was not modified
        self.activity2.refresh_from_db()
        self.assertEqual(self.activity2.notes, "User 2 activity")