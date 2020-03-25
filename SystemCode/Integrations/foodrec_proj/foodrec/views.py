from rest_framework import generics, status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework.reverse import reverse

from .models import Food, Profile, NutrientNeeds
from .serializers import FoodSerializer, ProfileSerializer, NutrientNeedsSerializer

from rest_framework.views import APIView
from rest_framework.renderers import JSONRenderer

from .models_ortools import run_optimizer, food_data

DATA_FoodName_INDEX = 0
DATA_FoodGroup_INDEX = 1
DATA_CarbohydrateAmount_g_INDEX = 2
DATA_EnergyAmount_kcal_INDEX = 3
DATA_ProteinAmount_g_INDEX = 4
DATA_TotalFatAmount_g_INDEX = 5

# class FoodList(generics.ListCreateAPIView):
#     queryset = Food.objects.all()
#     serializer_class = FoodSerializer


# class FoodDetail(generics.RetrieveUpdateDestroyAPIView):
#     queryset = Food.objects.all()
#     serializer_class = FoodDetailSerializer


@api_view(['GET'])
def api_root(request, format=None):
    return Response({
        # 'foods': reverse('food-list', request=request, format=format),   # Display the 'food-list' url in the urls.py
        'calculate-nutrient-needs-from-profile': reverse('calculate-nutrient-needs-from-profile', request=request, format=format),
        'food-recommendation-from-nutrient-needs': reverse('food-recommendation-from-nutrient-needs', request=request, format=format),
        'food-recommendation-from-profile': reverse('food-recommendation-from-profile', request=request, format=format),
    })   

class CalculateNutrientNeedsFromProfile(APIView):
    # Allow only 'get' method
    http_method_names = ['get']
    
    def get(self, request, format=None):
        # Validate the input data and create the Profile object
        ps = ProfileSerializer(data=request.query_params)
        if not ps.is_valid():
            return Response(
                data=ps.errors,
                status=status.HTTP_400_BAD_REQUEST
            )
        # return Response(ps.validated_data, status=status.HTTP_200_OK)

        p = Profile()
        p.gender = ps.validated_data['gender']
        p.age = ps.validated_data['age']
        p.height = ps.validated_data['height']
        p.weight = ps.validated_data['weight']
        p.activity = ps.validated_data['activity']
        p.diet = ps.validated_data['diet']
        p.nutrientNeeds.diet = p.diet

        # Calculate the Nutrient Needs
        p.nutrientNeeds.calculate()

        # Return the data to the API call
        data = NutrientNeedsSerializer(p.nutrientNeeds).data
        return Response(data, status=status.HTTP_200_OK)


class FoodRecommendationFromNutrientNeeds(APIView):
    # Allow only 'get' method
    http_method_names = ['get']
    
    def get(self, request, format=None):
        # nn_keys = ['CarbohydrateAmount_g', 'EnergyAmount_kcal', 'EnergyAmount_kcal', 'TotalFatAmount_g', 'diet']
        # nns = NutrientNeedsSerializer(data={key:request.query_params[key] for key in nn_keys})
        nns = NutrientNeedsSerializer(data=request.query_params)
        if not nns.is_valid():
            return Response(
                data=nns.errors,
                status=status.HTTP_400_BAD_REQUEST
            )

        nn = NutrientNeeds()
        nn.CarbohydrateAmount_g = nns.validated_data['CarbohydrateAmount_g']
        nn.EnergyAmount_kcal = nns.validated_data['EnergyAmount_kcal']
        nn.ProteinAmount_g = nns.validated_data['ProteinAmount_g']
        nn.TotalFatAmount_g = nns.validated_data['TotalFatAmount_g']
        nn.diet = nns.validated_data['diet']

        # Run the optimizer
        foodIndex_result = run_optimizer(EnergyAmount_kcal=nn.EnergyAmount_kcal)

        # Return the result to the API call
        food_result = []
        for i in foodIndex_result:
            food = Food()
            food.FoodName = food_data[i][DATA_FoodName_INDEX]
            food.FoodGroup = food_data[i][DATA_FoodGroup_INDEX]
            food.CarbohydrateAmount_g = food_data[i][DATA_CarbohydrateAmount_g_INDEX]
            food.EnergyAmount_kcal = food_data[i][DATA_EnergyAmount_kcal_INDEX]
            food.ProteinAmount_g = food_data[i][DATA_ProteinAmount_g_INDEX]
            food.TotalFatAmount_g = food_data[i][DATA_TotalFatAmount_g_INDEX]

            food_result.append(food)
        
        data = FoodSerializer(food_result, many=True).data
        return Response(data, status=status.HTTP_200_OK)



class FoodRecommendationFromProfile(APIView):
    # Allow only 'get' method
    http_method_names = ['get']
    
    def get(self, request, format=None):
        # Validate the input data and create the Profile object
        ps = ProfileSerializer(data=request.query_params)
        if not ps.is_valid():
            return Response(
                data=ps.errors,
                status=status.HTTP_400_BAD_REQUEST
            )
        # return Response(ps.validated_data, status=status.HTTP_200_OK)

        p = Profile()
        p.gender = ps.validated_data['gender']
        p.age = ps.validated_data['age']
        p.height = ps.validated_data['height']
        p.weight = ps.validated_data['weight']
        p.activity = ps.validated_data['activity']
        p.diet = ps.validated_data['diet']
        p.nutrientNeeds.diet = p.diet

        # Calculate the Nutrient Needs
        p.nutrientNeeds.calculate()

        # Run the optimizer
        foodIndex_result = run_optimizer(EnergyAmount_kcal=p.nutrientNeeds.EnergyAmount_kcal)

        # Return the result to the API call
        food_result = []
        for i in foodIndex_result:
            food = Food()
            food.FoodName = food_data[i][DATA_FoodName_INDEX]
            food.FoodGroup = food_data[i][DATA_FoodGroup_INDEX]
            food.CarbohydrateAmount_g = food_data[i][DATA_CarbohydrateAmount_g_INDEX]
            food.EnergyAmount_kcal = food_data[i][DATA_EnergyAmount_kcal_INDEX]
            food.ProteinAmount_g = food_data[i][DATA_ProteinAmount_g_INDEX]
            food.TotalFatAmount_g = food_data[i][DATA_TotalFatAmount_g_INDEX]

            food_result.append(food)
        
        data = FoodSerializer(food_result, many=True).data
        return Response(data, status=status.HTTP_200_OK)