import datetime

from pyke import knowledge_engine
engine = None

def pyke_load_engine():
    global engine
    engine = knowledge_engine.engine((__file__, '.rules'))

def pyke_calculate_bmr(gender, weight, height, age):
    global engine

    engine.reset()
    engine.activate('bc_rules')

    try:
        vals, plans = engine.prove_1_goal('bc_rules.calculate_bmr($bmr, $gender, $weight, $height, $age)', gender=gender, weight=weight, height=height, age=age)
        return vals['bmr']
    except knowledge_engine.CanNotProve:
        return None

def pyke_calculate_EnergyAmount_kcal(bmr, activity):
    global engine

    engine.reset()
    engine.activate('bc_rules')

    try:
        vals, plans = engine.prove_1_goal('bc_rules.calculate_EnergyAmount_kcal($energy, $bmr, $activity)', bmr=bmr, activity=activity)
        return vals['energy']
    except knowledge_engine.CanNotProve:
        return None

def pyke_calculate_Nutrients(energy, diet_type):
    global engine

    engine.reset()
    engine.activate('bc_rules')

    try:
        vals, plans = engine.prove_1_goal('bc_rules.calculate_Nutrients($carbo_g, $fat_g, $protein_g, $energy, $diet_type)', energy=energy, diet_type=diet_type)
        return vals['carbo_g'], vals['fat_g'], vals['protein_g']
    except knowledge_engine.CanNotProve:
        return None   

if __name__ == "__main__":
    pyke_load_engine()
    # print(pyke_calculate_bmr('male',85,182,32))
    a,b,c = pyke_calculate_Nutrients(1832.5, 'standard')
    print(a)
    