from django.contrib.auth.models import User
from rest_framework import serializers
from django.contrib.auth.password_validation import validate_password
from .models import Profile, Team, TeamMembership, ExerciseType, Exercise, WorkoutPlan, Activity

class ProfileSerializer(serializers.ModelSerializer):
    class Meta:
        model = Profile
        fields = ('height', 'weight', 'birth_date', 'fitness_goal', 
                 'activity_level', 'bio', 'profile_picture')

class TeamSerializer(serializers.ModelSerializer):
    created_by = serializers.ReadOnlyField(source='created_by.username')
    total_points = serializers.SerializerMethodField()

    class Meta:
        model = Team
        fields = ('id', 'name', 'description', 'created_by', 'is_private', 
                 'created_at', 'updated_at', 'total_points')

    def get_total_points(self, obj):
        return obj.get_total_points()

class TeamMembershipSerializer(serializers.ModelSerializer):
    user = serializers.ReadOnlyField(source='user.username')
    team = serializers.ReadOnlyField(source='team.name')

    class Meta:
        model = TeamMembership
        fields = ('id', 'user', 'team', 'role', 'joined_at')

class ExerciseTypeSerializer(serializers.ModelSerializer):
    class Meta:
        model = ExerciseType
        fields = ('id', 'name', 'description', 'category', 'calories_per_hour',
                 'difficulty_level', 'instructions', 'video_url', 'image')

class WorkoutPlanSerializer(serializers.ModelSerializer):
    user = serializers.ReadOnlyField(source='user.username')
    exercises = serializers.SerializerMethodField()

    class Meta:
        model = WorkoutPlan
        fields = ('id', 'user', 'name', 'description', 'difficulty_level',
                 'created_at', 'updated_at', 'exercises')

    def get_exercises(self, obj):
        exercises = Exercise.objects.filter(workout_plan=obj).order_by('order')
        return ExerciseSerializer(exercises, many=True).data

class ExerciseSerializer(serializers.ModelSerializer):
    exercise_type = ExerciseTypeSerializer(read_only=True)
    exercise_type_id = serializers.PrimaryKeyRelatedField(
        queryset=ExerciseType.objects.all(),
        source='exercise_type',
        write_only=True
    )

    class Meta:
        model = Exercise
        fields = ('id', 'workout_plan', 'exercise_type', 'exercise_type_id',
                 'sets', 'reps', 'duration', 'order', 'notes')
        read_only_fields = ('workout_plan',)

class ActivitySerializer(serializers.ModelSerializer):
    user = serializers.ReadOnlyField(source='user.username')
    exercise_type = ExerciseTypeSerializer(read_only=True)
    exercise_type_id = serializers.PrimaryKeyRelatedField(
        queryset=ExerciseType.objects.all(),
        source='exercise_type',
        write_only=True
    )

    class Meta:
        model = Activity
        fields = ('id', 'user', 'exercise_type', 'exercise_type_id', 'duration',
                 'calories_burned', 'date', 'notes', 'points', 'created_at')

class UserSerializer(serializers.ModelSerializer):
    profile = ProfileSerializer(required=False)
    password = serializers.CharField(write_only=True, required=True, validators=[validate_password])
    password2 = serializers.CharField(write_only=True, required=True)

    class Meta:
        model = User
        fields = ('id', 'username', 'password', 'password2', 'email', 'first_name', 'last_name', 'profile')
        extra_kwargs = {
            'first_name': {'required': True},
            'last_name': {'required': True},
            'email': {'required': True}
        }

    def validate(self, attrs):
        if attrs['password'] != attrs['password2']:
            raise serializers.ValidationError({"password": "Las contraseñas no coinciden"})
        return attrs

    def create(self, validated_data):
        # Remove profile data from the validated_data if it exists
        profile_data = validated_data.pop('profile', None)
        # Remove password2 as it's not needed for user creation
        validated_data.pop('password2', None)
        
        # Create the user with the remaining data
        user = User.objects.create(
            username=validated_data['username'],
            email=validated_data['email'],
            first_name=validated_data['first_name'],
            last_name=validated_data['last_name']
        )
        
        user.set_password(validated_data['password'])
        user.save()

        # Update profile if profile data was provided
        if profile_data:
            Profile.objects.filter(user=user).update(**profile_data)

        return user

class UserUpdateSerializer(serializers.ModelSerializer):
    profile = ProfileSerializer(required=False)

    class Meta:
        model = User
        fields = ('id', 'username', 'email', 'first_name', 'last_name', 'profile')
        read_only_fields = ('id', 'username')  # No permitir cambios en el username

    def update(self, instance, validated_data):
        # Update profile if profile data was provided
        profile_data = validated_data.pop('profile', None)
        if profile_data:
            Profile.objects.filter(user=instance).update(**profile_data)

        # Update user fields
        for attr, value in validated_data.items():
            setattr(instance, attr, value)
        instance.save()

        return instance