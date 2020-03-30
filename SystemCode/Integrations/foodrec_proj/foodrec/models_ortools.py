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


food_data = None
#csv_file = 'Dataset/FoodDatabase.csv'
csv_file = 'Dataset/FoodDatabase_HC.csv'
#NUM_FOOD = 1000
NUM_FOOD = 3701


def readFoodData(csv_file):
    # Reading only the 1st 'NUM_FOOD' rows for now, for the selected columns
    df = pd.read_csv(csv_file)[['FoodName','FoodGroup','CarbohydrateAmount_g','EnergyAmount_kcal','ProteinAmount_g','TotalFatAmount_g']]
    
    # Update the index here for the data arrays to point to different nutrients
    global DATA_FoodName_INDEX, DATA_FoodGroup_INDEX, DATA_CarbohydrateAmount_g_INDEX, \
        DATA_EnergyAmount_kcal_INDEX, DATA_ProteinAmount_g_INDEX, DATA_TotalFatAmount_g_INDEX
    DATA_FoodName_INDEX = 0
    DATA_FoodGroup_INDEX = 1
    DATA_CarbohydrateAmount_g_INDEX = 2
    DATA_EnergyAmount_kcal_INDEX = 3
    DATA_ProteinAmount_g_INDEX = 4
    DATA_TotalFatAmount_g_INDEX = 5

    global food_data
    food_data = df.head(NUM_FOOD).to_numpy()


