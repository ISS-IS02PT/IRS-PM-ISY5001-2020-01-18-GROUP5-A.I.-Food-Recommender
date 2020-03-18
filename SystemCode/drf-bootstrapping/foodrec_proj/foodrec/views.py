from rest_framework import generics, status
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework.reverse import reverse

from .models import Food, Profile, Nutrient
from .serializers import FoodDetailSerializer, FoodSerializer, ProfileSerializer, NutrientSerializer

from rest_framework.views import APIView
from rest_framework.renderers import JSONRenderer

@api_view(['GET'])
def api_root(request, format=None):
    return Response({
        'foods': reverse('food-list', request=request, format=format),   # Display the 'food-list' url in the urls.py
        'calculateNutrientNeeds': reverse('calculate-nutrient-needs', request=request, format=format)   # Display the 'food-list' url in the urls.py
    })   

class FoodList(generics.ListCreateAPIView):
    queryset = Food.objects.all()
    serializer_class = FoodSerializer


class FoodDetail(generics.RetrieveUpdateDestroyAPIView):
    queryset = Food.objects.all()
    serializer_class = FoodDetailSerializer



class CalculateNutrientNeeds(APIView):
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


        # Create the Nutrient object, and calculate the output recommended nutrients
        # Will be replaced with PyKE in next iteration
        n = Nutrient(profile=p)
        n.calculate()


        # Return the data
        data = NutrientSerializer(n).data
        return Response(data, status=status.HTTP_200_OK)
