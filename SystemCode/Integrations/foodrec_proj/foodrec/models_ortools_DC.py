from __future__ import print_function
from ortools.linear_solver import pywraplp

import pandas as pd

# Constants and Global variables
DATA_FoodName_INDEX = -1
DATA_FoodGroup_INDEX = -1
DATA_CarbohydrateAmount_g_INDEX = -1
DATA_EnergyAmount_kcal_INDEX = -1
DATA_ProteinAmount_g_INDEX = -1
DATA_TotalFatAmount_g_INDEX = -1
DATA_IsMainDish_INDEX = -1
DATA_IsBreakfast_INDEX = -1
DATA_IsFastFood_INDEX = -1
DATA_IsBeverages_INDEX = -1
DATA_IsOthers_INDEX = -1
DATA_Vegan_INDEX = -1
DATA_Vegetarian_INDEX = -1
DATA_Halal_INDEX = -1
DATA_ContainsBeef_INDEX = -1
DATA_Alcohol_INDEX = -1

food_data = None
csv_file = 'Dataset/foodDataBase_PleaseUseThis_DC2.csv'
NUM_FOOD = 3920


def readFoodData(csv_file):
    # Reading only the 1st 'NUM_FOOD' rows for now, for the selected columns
    df = pd.read_csv(csv_file)[['FoodName','FoodGroup','CarbohydrateAmount_g','EnergyAmount_kcal','ProteinAmount_g','TotalFatAmount_g' ,\
        'IsMainDish','IsFastFood', 'IsBreakfast','IsBeverages','IsOthers','Vegan','Vegetarian','Halal','ContainsBeef','Alcohol']]
    
    # Update the index here for the data arrays to point to different nutrients
    global DATA_FoodName_INDEX, DATA_FoodGroup_INDEX, DATA_CarbohydrateAmount_g_INDEX, \
        DATA_EnergyAmount_kcal_INDEX, DATA_ProteinAmount_g_INDEX, DATA_TotalFatAmount_g_INDEX, \
        DATA_IsMainDish_INDEX,DATA_IsFastFood_INDEX, DATA_IsBreakfast_INDEX,DATA_IsBeverages_INDEX, DATA_IsOthers_INDEX,\
        DATA_Vegan_INDEX, DATA_Vegetarian_INDEX, DATA_Halal_INDEX,DATA_ContainsBeef_INDEX,DATA_Alcohol_INDEX 
      
    DATA_FoodName_INDEX = 0
    DATA_FoodGroup_INDEX = 1
    DATA_CarbohydrateAmount_g_INDEX = 2
    DATA_EnergyAmount_kcal_INDEX = 3
    DATA_ProteinAmount_g_INDEX = 4
    DATA_TotalFatAmount_g_INDEX = 5
    DATA_IsMainDish_INDEX = 6
    DATA_IsBreakfast_INDEX = 7
    DATA_IsFastFood_INDEX = 8
    DATA_IsBeverages_INDEX = 9
    DATA_IsOthers_INDEX = 10
    DATA_Vegan_INDEX = 11
    DATA_Vegetarian_INDEX = 12
    DATA_Halal_INDEX = 13
    DATA_ContainsBeef_INDEX = 14
    DATA_Alcohol_INDEX = 15

    global food_data
    food_data = df.head(NUM_FOOD).to_numpy()

