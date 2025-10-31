from django.urls import path, include
from . import views
from rest_framework.routers import DefaultRouter

router = DefaultRouter()
router.register(r'teams', views.TeamViewSet)
router.register(r'memberships', views.TeamMembershipViewSet, basename='membership')
router.register(r'exercise-types', views.ExerciseTypeViewSet)
router.register(r'workout-plans', views.WorkoutPlanViewSet, basename='workout-plan')
router.register(r'activities', views.ActivityViewSet, basename='activity')

workout_router = DefaultRouter()
workout_router.register(r'exercises', views.ExerciseViewSet, basename='exercise')

urlpatterns = [
    # Autenticación y usuarios
    path('register/', views.RegisterView.as_view(), name='register'),
    path('me/profile/update/', views.update_user_profile, name='update-user-profile'),
    path('me/profile/', views.get_current_user, name='current-user'),
    path('me/', views.UserDetailView.as_view(), name='user-detail'),

    # API endpoints
    path('', include(router.urls)),
    path('workout-plans/<int:workout_plan_pk>/', include(workout_router.urls)),
]