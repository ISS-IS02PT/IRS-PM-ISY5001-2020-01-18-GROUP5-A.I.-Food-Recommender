
# Update - 25/Apr
## 1. Only 2 x API endpoints remain
* The 'food-recommendation-from-profile' endpoint is deprecated, given the fact we want to allow user to make modifications before submitting to the optimization
* Therefore, from the client side, please make 2 requests from profile -> nutrients, and nutrients->food recommendation
```bash
$ http http://localhost:8000/

{
    "calculate-nutrient-needs-from-profile": "http://localhost:8000/calculate-nutrient-needs-from-profile/",
    "food-recommendation-from-nutrient-needs": "http://localhost:8000/food-recommendation-from-nutrient-needs/"
}
```

## 2. PyKE rules implemented for 'ketogenic' diet type, besides 'standard'
```bash
# Standard
$ http http://localhost:8000/calculate-nutrient-needs-from-profile/ age==32 height==182 weight==72 gender==male activity==very_active diet==standard

{
    "CarbohydrateAmount_g": 293.68,
    "EnergyAmount_kcal": 2936.8125,
    "ProteinAmount_g": 220.26,
    "TotalFatAmount_g": 97.89,
    "diet": "standard"
}

# Ketogenic
$ http http://localhost:8000/calculate-nutrient-needs-from-profile/ age==32 height==182 weight==72 gender==male activity==very_active diet==ketogenic

{
    "CarbohydrateAmount_g": 36.71,
    "EnergyAmount_kcal": 2936.8125,
    "ProteinAmount_g": 146.84,
    "TotalFatAmount_g": 244.73,
    "diet": "ketogenic"
}
```

## 3. Changes to the 'food-recommendation-from-nutrient-needs' api endpoint.

### 3.1. Additional Input parameters for the request
* Restrictions:
    + 'isVegan'
    + 'isVegetarian'
    + 'isHalal'
    + 'containsBeef'
    + 'isAlcohol'
* Adhoc session variables:
    + 'food_keep_index': List of 'FoodIndex' that we want to keep in the optimization result
    + 'food_change_index': List of 'FoodIndex' that we don't want to keep in the optimization result. These won't be considered at all.

### 3.2. Returned JSON output response:
* More information about each food:
    + 5 x Labels for the restriction of this food
    + 'FoodIndex': Index of this food in the original database
    + 'FoodMealRanking': For our submission, let's just fix it to 3 meals as returned output. With 3 meals, there is simple logic to suggest the meal of this food, either "Breakfast", "Lunch" or "Dinner"
* Adhoc session variables:
    + food_keep_index: return as-is of that of input, so that it can track history (example of sequence of runs below)
    + food_change_index: return as-is of that of input, so that it can track history (example of sequence of runs below)

