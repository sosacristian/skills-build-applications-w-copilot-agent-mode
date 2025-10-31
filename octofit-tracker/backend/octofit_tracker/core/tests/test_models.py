"""
Tests for the OctoFit Tracker models.
"""
from django.test import TestCase
from django.contrib.auth.models import User
from django.utils import timezone
from datetime import timedelta
from core.models import Profile, Team, TeamMembership, ExerciseType, WorkoutPlan, Exercise, Activity

class ProfileModelTest(TestCase):
    """Tests for the Profile model."""
    
    def setUp(self):
        """Set up test data."""
        self.user = User.objects.create_user(
            username='testuser',
            email='test@example.com',
            password='testpass123'
        )
        self.profile = Profile.objects.get(user=self.user)  # Profile should be created via signal
    
    def test_profile_creation(self):
        """Test that profile is created automatically for a new user."""
        self.assertEqual(self.profile.user, self.user)
        self.assertEqual(self.profile.points, 0)
        self.assertEqual(self.profile.level, 1)
    
    def test_profile_str_method(self):
        """Test Profile string representation."""
        self.assertEqual(str(self.profile), f"Profile for {self.user.username}")
    
    def test_profile_update(self):
        """Test updating profile attributes."""
        self.profile.height = 180
        self.profile.weight = 75
        self.profile.fitness_goal = "GAIN_MUSCLE"
        self.profile.activity_level = "MODERATE"
        self.profile.save()
        
        updated_profile = Profile.objects.get(user=self.user)
        self.assertEqual(updated_profile.height, 180)
        self.assertEqual(updated_profile.weight, 75)
        self.assertEqual(updated_profile.fitness_goal, "GAIN_MUSCLE")
        self.assertEqual(updated_profile.activity_level, "MODERATE")


class TeamModelTest(TestCase):
    """Tests for the Team model."""
    
    def setUp(self):
        """Set up test data."""
        self.user = User.objects.create_user(
            username='teamowner',
            email='owner@example.com',
            password='testpass123'
        )
        self.team = Team.objects.create(
            name="Test Team",
            description="A test team",
            is_private=False
        )
        self.membership = TeamMembership.objects.create(
            team=self.team,
            user=self.user,
            role="ADMIN"
        )
    
    def test_team_creation(self):
        """Test creating a team."""
        self.assertEqual(self.team.name, "Test Team")
        self.assertEqual(self.team.description, "A test team")
        self.assertFalse(self.team.is_private)
    
    def test_team_str_method(self):
        """Test Team string representation."""
        self.assertEqual(str(self.team), "Test Team")
    
    def test_team_membership(self):
        """Test team membership relationship."""
        self.assertEqual(self.team.memberships.count(), 1)
        self.assertEqual(self.team.memberships.first().user, self.user)
        self.assertEqual(self.team.memberships.first().role, "ADMIN")
    
    def test_team_members_method(self):
        """Test getting team members."""
        members = self.team.members.all()
        self.assertEqual(members.count(), 1)
        self.assertEqual(members.first(), self.user)


class ExerciseTypeModelTest(TestCase):
    """Tests for the ExerciseType model."""
    
    def setUp(self):
        """Set up test data."""
        self.exercise_type = ExerciseType.objects.create(
            name="Running",
            description="Cardio exercise",
            category="CARDIO",
            difficulty_level="BEGINNER",
            calories_per_hour=500
        )
    
    def test_exercise_type_creation(self):
        """Test creating an exercise type."""
        self.assertEqual(self.exercise_type.name, "Running")
        self.assertEqual(self.exercise_type.category, "CARDIO")
        self.assertEqual(self.exercise_type.difficulty_level, "BEGINNER")
        self.assertEqual(self.exercise_type.calories_per_hour, 500)
    
    def test_exercise_type_str_method(self):
        """Test ExerciseType string representation."""
        self.assertEqual(str(self.exercise_type), "Running")


class WorkoutPlanModelTest(TestCase):
    """Tests for the WorkoutPlan model."""
    
    def setUp(self):
        """Set up test data."""
        self.user = User.objects.create_user(
            username='planuser',
            email='plan@example.com',
            password='testpass123'
        )
        self.plan = WorkoutPlan.objects.create(
            name="Beginner Plan",
            description="A plan for beginners",
            created_by=self.user,
            duration_weeks=4,
            difficulty_level="BEGINNER"
        )
        self.exercise_type = ExerciseType.objects.create(
            name="Push-ups",
            description="Strength exercise",
            category="STRENGTH",
            difficulty_level="BEGINNER",
            calories_per_hour=300
        )
        self.exercise = Exercise.objects.create(
            workout_plan=self.plan,
            exercise_type=self.exercise_type,
            sets=3,
            reps=10,
            duration_minutes=15,
            day_of_week=1,
            order=1
        )
    
    def test_workout_plan_creation(self):
        """Test creating a workout plan."""
        self.assertEqual(self.plan.name, "Beginner Plan")
        self.assertEqual(self.plan.duration_weeks, 4)
        self.assertEqual(self.plan.created_by, self.user)
    
    def test_workout_plan_str_method(self):
        """Test WorkoutPlan string representation."""
        self.assertEqual(str(self.plan), "Beginner Plan")
    
    def test_workout_plan_exercises(self):
        """Test workout plan exercises relationship."""
        self.assertEqual(self.plan.exercises.count(), 1)
        self.assertEqual(self.plan.exercises.first().exercise_type, self.exercise_type)


class ActivityModelTest(TestCase):
    """Tests for the Activity model."""
    
    def setUp(self):
        """Set up test data."""
        self.user = User.objects.create_user(
            username='activityuser',
            email='activity@example.com',
            password='testpass123'
        )
        self.exercise_type = ExerciseType.objects.create(
            name="Cycling",
            description="Cardio exercise",
            category="CARDIO",
            difficulty_level="INTERMEDIATE",
            calories_per_hour=400
        )
        self.activity = Activity.objects.create(
            user=self.user,
            exercise_type=self.exercise_type,
            duration_minutes=30,
            calories_burned=200,
            date=timezone.now().date(),
            notes="Good session"
        )
    
    def test_activity_creation(self):
        """Test creating an activity."""
        self.assertEqual(self.activity.user, self.user)
        self.assertEqual(self.activity.exercise_type, self.exercise_type)
        self.assertEqual(self.activity.duration_minutes, 30)
        self.assertEqual(self.activity.calories_burned, 200)
    
    def test_activity_str_method(self):
        """Test Activity string representation."""
        self.assertEqual(str(self.activity), f"Cycling by {self.user.username} on {self.activity.date}")
    
    def test_activity_date_filter(self):
        """Test filtering activities by date."""
        yesterday = timezone.now().date() - timedelta(days=1)
        week_ago = timezone.now().date() - timedelta(days=7)
        
        Activity.objects.create(
            user=self.user,
            exercise_type=self.exercise_type,
            duration_minutes=45,
            calories_burned=300,
            date=yesterday,
            notes="Yesterday's session"
        )
        
        recent_activities = Activity.objects.filter(
            date__gte=week_ago
        )
        self.assertEqual(recent_activities.count(), 2)
        
        today_activities = Activity.objects.filter(
            date=timezone.now().date()
        )
        self.assertEqual(today_activities.count(), 1)