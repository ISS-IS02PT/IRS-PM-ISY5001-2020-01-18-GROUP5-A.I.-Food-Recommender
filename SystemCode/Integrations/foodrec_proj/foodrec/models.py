from django.db import models
import os
from django.conf import settings

#### PyKE import ####
from .models_pyke.knowledge import pyke_load_engine, pyke_calculate_bmr, pyke_calculate_EnergyAmount_kcal, pyke_calculate_Nutrients
pyke_load_engine()

######################################################################
# Initialize the data once when Django loads up
# -- will improve in next iteration
######################################################################

from .models_ortools_Ken import readFoodData
csv_file = open(os.path.join(settings.BASE_DIR, 'foodrec/Dataset/foodDataBase_PleaseUseThis_DC2.csv'))
readFoodData(csv_file)



class Food(models.Model):
    FOOD_GROUPS = sorted(
                    [
                        ("BEVERAGES", "BEVERAGES"), 
                        ("CEREAL AND CEREAL PRODUCTS", "CEREAL AND CEREAL PRODUCTS"),
                        ("EGG AND EGG PRODUCTS", "EGG AND EGG PRODUCTS"),
                        ("FAST FOODS", "FAST FOODS"),
                        ("FISH AND FISH PRODUCTS", "FISH AND FISH PRODUCTS"),
                        ("FRUIT AND FRUIT PRODUCTS", "FRUIT AND FRUIT PRODUCTS"),
                        ("HEALTHIER CHOICE SYMBOL (HCS) PRODUCTS", "HEALTHIER CHOICE SYMBOL (HCS) PRODUCTS"),
                        ("MEAT AND MEAT PRODUCTS", "MEAT AND MEAT PRODUCTS"),
                        ("MILK AND MILK PRODUCTS", "MILK AND MILK PRODUCTS"),
                        ("MISCELLANEOUS", "MISCELLANEOUS"),
                        ("MIXED ETHNIC DISHES, ANALYZED IN SINGAPORE", "MIXED ETHNIC DISHES, ANALYZED IN SINGAPORE"),
                        ("NUTS AND SEEDS, PULSES AND PRODUCTS", "NUTS AND SEEDS, PULSES AND PRODUCTS"),
                        ("OILS AND FATS", "OILS AND FATS"),
                        ("OTHER MIXED ETHNIC DISHES", "OTHER MIXED ETHNIC DISHES"),
                        ("SUGARS, SWEETS AND CONFECTIONERY", "SUGARS, SWEETS AND CONFECTIONERY"),
                        ("VEGETABLE AND VEGETABLE PRODUCTS", "VEGETABLE AND VEGETABLE PRODUCTS"),
                    ])

    MEALS = [
                ("BREAKFAST", "BREAKFAST"),
                ("LUNCH", "LUNCH"),
                ("DINNER", "DINNER"),
            ]

    FoodName = models.CharField(max_length=100, blank=False)
    FoodGroup = models.CharField(max_length=100, choices=FOOD_GROUPS, default='MISCELLANEOUS')
    CarbohydrateAmount_g = models.FloatField()
    EnergyAmount_kcal = models.FloatField()
    ProteinAmount_g = models.FloatField()
    TotalFatAmount_g = models.FloatField()
    FoodIndex = models.IntegerField()
    FoodMealRanking = models.CharField(max_length=100, choices=MEALS)

    def __repr__(self):
        return self.FoodName

    def __str__(self):
        return self.FoodName

    class Meta:
        ordering = ['FoodName']



#######################################################
# Non-Django classes for now
#######################################################
class Profile:
    # tuple (value, display_text) for GENDER
    GENDER = (
        ('male', 'Male'),
        ('female', 'Female'),
    )

    ACTIVITY = (
        ('sedentary', 'Sedentary'),
        ('lightly_active', 'Lightly Active'),
        ('moderately_active', 'Moderately Active'),
        ('very_active', 'Very Active'),
    )

    DIET = (
        ('standard', 'Standard'),
        ('ketogenic', 'Ketogenic'),
    )

    def __init__(self, gender=None, age=None, height=None, weight=None, activity=None, diet=None):
        self.gender = gender
        self.age = age
        self.height = height
        self.weight = weight
        self.activity = activity
        self.diet = diet
        self.nutrientNeeds = NutrientNeeds(diet=diet,profile=self)    # Initialize an empty NutriendNeeds object

class NutrientNeeds:
    def __init__(self, CarbohydrateAmount_g=None, EnergyAmount_kcal=None, ProteinAmount_g=None, TotalFatAmount_g=None, diet=None, profile=None):
        self.CarbohydrateAmount_g = CarbohydrateAmount_g
        self.EnergyAmount_kcal = EnergyAmount_kcal
        self.ProteinAmount_g = ProteinAmount_g
        self.TotalFatAmount_g = TotalFatAmount_g
        self.diet = diet
        self.profile = profile

    # Calculate the Nutrient Needs
    def calculate(self):
        # Firstly, calculate the BMR
        bmr =float('%.2f' % (pyke_calculate_bmr(self.profile.gender, self.profile.weight, self.profile.height, self.profile.age)))
        
        # Calculate the Energy
        # EnergyAmount_kcal
        self.EnergyAmount_kcal = pyke_calculate_EnergyAmount_kcal(bmr, self.profile.activity)
        
        # Calculate the nutrients
        # CarbohydrateAmount_g, TotalFatAmount_g, ProteinAmount_g
        # * Standard diet = balance ratio of Carbs:Fat:Protein = 40%:30%:30%
        # * Keto diet = 75% Fat, 20% Protein, 5% Carb or <50grams
        self.CarbohydrateAmount_g, self.TotalFatAmount_g, self.ProteinAmount_g = pyke_calculate_Nutrients(self.EnergyAmount_kcal, self.diet)

        