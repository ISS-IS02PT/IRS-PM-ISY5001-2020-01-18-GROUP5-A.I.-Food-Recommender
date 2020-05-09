from rest_framework import serializers
from foodrec.models import Food, Profile

# class FoodSerializer(serializers.HyperlinkedModelSerializer):
#     selfLink = serializers.HyperlinkedIdentityField(view_name='food-detail', read_only=True)
#     class Meta:
#         model = Food
#         fields = ['id', 'FoodName', 'selfLink']

# class FoodDetailSerializer(serializers.HyperlinkedModelSerializer):
#     selfLink = serializers.HyperlinkedIdentityField(view_name='food-detail', read_only=True)
#     class Meta:
#         model = Food
#         fields = ['id', 'FoodName', 'FoodGroup', 'CarbohydrateAmount_g', 'EnergyAmount_kcal', 'ProteinAmount_g', 'TotalFatAmount_g', 'selfLink']


class FoodSerializer(serializers.Serializer):
    FoodIndex = serializers.IntegerField(required=True)
    FoodName = serializers.CharField(required=True)
    FoodGroup = serializers.ChoiceField(Food.FOOD_GROUPS, required=True)
    CarbohydrateAmount_g = serializers.FloatField(required=True)
    EnergyAmount_kcal = serializers.FloatField(required=True)
    ProteinAmount_g = serializers.FloatField(required=True)
    TotalFatAmount_g = serializers.FloatField(required=True)

    IsVegan = serializers.BooleanField(required=True)
    IsVegetarian = serializers.BooleanField(required=True)
    IsHalal = serializers.BooleanField(required=True)

    FoodMealRanking = serializers.ChoiceField(Food.FOOD_GROUPS, required=True)

class ProfileSerializer(serializers.Serializer):
    gender = serializers.ChoiceField(Profile.GENDER, required=True)
    age = serializers.IntegerField(required=True)
    height = serializers.FloatField(required=True)
    weight = serializers.FloatField(required=True)
    activity = serializers.ChoiceField(Profile.ACTIVITY, required=True)
    diet = serializers.ChoiceField(Profile.DIET, required=True)

    def validate_age(self, value):
        if value <= 0 or value > 150:
            raise serializers.ValidationError("Age has to be within the range (0-150]")
        return value

    def validate_height(self, value):
        if value <= 2 or value > 300:
            raise serializers.ValidationError("Height has to be within the range (1-300] Cm")
        return value

    def validate_weight(self, value):
        if value <= 0 or value > 700:
            raise serializers.ValidationError("Weight has to be within the range (1-700] Kg")
        return value

class NutrientNeedsSerializer(serializers.Serializer):
    CarbohydrateAmount_g = serializers.FloatField(required=True)
    EnergyAmount_kcal = serializers.FloatField(required=True)
    ProteinAmount_g = serializers.FloatField(required=True)
    TotalFatAmount_g = serializers.FloatField(required=True)
    diet = serializers.ChoiceField(Profile.DIET, required=True)