from django.db import models

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


class Food(models.Model):
    name = models.CharField(max_length=100, blank=False)
    group = models.CharField(max_length=100, choices=FOOD_GROUPS, default='MISCELLANEOUS')
    nutrient_protein_amount = models.FloatField()
    nutrient_protein_unit = models.CharField(max_length=10)
    nutrient_sugar_amount = models.FloatField()
    nutrient_sugar_unit = models.CharField(max_length=10)

    class Meta:
        ordering = ['name']

    def __repr__(self):
        return self.name


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
        ('very_active', 'Very Active')
    )

    def __init__(self, gender=None, age=None, height=None, weight=None, activity=None):
        self.gender = gender
        self.age = age
        self.height = height
        self.weight = weight
        self.activity = activity

class Nutrient:
    def __init__(self, calories=None, carbs=None, fat=None, protein=None, profile=None):
        self.calories = calories
        self.carbs = carbs
        self.fat = fat
        self.protein = protein
        self.profile = profile

    def calculate(self):
        # Firstly, calculate the BMR
        bmr = None
        if self.profile.gender == 'Male':
            bmr = 10 * self.profile.weight + 6.25 * self.profile.height - 5 * self.profile.age + 5
        else:
            bmr = 10 * self.profile.weight + 6.25 * self.profile.height - 5 * self.profile.age - 161
        
        # Calculate the nutrients
        # Calories
        if self.profile.activity == 'sedentary':
            self.calories = bmr * 1.2
        elif self.profile.activity == 'lightly_active':
            self.calories = bmr * 1.375
        elif self.profile.activity == 'moderately_active':
            self.calories = bmr * 1.55
        elif self.profile.activity == 'very_active':
            self.calories = bmr * 1.725
        self.calories = float('%.2f' % (self.calories))

        # Assuming balance ratio of Carbs:Fat:Protein = 40%:30%:30%
        # - Carbohydrates provide 4 Calories of energy per gram
        # - Fats provide 9 Calories of energy per gram
        # - Protein provide 4 Calories of energy per gram
        self.carbs = float('%.2f' % (self.calories * 0.4 / 4))
        self.fat = float('%.2f' % (self.calories * 0.3 / 9))
        self.protein = float('%.2f' % (self.calories * 0.3 / 4))