```bash
# * Run 1 *
# No food restrictions at all
# - all restriction parameters set to 'false' 
#
# No history for {food_keep_index} and parameter {food_change_index}
# - Just pass in empty string

$ http http://localhost:8000/food-recommendation-from-nutrient-needs/ EnergyAmount_kcal==2936.8125 ProteinAmount_g==220.26 TotalFatAmount_g==97.89 CarbohydrateAmount_g==2
93.68 diet==standard food_keep_index== food_change_index== isVegetarian=
=false isVegan==false isHalal==False containsBeef==False isAlcohol==False

[
    {
        "CarbohydrateAmount_g": 0.0,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 2714.63,
        "FoodGroup": "MEAT AND MEAT PRODUCTS",
        "FoodIndex": 464,
        "FoodMealRanking": "LUNCH",
        "FoodName": "Chicken, broiler or fryer, unspecified cut, raw, meat and skin",
        "IsAlcohol": false,
        "IsHalal": false,
        "IsVegan": false,
        "IsVegetarian": false,
        "ProteinAmount_g": 234.73,
        "TotalFatAmount_g": 190.56
    },
    {
        "CarbohydrateAmount_g": 0.0,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 141.53,
        "FoodGroup": "MEAT AND MEAT PRODUCTS",
        "FoodIndex": 1654,
        "FoodMealRanking": "DINNER",
        "FoodName": "Pork, spare rib, boiled, lean and fat",
        "IsAlcohol": false,
        "IsHalal": false,
        "IsVegan": false,
        "IsVegetarian": false,
        "ProteinAmount_g": 10.36,
        "TotalFatAmount_g": 10.8
    },
    {
        "CarbohydrateAmount_g": 23.5,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 106.0,
        "FoodGroup": "BREAKFAST FOODS",
        "FoodIndex": 2318,
        "FoodMealRanking": "BREAKFAST",
        "FoodName": "Buckwheat Banana Pancakes",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": true,
        "IsVegetarian": true,
        "ProteinAmount_g": 1.8,
        "TotalFatAmount_g": 1.6
    },
    {
        "food_change_index": "",
        "food_keep_index": ""
    }
]

# * Run 2 *
# No food restrictions at all
# - all restriction parameters set to 'false' 
#
# Let's keep the same Lunch and Dinner as Run 1, but change the Breakfast.
# - food_keep_index=464,1654 <-- This is the FoodIndex of each food returned above
# - food_change_index=2318

$ http http://localhost:8000/food-recommendation-from-nutrient-needs/ EnergyAmount_kcal==2936.8125 ProteinAmount_g==220.26 TotalFatAmount_g==97.89 CarbohydrateAmount_g==293.68 diet==standard food_keep_index==464,1654 food_change_index==2318 isVegetarian==false isVegan==false isHalal==False containsBeef==False isAlcohol==False

[
    {
        "CarbohydrateAmount_g": 0.0,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 2714.63,
        "FoodGroup": "MEAT AND MEAT PRODUCTS",
        "FoodIndex": 464,
        "FoodMealRanking": "LUNCH",
        "FoodName": "Chicken, broiler or fryer, unspecified cut, raw, meat and skin",
        "IsAlcohol": false,
        "IsHalal": false,
        "IsVegan": false,
        "IsVegetarian": false,
        "ProteinAmount_g": 234.73,
        "TotalFatAmount_g": 190.56
    },
    {
        "CarbohydrateAmount_g": 0.0,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 141.53,
        "FoodGroup": "MEAT AND MEAT PRODUCTS",
        "FoodIndex": 1654,
        "FoodMealRanking": "DINNER",
        "FoodName": "Pork, spare rib, boiled, lean and fat",
        "IsAlcohol": false,
        "IsHalal": false,
        "IsVegan": false,
        "IsVegetarian": false,
        "ProteinAmount_g": 10.36,
        "TotalFatAmount_g": 10.8
    },
    {
        "CarbohydrateAmount_g": 35.1,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 165.0,
        "FoodGroup": "BREAKFAST FOODS",
        "FoodIndex": 2314,
        "FoodMealRanking": "BREAKFAST",
        "FoodName": "Cinnamon Yogurt with Sliced Apple",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": true,
        "IsVegetarian": true,
        "ProteinAmount_g": 7.5,
        "TotalFatAmount_g": 0.5
    },
    {
        "food_change_index": "2318",
        "food_keep_index": "464,1654"
    }
]

# * Run 3 *
# No food restrictions at all
# - all restriction parameters set to 'false' 
#
# Let's keep the same Lunch and Dinner as Run 1 and 2, but change the Breakfast.
# - food_keep_index=464,1654
# - food_change_index=2318,2314 <-- including both so that it won't give back the same breakfast result as Run 1>

http http://localhost:8000/food-recommendation-from-nutrient-needs/ EnergyAmount_kcal==2936.8125 ProteinAmount_g==220.26 TotalFatAmount_g==97.89 CarbohydrateAmount_g==293.68 diet==standard food_keep_index==464,1654 food_change_index==2318,2314 isVegetarian==false isVegan==false isHalal==False containsBeef==False isAlcohol==False

[
    {
        "CarbohydrateAmount_g": 0.0,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 2714.63,
        "FoodGroup": "MEAT AND MEAT PRODUCTS",
        "FoodIndex": 464,
        "FoodMealRanking": "LUNCH",
        "FoodName": "Chicken, broiler or fryer, unspecified cut, raw, meat and skin",
        "IsAlcohol": false,
        "IsHalal": false,
        "IsVegan": false,
        "IsVegetarian": false,
        "ProteinAmount_g": 234.73,
        "TotalFatAmount_g": 190.56
    },
    {
        "CarbohydrateAmount_g": 0.0,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 141.53,
        "FoodGroup": "MEAT AND MEAT PRODUCTS",
        "FoodIndex": 1654,
        "FoodMealRanking": "DINNER",
        "FoodName": "Pork, spare rib, boiled, lean and fat",
        "IsAlcohol": false,
        "IsHalal": false,
        "IsVegan": false,
        "IsVegetarian": false,
        "ProteinAmount_g": 10.36,
        "TotalFatAmount_g": 10.8
    },
    {
        "CarbohydrateAmount_g": 30.2,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 150.0,
        "FoodGroup": "BREAKFAST FOODS",
        "FoodIndex": 2365,
        "FoodMealRanking": "BREAKFAST",
        "FoodName": "Cinnamon Porridge with Blueberries and Coconut Yogurt",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": true,
        "IsVegetarian": true,
        "ProteinAmount_g": 5.0,
        "TotalFatAmount_g": 2.2
    },
    {
        "food_change_index": "2318,2314",
        "food_keep_index": "464,1654"
    }
]

# * Run 4 *
# Only Vegetarian food
# - set 'isVegetarian' to True
# Setting multiple restrictive flags is possible, but we may not have big enough database to solve
#
# Let's refresh the history
# - Just pass in empty string for parameter {food_keep_index} and parameter {food_change_index}

http http://localhost:8000/food-recommendation-from-nutrient-needs/ EnergyAmount_kcal==2936.8125 ProteinAmount_g==220.26 TotalFatAmount_g==97.89 CarbohydrateAmount_g==293.68 diet==standard food_keep_index== food_change_index== isVegetarian==true isVegan==false isHalal==false containsBeef==false isAlcohol==false

[
    {
        "CarbohydrateAmount_g": 2.32,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 140.2,
        "FoodGroup": "MILK AND MILK PRODUCTS",
        "FoodIndex": 597,
        "FoodMealRanking": "DINNER",
        "FoodName": "Cream cheese powder",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": false,
        "IsVegetarian": true,
        "ProteinAmount_g": 2.78,
        "TotalFatAmount_g": 13.26
    },
    {
        "CarbohydrateAmount_g": 59.08,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 2304.12,
        "FoodGroup": "MIXED ETHNIC DISHES, ANALYZED IN SINGAPORE",
        "FoodIndex": 733,
        "FoodMealRanking": "LUNCH",
        "FoodName": "Egg yolk, powder",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": false,
        "IsVegetarian": true,
        "ProteinAmount_g": 176.4,
        "TotalFatAmount_g": 151.08
    },
    {
        "CarbohydrateAmount_g": 60.1,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 496.0,
        "FoodGroup": "BREAKFAST FOODS",
        "FoodIndex": 2361,
        "FoodMealRanking": "BREAKFAST",
        "FoodName": "Vega One Blackberry, Strawberry, and Acai Smoothie",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": true,
        "IsVegetarian": true,
        "ProteinAmount_g": 16.2,
        "TotalFatAmount_g": 23.8
    },
    {
        "food_change_index": "",
        "food_keep_index": ""
    }
]

# * Run 5 *
# Same as Run 4
# Only Vegetarian food
# - set 'isVegetarian' to True
# Setting multiple restrictive flags is possible, but we may not have big enough database to solve
#
# But let's refresh the result of all 3 foods this time
# - food_keep_index=
# - food_change_index=597,733,2361

http http://localhost:8000/food-recommendation-from-nutrient-needs/ EnergyAmount_kcal==2936.8125 ProteinAmount_g==220.26 TotalFatAmount_g==97.89 CarbohydrateAmount_g==293.68 diet==standard food_keep_index== food_change_index==597,733,2361 isVegetarian==true isVegan==false isHalal==false containsBeef==false isAlcohol==false

[
    {
        "CarbohydrateAmount_g": 9.08,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 870.44,
        "FoodGroup": "MIXED ETHNIC DISHES, ANALYZED IN SINGAPORE",
        "FoodIndex": 1145,
        "FoodMealRanking": "DINNER",
        "FoodName": "Long beans with tempeh, stir fried",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": true,
        "IsVegetarian": true,
        "ProteinAmount_g": 50.88,
        "TotalFatAmount_g": 70.22
    },
    {
        "CarbohydrateAmount_g": 287.7,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 1786.81,
        "FoodGroup": "MILK AND MILK PRODUCTS",
        "FoodIndex": 1232,
        "FoodMealRanking": "LUNCH",
        "FoodName": "Milk, filled",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": false,
        "IsVegetarian": true,
        "ProteinAmount_g": 45.68,
        "TotalFatAmount_g": 49.35
    },
    {
        "CarbohydrateAmount_g": 17.9,
        "ContainsBeef": false,
        "EnergyAmount_kcal": 356.0,
        "FoodGroup": "BREAKFAST FOODS",
        "FoodIndex": 2307,
        "FoodMealRanking": "BREAKFAST",
        "FoodName": "Italian Baked Eggs",
        "IsAlcohol": false,
        "IsHalal": true,
        "IsVegan": true,
        "IsVegetarian": true,
        "ProteinAmount_g": 27.5,
        "TotalFatAmount_g": 19.6
    },
    {
        "food_change_index": "597,733,2361",
        "food_keep_index": ""
    }
]
```