# This optimizer only takes input parameter as 'EnergyAmount_kcal'
def optimizer1(EnergyAmount_kcal):
    # Create the mip solver with the CBC backend
    solver = pywraplp.Solver('optimizer1',
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

    # Additional contrainst -> within the 90% - 110% of recommended calories
    constraint = solver.Constraint(EnergyAmount_kcal * 0.9, EnergyAmount_kcal * 1.1)
    for i in range(0, len(food_data)):
        constraint.SetCoefficient(food[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])

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

# Harry: This optimizer takes input parameter as 'EnergyAmount_kcal' and 'BodyWeight_kg'
def optimizer_HC_2(EnergyAmount_kcal,BodyWeight_kg):
    # Create the mip solver with the CBC backend
    solver = pywraplp.Solver('optimizer_HC_2',
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

    # Additional constraint0 -> within the 90% - 110% of recommended calories
    constraint0 = solver.Constraint(EnergyAmount_kcal * 0.9, EnergyAmount_kcal * 1.1)
    # Additional constraint1 -> protein consumption must be more than (0.8 * body weight in kg) grams
    constraint1 = solver.Constraint(BodyWeight_kg * 0.8, solver.infinity())   
    for i in range(0, len(food_data)):
        constraint0.SetCoefficient(food[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])
        constraint1.SetCoefficient(food[i], food_data[i][DATA_ProteinAmount_g_INDEX])

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


# Harry: This optimizer takes input parameter as 'EnergyAmount_kcal' and 'BodyWeight_kg', this is for Keto Diet
def optimizer_HC_3(EnergyAmount_kcal,BodyWeight_kg):
    # Create the mip solver with the CBC backend
    solver = pywraplp.Solver('optimizer_HC_3',
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

    # Additional constraint0 -> within the 90% - 110% of recommended calories
    constraint0 = solver.Constraint(EnergyAmount_kcal * 0.9, EnergyAmount_kcal * 1.1)
    # Additional constraint1 -> protein consumption must be more than (0.8 * body weight in kg) grams
    constraint1 = solver.Constraint(BodyWeight_kg * 0.8, solver.infinity())   
    # Additional constraint2 -> carb consumption must be less than 50 grams
    constraint2 = solver.Constraint(-solver.infinity(),50)
    # Additional constraint3 -> calorie from fat must be about 75% of total calorie, within 90%-110% of this goal
    constraint3 = solver.Constraint(EnergyAmount_kcal * 0.75 * 0.9 / 9, EnergyAmount_kcal * 0.75 * 1.1 / 9)
    # Additional constraint4 -> calorie from protein must be about 20% of total calorie, within 90%-110% of this goal
    constraint4 = solver.Constraint(EnergyAmount_kcal * 0.20 * 0.9 / 4, EnergyAmount_kcal * 0.20 * 1.1 / 4)
    
    
    for i in range(0, len(food_data)):
        constraint0.SetCoefficient(food[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])
        constraint1.SetCoefficient(food[i], food_data[i][DATA_ProteinAmount_g_INDEX])
        constraint2.SetCoefficient(food[i], food_data[i][DATA_CarbohydrateAmount_g_INDEX])
        constraint3.SetCoefficient(food[i], food_data[i][DATA_TotalFatAmount_g_INDEX])
        constraint4.SetCoefficient(food[i], food_data[i][DATA_ProteinAmount_g_INDEX])

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



# Generic Optimizer for various nutrients requirements ( Input parameters from pyke)
def optimizer_Dennis_1(EnergyAmount_kcal,CarbohydrateAmount_g,ProteinAmount_g,TotalFatAmount_g ):
    # Create the mip solver with the CBC backend
    solver = pywraplp.Solver('optimizer_Dennis_1',
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
    constraint1 = solver.Constraint(CarbohydrateAmount_g * 0.9 , CarbohydrateAmount_g * 1.1  )
    constraint2 = solver.Constraint(ProteinAmount_g * 0.9 , ProteinAmount_g * 1.1 )
    constraint3 = solver.Constraint(TotalFatAmount_g * 0.9 , TotalFatAmount_g * 1.1 )
    for i in range(0, len(food_data)):
        constraint0.SetCoefficient(food[i], food_data[i][DATA_EnergyAmount_kcal_INDEX])
        constraint1.SetCoefficient(food[i], food_data[i][DATA_CarbohydrateAmount_g_INDEX])
        constraint2.SetCoefficient(food[i], food_data[i][DATA_ProteinAmount_g_INDEX])
        constraint3.SetCoefficient(food[i], food_data[i][DATA_TotalFatAmount_g_INDEX])

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

def run_optimizer_Dennis(EnergyAmount_kcal,CarbohydrateAmount_g,ProteinAmount_g,TotalFatAmount_g):
    return optimizer_Dennis_1(EnergyAmount_kcal,CarbohydrateAmount_g,ProteinAmount_g,TotalFatAmount_g )

def run_optimizer(EnergyAmount_kcal):
    return optimizer1(EnergyAmount_kcal)

def run_optimizer_HC_2(EnergyAmount_kcal, BodyWeight_kg):
    return optimizer_HC_2(EnergyAmount_kcal, BodyWeight_kg)

def run_optimizer_HC_3(EnergyAmount_kcal, BodyWeight_kg):
    return optimizer_HC_3(EnergyAmount_kcal, BodyWeight_kg)


# For quick testing without Django
def main():
    readFoodData(csv_file)
    #foodIndex_result = run_optimizer(EnergyAmount_kcal=2500)
    #foodIndex_result = run_optimizer_HC_2(EnergyAmount_kcal=2500, BodyWeight_kg=70)
    foodIndex_result = run_optimizer_HC_3(EnergyAmount_kcal=2500, BodyWeight_kg=70)
    #foodIndex_result = run_optimizer_Dennis(EnergyAmount_kcal=2000, CarbohydrateAmount_g =150,ProteinAmount_g = 150 , TotalFatAmount_g = 50 )
    for i in foodIndex_result:
        print('%s' % food_data[i][DATA_FoodName_INDEX], end ='' )
        print(' (Calories=%skcal)' % food_data[i][DATA_EnergyAmount_kcal_INDEX], end ='' )
        print(' (Carbo=%sg)' % food_data[i][DATA_CarbohydrateAmount_g_INDEX],end ='')
        print(' (Protein=%sg)' % food_data[i][DATA_ProteinAmount_g_INDEX],end ='')
        print(' (Fat=%sg)' % food_data[i][DATA_TotalFatAmount_g_INDEX])

if __name__ == '__main__':
    main()