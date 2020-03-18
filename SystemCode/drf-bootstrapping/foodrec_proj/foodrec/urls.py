from django.urls import path
from rest_framework.urlpatterns import format_suffix_patterns
from foodrec import views

# API endpoints
urlpatterns = format_suffix_patterns([
    path('', views.api_root),
    path('calculateNutrientNeeds/', views.CalculateNutrientNeeds.as_view(), name='calculate-nutrient-needs'),
    path('foods/', views.FoodList.as_view(), name='food-list'),
    path('foods/<int:pk>/', views.FoodDetail.as_view(), name='food-detail'),
])