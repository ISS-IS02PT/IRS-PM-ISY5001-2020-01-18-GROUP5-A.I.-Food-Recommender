from rest_framework import serializers
from foodrec.models import Food, Profile

class FoodSerializer(serializers.HyperlinkedModelSerializer):
    selfLink = serializers.HyperlinkedIdentityField(view_name='food-detail', read_only=True)
    class Meta:
        model = Food
        fields = ['id', 'name', 'selfLink']

class FoodDetailSerializer(serializers.HyperlinkedModelSerializer):
    selfLink = serializers.HyperlinkedIdentityField(view_name='food-detail', read_only=True)
    class Meta:
        model = Food
        fields = ['id', 'name', 'group', 'nutrient_protein_amount', 'nutrient_protein_unit', 'nutrient_sugar_amount', 'nutrient_sugar_unit', 'selfLink']






class ProfileSerializer(serializers.Serializer):
    gender = serializers.ChoiceField(Profile.GENDER, required=True)
    age = serializers.IntegerField(required=True)
    height = serializers.FloatField(required=True)
    weight = serializers.FloatField(required=True)
    activity = serializers.ChoiceField(Profile.ACTIVITY, required=True)

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

class NutrientSerializer(serializers.Serializer):
    calories = serializers.FloatField()
    protein = serializers.FloatField()
    carbs = serializers.FloatField()
    fat = serializers.FloatField()


