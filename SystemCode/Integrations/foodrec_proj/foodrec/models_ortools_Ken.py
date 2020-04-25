from __future__ import print_function
from ortools.linear_solver import pywraplp

import pandas as pd
import numpy as np
from numpy.random import seed
from numpy.random import rand, randint, random
import time


# Constants and Global variables
DATA_FoodName_INDEX = -1
DATA_FoodGroup_INDEX = -1
DATA_CarbohydrateAmount_g_INDEX = -1
DATA_EnergyAmount_kcal_INDEX = -1
DATA_ProteinAmount_g_INDEX = -1
DATA_TotalFatAmount_g_INDEX = -1
DATA_IsMainDish_INDEX = -1
DATA_IsFastFood_INDEX = -1
DATA_IsBreakfast_INDEX = -1
# DATA_IsOthers_INDEX = -1
DATA_Vegan_INDEX = -1
DATA_Vegetarian_INDEX = -1
DATA_Halal_INDEX = -1
DATA_ContainsBeef_INDEX = -1
DATA_Alcohol_INDEX = -1



food_data = None
#csv_file = 'Dataset/FoodDatabase.csv'
csv_file = 'Dataset/foodDataBase_PleaseUseThis_DC2.csv'
NUM_FOOD = 100
# NUM_FOOD = 3920


def readFoodData(csv_file):
    df = pd.read_csv(csv_file)[['FoodName',
                                'FoodGroup',
                                'CarbohydrateAmount_g',
                                'EnergyAmount_kcal',
                                'ProteinAmount_g',
                                'TotalFatAmount_g',
                                'IsMainDish',
                                'IsFastFood',
                                'IsBreakfast',
                                'Vegan',
                                'Vegetarian',
                                'Halal',
                                'ContainsBeef',
                                'Alcohol']]
    
    # Update the index here for the data arrays to point to different nutrients
    global DATA_FoodName_INDEX, DATA_FoodGroup_INDEX, DATA_CarbohydrateAmount_g_INDEX, \
        DATA_EnergyAmount_kcal_INDEX, DATA_ProteinAmount_g_INDEX, DATA_TotalFatAmount_g_INDEX, \
        DATA_IsMainDish_INDEX, DATA_IsFastFood_INDEX, DATA_IsBreakfast_INDEX, DATA_Vegan_INDEX, \
        DATA_Vegetarian_INDEX, DATA_Halal_INDEX, DATA_ContainsBeef_INDEX, DATA_Alcohol_INDEX
      
    DATA_FoodName_INDEX = 0
    DATA_FoodGroup_INDEX = 1
    DATA_CarbohydrateAmount_g_INDEX = 2
    DATA_EnergyAmount_kcal_INDEX = 3
    DATA_ProteinAmount_g_INDEX = 4
    DATA_TotalFatAmount_g_INDEX = 5
    DATA_IsMainDish_INDEX = 6
    DATA_IsFastFood_INDEX = 7
    DATA_IsBreakfast_INDEX = 8
    DATA_Vegan_INDEX = 9
    DATA_Vegetarian_INDEX = 10
    DATA_Halal_INDEX = 11
    DATA_ContainsBeef_INDEX = 12
    DATA_Alcohol_INDEX = 13

    # Filter food with Energy > 100kcal
    df = df.loc[df['EnergyAmount_kcal'] > 100]

    global food_data
    # food_data = df.head(NUM_FOOD).to_numpy()
    food_data = df.to_numpy()