# Generic Optimizer for various nutrients requirements ( Input parameters from pyke)
def optimizer_DC_1(EnergyAmount_kcal,CarbohydrateAmount_g,ProteinAmount_g,TotalFatAmount_g,\
    IsMainDish,IsFastFood,IsBreakfast,IsBeverages,IsOthers,Vegan,Vegetarian,Halal,ContainsBeef,Alcohol ):
    # Create the mip solver with the CBC backend
    solver = pywraplp.Solver('optimizer_DC_1',
                             pywraplp.Solver.CBC_MIXED_INTEGER_PROGRAMMING)

    # Declare the objective function
    objective = solver.Objective()

    # Declare an array to hold the variable value whether the food is selected, which is 0 or 1 for each food
    food = [[]] * len(food_data)
    # The coeficient for the objective function is the amount of calories for the corresponding food
    for i in range(0, len(food_data)):
        food[i] = solver.IntVar(0.0, 1.0, food_data[i][DATA_FoodName_INDEX])
        objective.SetCoefficient(food[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])

    # Minimize the calories
    objective.SetMinimization()

    # Constraints for various nutrients -> within the 90% - 110% of recommended 
    constraint0 = solver.Constraint(EnergyAmount_kcal * 0.9, EnergyAmount_kcal * 1.1)
    constraint1 = solver.Constraint(CarbohydrateAmount_g * 1 , CarbohydrateAmount_g * 1000  )
    constraint2 = solver.Constraint(ProteinAmount_g * 1 , ProteinAmount_g * 1000 )
    constraint3 = solver.Constraint(TotalFatAmount_g * 1 , TotalFatAmount_g * 1000 )
    # Constraints to choose how many dishes per category for the day ( Main Dish , Breakfast , Fast Food and Beverages)
    constraint4 = solver.Constraint(IsMainDish *1 ,IsMainDish *1  )
    constraint5 = solver.Constraint(IsFastFood *1 ,IsFastFood *1  )
    constraint6 = solver.Constraint(IsBreakfast *1 , IsBreakfast *1 )
    constraint7 = solver.Constraint(IsBeverages*1 , IsBeverages *1 )
    constraint8 = solver.Constraint(IsOthers *1 , IsOthers *1 )
    # Constraints to opt for diet preferences
    constraint9 = solver.Constraint(Vegan *1 , Vegan *100 )
    constraint10 = solver.Constraint(Vegetarian *1, Vegetarian *100 )
    constraint11 = solver.Constraint(Halal *1 , Halal *100 )
    constraint12 = solver.Constraint(ContainsBeef *1 , ContainsBeef *100 )
    constraint13 = solver.Constraint(Alcohol *1 , Alcohol *100 )
 
    for i in range(0, len(food_data)):
        constraint0.SetCoefficient(food[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])
        constraint1.SetCoefficient(food[i], food_data[i][DATA_CarbohydrateAmount_g_INDEX])
        constraint2.SetCoefficient(food[i], food_data[i][DATA_ProteinAmount_g_INDEX])
        constraint3.SetCoefficient(food[i], food_data[i][DATA_TotalFatAmount_g_INDEX])
        constraint4.SetCoefficient(food[i], food_data[i][DATA_IsMainDish_INDEX])
        constraint5.SetCoefficient(food[i], food_data[i][DATA_IsFastFood_INDEX])
        constraint6.SetCoefficient(food[i], food_data[i][DATA_IsBreakfast_INDEX])
        constraint7.SetCoefficient(food[i], food_data[i][DATA_IsBeverages_INDEX])
        constraint8.SetCoefficient(food[i], food_data[i][DATA_IsOthers_INDEX])
        constraint9.SetCoefficient(food[i], food_data[i][DATA_Vegan_INDEX])
        constraint10.SetCoefficient(food[i], food_data[i][DATA_Vegetarian_INDEX])
        constraint11.SetCoefficient(food[i], food_data[i][DATA_Halal_INDEX])
        constraint12.SetCoefficient(food[i], food_data[i][DATA_ContainsBeef_INDEX])
        constraint13.SetCoefficient(food[i], food_data[i][DATA_Alcohol_INDEX])

    # Solve!
    status = solver.Solve()

    foodIndex_result = []

    if status == solver.OPTIMAL:
        print('An optimal solution was found.')
        print('Objective value =', solver.Objective().Value())
        for i in range(0, len(food_data)):
            if food[i].solution_value() > 0:
                foodIndex_result.append(i)
                # print('%s = %f' % (data[i][0], food[i].solution_value()))

    else:  # No optimal solution was found.
        if status == solver.FEASIBLE:
            print('A potentially suboptimal solution was found.')
        else:
            print('The solver could not solve the problem.')

    return foodIndex_result

def run_optimizer_DC_1(EnergyAmount_kcal,CarbohydrateAmount_g,ProteinAmount_g,TotalFatAmount_g,\
    IsMainDish,IsFastFood,IsBreakfast,IsBeverages,IsOthers,Vegan,Vegetarian,Halal,ContainsBeef,Alcohol):
    return optimizer_DC_1(EnergyAmount_kcal,CarbohydrateAmount_g,ProteinAmount_g,TotalFatAmount_g,\
        IsMainDish,IsFastFood,IsBreakfast,IsBeverages,IsOthers,Vegan,Vegetarian,Halal,ContainsBeef,Alcohol )

# For quick testing without Django
def main():
    readFoodData(csv_file)
    foodIndex_result = run_optimizer_DC_1(EnergyAmount_kcal = 2500,CarbohydrateAmount_g = 50,ProteinAmount_g =50,TotalFatAmount_g =50,\
        IsMainDish =5,IsFastFood =0, IsBreakfast =1,IsBeverages =10, IsOthers =0, Vegan =1, Vegetarian=1, Halal=1,ContainsBeef=0, Alcohol=0)
    for i in foodIndex_result:
        print('%s' % food_data[i][DATA_FoodName_INDEX], end ='' )
        print(' (Calories=%skcal)' % food_data[i][DATA_EnergyAmount_kcal_INDEX], end ='' )
        #print(' (Carbo=%sg)' % food_data[i][DATA_CarbohydrateAmount_g_INDEX],end ='')
        #print(' (Protein=%sg)' % food_data[i][DATA_ProteinAmount_g_INDEX],end ='')
        #print(' (Fat=%sg)' % food_data[i][DATA_TotalFatAmount_g_INDEX],end ='')
        print(' (IsMainDish=%s)' % food_data[i][DATA_IsMainDish_INDEX],end ='')
        print(' (IsFastFood=%s)' % food_data[i][DATA_IsFastFood_INDEX],end ='')
        print(' (IsBeverages=%s)' % food_data[i][DATA_IsBeverages_INDEX],end ='')
        print(' (IsBreakfast=%s)' % food_data[i][DATA_IsBreakfast_INDEX],end ='')
        #print(' (IsOthers=%s)' % food_data[i][DATA_IsOthers_INDEX],end ='')
        print(' (Vegan=%s)' % food_data[i][DATA_Vegan_INDEX],end ='')
        print(' (Vegetarian=%s)' % food_data[i][DATA_Vegetarian_INDEX],end ='')
        print(' (Halal=%s)' % food_data[i][DATA_Halal_INDEX],end ='')
        print(' (ContainsBeef=%s)' % food_data[i][DATA_ContainsBeef_INDEX],end ='')
        print(' (Alcohol=%s)' % food_data[i][DATA_Alcohol_INDEX])

if __name__ == '__main__':
    main()