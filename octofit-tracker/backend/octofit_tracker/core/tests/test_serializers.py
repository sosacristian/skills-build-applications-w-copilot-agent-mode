"""
Tests for the OctoFit Tracker serializers.
"""
from django.test import TestCase
from django.contrib.auth.models import User
from core.models import Profile, Team, TeamMembership, ExerciseType, WorkoutPlan, Exercise, Activity
from core.serializers import (
    UserSerializer, UserUpdateSerializer, TeamSerializer, TeamMembershipSerializer,
    ExerciseTypeSerializer, WorkoutPlanSerializer, ExerciseSerializer, ActivitySerializer
)
from rest_framework.test import APIRequestFactory
from django.utils import timezone

class UserSerializerTest(TestCase):
    """Tests for the UserSerializer."""
    
    def setUp(self):
        """Set up test data."""
        self.user_data = {
            'username': 'newuser',
            'email': 'new@example.com',
            'password': 'newpass123',
            'confirm_password': 'newpass123'
        }
    
    def test_user_creation(self):
        """Test user creation with serializer."""
        serializer = UserSerializer(data=self.user_data)
        self.assertTrue(serializer.is_valid())
        user = serializer.save()
        
        self.assertEqual(user.username, 'newuser')
        self.assertEqual(user.email, 'new@example.com')
        self.assertTrue(user.check_password('newpass123'))
    
    def test_password_validation(self):
        """Test password validation."""
        # Passwords don't match
        invalid_data = self.user_data.copy()
        invalid_data['confirm_password'] = 'wrongpass'
        
        serializer = UserSerializer(data=invalid_data)
        self.assertFalse(serializer.is_valid())
        self.assertIn('confirm_password', serializer.errors)
    
    def test_profile_creation(self):
        """Test that profile is created with user."""
        serializer = UserSerializer(data=self.user_data)
        self.assertTrue(serializer.is_valid())
        user = serializer.save()
        
        # Check that profile exists
        self.assertTrue(hasattr(user, 'profile'))
        self.assertEqual(user.profile.points, 0)
        self.assertEqual(user.profile.level, 1)


class TeamSerializerTest(TestCase):
    """Tests for the TeamSerializer."""
    
    def setUp(self):
        """Set up test data."""
        self.user = User.objects.create_user(
            username='teamowner',
            email='owner@example.com',
            password='testpass123'
        )
        self.team_data = {
            'name': 'New Team',
            'description': 'Team description',
            'is_private': True
        }
    
    def test_team_serialization(self):
        """Test team serialization."""
        team = Team.objects.create(
            name="Test Team",
            description="A test team",
            is_private=False
        )
        TeamMembership.objects.create(
            team=team,
            user=self.user,
            role="ADMIN"
        )
        
        serializer = TeamSerializer(team)
        data = serializer.data
        
        self.assertEqual(data['name'], "Test Team")
        self.assertEqual(data['description'], "A test team")
        self.assertFalse(data['is_private'])
        self.assertEqual(data['members_count'], 1)
    
    def test_team_creation(self):
        """Test team creation with serializer."""
        # Create a request with user
        factory = APIRequestFactory()
        request = factory.post('/api/teams/')
        request.user = self.user
        
        serializer = TeamSerializer(data=self.team_data, context={'request': request})
        self.assertTrue(serializer.is_valid())
        team = serializer.save()
        
        self.assertEqual(team.name, 'New Team')
        self.assertEqual(team.description, 'Team description')
        self.assertTrue(team.is_private)
        
        # Check that the user is added as admin
        self.assertEqual(team.memberships.count(), 1)
        self.assertEqual(team.memberships.first().user, self.user)
        self.assertEqual(team.memberships.first().role, "ADMIN")


class ActivitySerializerTest(TestCase):
    """Tests for the ActivitySerializer."""
    
    def setUp(self):
        """Set up test data."""
        self.user = User.objects.create_user(
            username='activityuser',
            email='activity@example.com',
            password='testpass123'
        )
        self.exercise_type = ExerciseType.objects.create(
            name="Running",
            description="Cardio exercise",
            category="CARDIO",
            difficulty_level="BEGINNER",
            calories_per_hour=500
        )
        self.activity_data = {
            'exercise_type': self.exercise_type.id,
            'duration_minutes': 45,
            'calories_burned': 300,
            'date': timezone.now().date().isoformat(),
            'notes': 'Great workout'
        }
    
    def test_activity_serialization(self):
        """Test activity serialization."""
        activity = Activity.objects.create(
            user=self.user,
            exercise_type=self.exercise_type,
            duration_minutes=30,
            calories_burned=200,
            date=timezone.now().date(),
            notes="Test activity"
        )
        
        serializer = ActivitySerializer(activity)
        data = serializer.data
        
        self.assertEqual(data['duration_minutes'], 30)
        self.assertEqual(data['calories_burned'], 200)
        self.assertEqual(data['notes'], "Test activity")
        self.assertEqual(data['exercise_type']['name'], "Running")
    
    def test_activity_creation(self):
        """Test activity creation with serializer."""
        # Create a request with user
        factory = APIRequestFactory()
        request = factory.post('/api/activities/')
        request.user = self.user
        
        serializer = ActivitySerializer(data=self.activity_data, context={'request': request})
        self.assertTrue(serializer.is_valid())
        activity = serializer.save()
        
        self.assertEqual(activity.user, self.user)
        self.assertEqual(activity.exercise_type, self.exercise_type)
        self.assertEqual(activity.duration_minutes, 45)
        self.assertEqual(activity.calories_burned, 300)
        self.assertEqual(activity.notes, 'Great workout')