def optimizer1(EnergyAmount_kcal, CarbohydrateAmount_g, ProteinAmount_g, TotalFatAmount_g, food_keep_index, food_change_index, num_meals, isVegan, isVegetarian, isHalal, containsBeef, isAlcohol):
    # Create the mip solver with the CBC backend
    solver = pywraplp.Solver('optimizer_refresh1',
                             pywraplp.Solver.CBC_MIXED_INTEGER_PROGRAMMING)

    print("Total food reading in from database: {}".format(len(food_data)))
    #### Variables ####
    # Declare an array to hold the variable value whether the food is selected, which is 0 or 1 for each food
    foodVar = [[]] * len(food_data)

    def returnBoundValue(bool):
        if bool:
            return 1.0
        else:
            return 0.0

    def determineLowerBound(index, food_keep_index):
        if index in food_keep_index:
            return 1.0
        else:
            return 0.0

    def determineUpperBound(index, food_change_index, isVegan, isVegetarian, isHalal, containsBeef, isAlcohol):
        upperBound = 1.0

        if index in food_change_index:
            upperBound = 0.0

        if isVegan and not food_data[index][DATA_Vegan_INDEX]:
            upperBound = 0.0
        if isVegetarian and not food_data[index][DATA_Vegetarian_INDEX]:
            upperBound = 0.0
        if isHalal and not food_data[index][DATA_Halal_INDEX]:
            upperBound = 0.0
        if containsBeef and not food_data[index][DATA_ContainsBeef_INDEX]:
            upperBound = 0.0
        if isAlcohol and not food_data[index][DATA_Alcohol_INDEX]:
            upperBound = 0.0

        return upperBound


    #### Objective ####
    # Declare the objective function
    objective = solver.Objective()
    for i in range(0, len(food_data)):
        # print("{} {}", determineLowerBound(i, food_keep_index), determineUpperBound(i, food_change_index, isVegan, isVegetarian, isHalal, containsBeef, isAlcohol))
        foodVar[i] = solver.IntVar(determineLowerBound(i, food_keep_index), determineUpperBound(i, food_change_index, isVegan, isVegetarian, isHalal, containsBeef, isAlcohol), food_data[i][DATA_FoodName_INDEX])

        # # Food Refresh
        # if i in food_keep_index:
        #     foodVar[i] = solver.IntVar(1.0, 1.0, food_data[i][DATA_FoodName_INDEX])
        # elif i in food_change_index:
        #     foodVar[i] = solver.IntVar(0.0, 0.0, food_data[i][DATA_FoodName_INDEX])
        # else:
        #     foodVar[i] = solver.IntVar(0.0, 1.0, food_data[i][DATA_FoodName_INDEX])

        # # Restriction
        # if isVegan:
        #     foodVar[i] = solver.IntVar(0.0, food_data[i][DATA_Vegan_INDEX], food_data[i][DATA_FoodName_INDEX])
        # if isVegetarian:
        #     foodVar[i] = solver.IntVar(0.0, food_data[i][DATA_Vegetarian_INDEX], food_data[i][DATA_FoodName_INDEX])
        # if isHalal:
        #     foodVar[i] = solver.IntVar(0.0, food_data[i][DATA_Halal_INDEX], food_data[i][DATA_FoodName_INDEX])
        # if containsBeef:
        #     foodVar[i] = solver.IntVar(0.0, food_data[i][DATA_ContainsBeef_INDEX], food_data[i][DATA_FoodName_INDEX])
        # if isAlcohol:
        #     foodVar[i] = solver.IntVar(0.0, food_data[i][DATA_Alcohol_INDEX], food_data[i][DATA_FoodName_INDEX])

        # The coeficient for the objective function is the amount of calories for the corresponding food
        objective.SetCoefficient(foodVar[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])

    # Minimize the calories
    objective.SetMinimization()

    #### Contraints ####
    # Energy, Carbohydrate, Protein, Fat
    min = 0
    max = 0.03
    constraint_total_energy = solver.Constraint(EnergyAmount_kcal * (1 + (min + (random() * (max - min)))), EnergyAmount_kcal * 1.1)
    # constraint_total_carbohydrate = solver.Constraint(CarbohydrateAmount_g * 0.95, CarbohydrateAmount_g * 1.05)
    # constraint_total_protein = solver.Constraint(ProteinAmount_g * 0.9, ProteinAmount_g * 1.1)
    # constraint_total_fat = solver.Constraint(TotalFatAmount_g * 0.8, TotalFatAmount_g * 1.2)
    for i in range(0, len(food_data)):
        constraint_total_energy.SetCoefficient(foodVar[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])
        # constraint_total_carbohydrate.SetCoefficient(foodVar[i], food_data[i][DATA_CarbohydrateAmount_g_INDEX])
        # constraint_total_protein.SetCoefficient(foodVar[i], food_data[i][DATA_ProteinAmount_g_INDEX])
        # constraint_total_fat.SetCoefficient(foodVar[i], food_data[i][DATA_TotalFatAmount_g_INDEX])
        
    # # Introduce randomness into the solver by setting available foods from input list randomly
    # random_arr = rand(len(food_data))
    # count_ones = 0
    # for i in range(0, len(random_arr)):
    #     random_arr[i] = round(random_arr[i])
    #     count_ones += random_arr[i]
    # constraint_random_available_foods = solver.Constraint(num_meals, num_meals)
    # for i in range(0, len(food_data)):
    #     constraint_random_available_foods.SetCoefficient(foodVar[i], random_arr[i])
    # print("Total food remaining for selection after randomness: {}".format(len(food_data) - count_ones))

    # numer of meals
    constraint_count_meals = solver.Constraint(num_meals, num_meals)
    for i in range(0, len(food_data)):
        constraint_count_meals.SetCoefficient(foodVar[i], 1)

    # 1 x Breakfast
    constraint_count_breakfast = solver.Constraint(1, 1)
    for i in range(0, len(food_data)):
        constraint_count_breakfast.SetCoefficient(foodVar[i], food_data[i][DATA_IsBreakfast_INDEX])

    # # [0,1] x FastFood
    # constraint_fastfood = solver.Constraint(0, 1)
    # for i in range(0, len(food_data)):
    #     constraint_fastfood.SetCoefficient(foodVar[i], food_data[i][DATA_IsFastFood_INDEX])
    
    # # [1,2] x Main
    # constraint_main = solver.Constraint(1, 2)
    # for i in range(0, len(food_data)):
    #     constraint_main.SetCoefficient(foodVar[i], food_data[i][DATA_IsMainDish_INDEX])
    
    # # Main should be having more calories compared to Breakfast
    # constraint_main_energy_1 = solver.Constraint(0, solver.infinity())
    # for i in range(0, len(food_data)):
    #     constraint_main_energy_1.SetCoefficient(foodVar[i], food_data[i][DATA_EnergyAmount_kcal_INDEX] * (food_data[i][DATA_IsMainDish_INDEX] - food_data[i][DATA_IsBreakfast_INDEX]))
                                                
    # # Main should be having more calories compared to FastFood
    # constraint_main_energy_2 = solver.Constraint(0, solver.infinity())
    # for i in range(0, len(food_data)):
    #     constraint_main_energy_2.SetCoefficient(foodVar[i], food_data[i][DATA_EnergyAmount_kcal_INDEX] * (food_data[i][DATA_IsMainDish_INDEX] - food_data[i][DATA_IsFastFood_INDEX]))

    # # Meals with range
    # # Breakfast with calories within [0% - 10%]
    # constraint_breakfast_energy = solver.Constraint(0, EnergyAmount_kcal * 1.1 * 0.1)
    # for i in range(0, len(food_data)):
    #     constraint_breakfast_energy.SetCoefficient(foodVar[i], food_data[i][DATA_EnergyAmount_kcal_INDEX] * food_data[i][DATA_IsBreakfast_INDEX])

    # Restrictions


    # Solve!
    status = solver.Solve()
    foodIndex_result = []

    if status == solver.OPTIMAL:
        solver.NextSolution()
        print('\nAn optimal solution was found.')
        print('Objective value =', solver.Objective().Value())
        for i in range(0, len(food_data)):
            if foodVar[i].solution_value() > 0:
                foodIndex_result.append(i)
                # print('%s = %f' % (data[i][0], food[i].solution_value()))

    else:  # No optimal solution was found.
        if status == solver.FEASIBLE:
            print('\nA potentially suboptimal solution was found.')
        else:
            print('\nThe solver could not solve the problem.')

    return foodIndex_result

