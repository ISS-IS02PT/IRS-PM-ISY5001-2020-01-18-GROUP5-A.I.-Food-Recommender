from rest_framework import generics, status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework.reverse import reverse

from .models import Food, Profile, NutrientNeeds
from .serializers import FoodSerializer, ProfileSerializer, NutrientNeedsSerializer

from rest_framework.views import APIView
from rest_framework.renderers import JSONRenderer

from .models_ortools_Ken import run_optimizer, \
                                food_data, \
                                DATA_FoodName_INDEX, \
                                DATA_FoodGroup_INDEX, \
                                DATA_CarbohydrateAmount_g_INDEX, \
                                DATA_EnergyAmount_kcal_INDEX, \
                                DATA_ProteinAmount_g_INDEX, \
                                DATA_TotalFatAmount_g_INDEX, \
                                DATA_IsMainDish_INDEX, \
                                DATA_IsFastFood_INDEX, \
                                DATA_IsBreakfast_INDEX

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
        # 'food-recommendation-from-profile': reverse('food-recommendation-from-profile', request=request, format=format),
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
        nn_keys = ['CarbohydrateAmount_g', 'EnergyAmount_kcal', 'ProteinAmount_g', 'TotalFatAmount_g', 'diet']
        nns = NutrientNeedsSerializer(data={key:request.query_params[key] for key in nn_keys})
        
        food_keep_index = convert_string_to_array(request.query_params['food_keep_index'])
        food_change_index = convert_string_to_array(request.query_params['food_change_index'])

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
        foodIndex_result = run_optimizer(EnergyAmount_kcal=nn.EnergyAmount_kcal, CarbohydrateAmount_g=nn.CarbohydrateAmount_g, ProteinAmount_g=nn.ProteinAmount_g, TotalFatAmount_g=nn.TotalFatAmount_g, num_meals=3, food_keep_index=food_keep_index, food_change_index=food_change_index)

        # Return the result to the API call
        food_result = prepare_food_response(foodIndex_result)
        
        data = FoodSerializer(food_result, many=True).data
        data.append({'food_keep_index': convert_array_to_string(food_keep_index), 'food_change_index': convert_array_to_string(food_change_index)})
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
        nn = p.nutrientNeeds

        # Run the optimizer
        foodIndex_result = run_optimizer(EnergyAmount_kcal=nn.EnergyAmount_kcal, CarbohydrateAmount_g=nn.CarbohydrateAmount_g, ProteinAmount_g=nn.ProteinAmount_g, TotalFatAmount_g=nn.TotalFatAmount_g, num_meals=3)

        # Return the result to the API call
        food_result = prepare_food_response(foodIndex_result)
    
        data = FoodSerializer(food_result, many=True).data
        return Response(data, status=status.HTTP_200_OK)

##############################
###### Helper functions ######
##############################

# Prepare Food Response
def prepare_food_response(foodIndex_result):
    food_result = []
    max_calories = 0

    for i in foodIndex_result:
        food = Food()
        food.FoodIndex = i
        food.FoodName = food_data[i][DATA_FoodName_INDEX]
        food.FoodGroup = food_data[i][DATA_FoodGroup_INDEX]
        food.CarbohydrateAmount_g = food_data[i][DATA_CarbohydrateAmount_g_INDEX]
        food.EnergyAmount_kcal = food_data[i][DATA_EnergyAmount_kcal_INDEX]
        food.ProteinAmount_g = food_data[i][DATA_ProteinAmount_g_INDEX]
        food.TotalFatAmount_g = food_data[i][DATA_TotalFatAmount_g_INDEX]
        food.FoodMealRanking = -1

        if food_data[i][DATA_IsBreakfast_INDEX]:
            food.FoodMealRanking = 'BREAKFAST'
        else:
            if food.EnergyAmount_kcal > max_calories:
                max_calories = food.EnergyAmount_kcal

        food_result.append(food)
    
    for food in food_result:
        if food.FoodMealRanking == -1:
            if food.EnergyAmount_kcal == max_calories:
                food.FoodMealRanking = 'LUNCH'
            else:
                food.FoodMealRanking = 'DINNER'

    return food_result

def convert_string_to_array(str):
    arr = []
    for element in str.split(","):
        element = element.strip()
        if len(element) > 0:
            arr += [int(element)]
    return arr

def convert_array_to_string(arr):
    return ','.join(str(e) for e in arr)