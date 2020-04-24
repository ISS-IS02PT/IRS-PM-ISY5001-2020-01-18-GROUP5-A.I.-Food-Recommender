from django.urls import path
from rest_framework.urlpatterns import format_suffix_patterns
from foodrec import views

# API endpoints
urlpatterns = format_suffix_patterns([
    path('', views.api_root),
    path('calculate-nutrient-needs-from-profile/', views.CalculateNutrientNeedsFromProfile.as_view(), name='calculate-nutrient-needs-from-profile'),
    path('food-recommendation-from-nutrient-needs/', views.FoodRecommendationFromNutrientNeeds.as_view(), name='food-recommendation-from-nutrient-needs'),
    # path('food-recommendation-from-profile/', views.FoodRecommendationFromProfile.as_view(), name='food-recommendation-from-profile'),
    # path('foods/', views.FoodList.as_view(), name='food-list'),
    # path('foods/<int:pk>/', views.FoodDetail.as_view(), name='food-detail'),
])