def run_optimizer(EnergyAmount_kcal, CarbohydrateAmount_g, ProteinAmount_g, TotalFatAmount_g, food_keep_index=[], food_change_index=[], num_meals=3, isVegan=False, isVegetarian=False, isHalal=False, containsBeef=False, isAlcohol=False):
    return optimizer1(EnergyAmount_kcal, CarbohydrateAmount_g, ProteinAmount_g, TotalFatAmount_g, food_keep_index, food_change_index, num_meals, isVegan, isVegetarian, isHalal, containsBeef, isAlcohol)

# For quick testing without Django
def main():
    readFoodData(csv_file)
    foodIndex_result = run_optimizer(EnergyAmount_kcal=2000, CarbohydrateAmount_g=293.68, ProteinAmount_g=220.26, TotalFatAmount_g=97.89, food_keep_index=[2339,274], food_change_index=[2279,2252,1340,1206], num_meals=3, isHalal=True, isVegan=True)

    total_calories = 0
    total_carbo = 0
    total_protein = 0
    total_fat = 0

    for i in foodIndex_result:
        total_calories += food_data[i][DATA_EnergyAmount_kcal_INDEX]
        total_carbo += food_data[i][DATA_CarbohydrateAmount_g_INDEX]
        total_protein += food_data[i][DATA_ProteinAmount_g_INDEX]
        total_fat += food_data[i][DATA_TotalFatAmount_g_INDEX]

        print('%s' % food_data[i][DATA_FoodName_INDEX], end ='' )
        print(' (Index=%s)' % i, end ='' )
        print(' (Calories=%skcal)' % food_data[i][DATA_EnergyAmount_kcal_INDEX], end ='' )
        print(' (Carbo=%sg)' % food_data[i][DATA_CarbohydrateAmount_g_INDEX],end ='')
        print(' (Protein=%sg)' % food_data[i][DATA_ProteinAmount_g_INDEX],end ='')
        print(' (Fat=%sg)' % food_data[i][DATA_TotalFatAmount_g_INDEX],end ='')
        print(' (IsBreakfast=%s)' % food_data[i][DATA_IsBreakfast_INDEX], end='')
        print(' (IsMainDish=%s)' % food_data[i][DATA_IsMainDish_INDEX],end ='')
        print(' (IsFastFood=%s)' % food_data[i][DATA_IsFastFood_INDEX],end ='')
        print(' (IsVegan=%s)' % food_data[i][DATA_Vegan_INDEX],end ='')
        print(' (IsVegetarian=%s)' % food_data[i][DATA_Vegetarian_INDEX],end ='')
        print(' (IsHalal=%s)' % food_data[i][DATA_Halal_INDEX],end ='')
        print(' (ContainsBeef=%s)' % food_data[i][DATA_ContainsBeef_INDEX],end ='')
        print(' (IsAlcohol=%s)' % food_data[i][DATA_Alcohol_INDEX])
        
    if len(foodIndex_result) > 0:
        # Displaying food according to meals

        # Final sum and ratio of nutrients
        print("\nTotal nutrients:\n - Calories: {0:.2f} kcal\n - Carbo: {1:.2f}g ({4:.2%})\n - Protein: {2:.2f}g ({5:.2%})\n - Fat: {3:.2f}g ({6:.2%})".format(total_calories, total_carbo, total_protein, total_fat, total_carbo*4/total_calories, total_protein*4/total_calories, total_fat*9/total_calories))


if __name__ == '__main__':
    start = time.time()
    main()
    end = time.time()
    print("\nTotal time: {}".format(end - start))

