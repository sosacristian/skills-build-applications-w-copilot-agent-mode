from rest_framework import generics, status, permissions, viewsets, filters
from rest_framework.response import Response
from rest_framework_simplejwt.views import TokenObtainPairView
from django.contrib.auth.models import User
from django.db.models import Sum
from django.utils import timezone
from datetime import timedelta
from .serializers import (
    UserSerializer, UserUpdateSerializer, TeamSerializer, TeamMembershipSerializer,
    ExerciseTypeSerializer, WorkoutPlanSerializer, ExerciseSerializer, ActivitySerializer
)
from .models import Profile, Team, TeamMembership, ExerciseType, WorkoutPlan, Exercise, Activity
from rest_framework.decorators import api_view, permission_classes, action
from rest_framework.permissions import IsAuthenticated
from django_filters import rest_framework as django_filters

class RegisterView(generics.CreateAPIView):
    queryset = User.objects.all()
    permission_classes = (permissions.AllowAny,)
    serializer_class = UserSerializer

class UserDetailView(generics.RetrieveUpdateAPIView):
    queryset = User.objects.all()
    permission_classes = (IsAuthenticated,)
    serializer_class = UserUpdateSerializer

    def get_object(self):
        return self.request.user

    def update(self, request, *args, **kwargs):
        partial = kwargs.pop('partial', False)
        instance = self.get_object()
        serializer = self.get_serializer(instance, data=request.data, partial=partial)
        serializer.is_valid(raise_exception=True)
        self.perform_update(serializer)
        return Response(serializer.data)

class TeamViewSet(viewsets.ModelViewSet):
    queryset = Team.objects.all()
    serializer_class = TeamSerializer
    permission_classes = [IsAuthenticated]
    filter_backends = [filters.SearchFilter]
    search_fields = ['name', 'description']

    def get_queryset(self):
        # Mostrar equipos públicos y los privados donde el usuario es miembro
        user_teams = TeamMembership.objects.filter(user=self.request.user).values_list('team', flat=True)
        return Team.objects.filter(
            models.Q(is_private=False) | models.Q(id__in=user_teams)
        )

    def perform_create(self, serializer):
        team = serializer.save(created_by=self.request.user)
        # Crear membresía automática para el creador como admin
        TeamMembership.objects.create(user=self.request.user, team=team, role='admin')

    @action(detail=True, methods=['post'])
    def join(self, request, pk=None):
        team = self.get_object()
        if team.is_private:
            return Response(
                {"detail": "No puedes unirte a un equipo privado directamente."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        TeamMembership.objects.get_or_create(
            user=request.user,
            team=team,
            defaults={'role': 'member'}
        )
        return Response({"detail": "Te has unido al equipo exitosamente."})

class TeamMembershipViewSet(viewsets.ModelViewSet):
    serializer_class = TeamMembershipSerializer
    permission_classes = [IsAuthenticated]

    def get_queryset(self):
        if self.action == 'list':
            # Para listar, mostrar solo las membresías del usuario actual
            return TeamMembership.objects.filter(user=self.request.user)
        # Para otras acciones, permitir ver todas las membresías de los equipos donde es admin
        admin_teams = TeamMembership.objects.filter(
            user=self.request.user,
            role='admin'
        ).values_list('team', flat=True)
        return TeamMembership.objects.filter(team__in=admin_teams)

class ExerciseTypeViewSet(viewsets.ModelViewSet):
    queryset = ExerciseType.objects.all()
    serializer_class = ExerciseTypeSerializer
    permission_classes = [IsAuthenticated]
    filter_backends = [filters.SearchFilter]
    search_fields = ['name', 'description', 'category']

class WorkoutPlanViewSet(viewsets.ModelViewSet):
    serializer_class = WorkoutPlanSerializer
    permission_classes = [IsAuthenticated]
    filter_backends = [filters.SearchFilter]
    search_fields = ['name', 'description']

    def get_queryset(self):
        return WorkoutPlan.objects.filter(user=self.request.user)

    def perform_create(self, serializer):
        serializer.save(user=self.request.user)

class ExerciseViewSet(viewsets.ModelViewSet):
    serializer_class = ExerciseSerializer
    permission_classes = [IsAuthenticated]

    def get_queryset(self):
        return Exercise.objects.filter(workout_plan__user=self.request.user)

    def perform_create(self, serializer):
        workout_plan = WorkoutPlan.objects.get(pk=self.kwargs['workout_plan_pk'])
        if workout_plan.user != self.request.user:
            raise permissions.PermissionDenied()
        serializer.save(workout_plan=workout_plan)

class ActivityFilter(django_filters.FilterSet):
    start_date = django_filters.DateFilter(field_name='date', lookup_expr='gte')
    end_date = django_filters.DateFilter(field_name='date', lookup_expr='lte')
    category = django_filters.CharFilter(field_name='exercise_type__category')

    class Meta:
        model = Activity
        fields = ['exercise_type', 'date', 'start_date', 'end_date', 'category']

class ActivityViewSet(viewsets.ModelViewSet):
    serializer_class = ActivitySerializer
    permission_classes = [IsAuthenticated]
    filter_backends = [django_filters.DjangoFilterBackend]
    filterset_class = ActivityFilter

    def get_queryset(self):
        return Activity.objects.filter(user=self.request.user)

    def perform_create(self, serializer):
        serializer.save(user=self.request.user)

    @action(detail=False)
    def statistics(self, request):
        # Obtener estadísticas del último mes
        last_month = timezone.now() - timedelta(days=30)
        activities = Activity.objects.filter(
            user=request.user,
            date__gte=last_month
        )

        total_calories = activities.aggregate(Sum('calories_burned'))['calories_burned__sum'] or 0
        total_points = activities.aggregate(Sum('points'))['points__sum'] or 0
        total_duration = activities.aggregate(Sum('duration'))['duration__sum'] or 0

        # Actividades por categoría
        by_category = activities.values('exercise_type__category').annotate(
            total_activities=models.Count('id'),
            total_calories=models.Sum('calories_burned'),
            total_duration=models.Sum('duration')
        )

        return Response({
            'total_calories': total_calories,
            'total_points': total_points,
            'total_duration': total_duration,
            'by_category': by_category
        })

@api_view(['GET'])
@permission_classes([IsAuthenticated])
@api_view(['GET'])
@permission_classes([IsAuthenticated])
def get_current_user(request):
    """
    Endpoint para obtener los datos del usuario actual
    """
    user = request.user
    serializer = UserSerializer(user)
    return Response(serializer.data)

@api_view(['PUT', 'PATCH'])
@permission_classes([IsAuthenticated])
def update_user_profile(request):
    """
    Endpoint para actualizar el perfil del usuario actual
    """
    user = request.user
    
    # Actualizar el perfil del usuario
    try:
        profile = user.profile
    except Profile.DoesNotExist:
        # Crear perfil si no existe
        profile = Profile.objects.create(user=user)
    
    # Usar partial=True para PATCH
    from .serializers import ProfileSerializer
    serializer = ProfileSerializer(profile, data=request.data, partial=(request.method == 'PATCH'))
    
    if serializer.is_valid():
        serializer.save()
        # Devolver el usuario completo con el perfil actualizado
        user_serializer = UserSerializer(user)
        return Response(user_serializer.data)
    return Response(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
