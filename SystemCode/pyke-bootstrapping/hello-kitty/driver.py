from pyke import knowledge_engine, krb_traceback, goal

# Compile and load .krb files in same directory (recursively).
engine = knowledge_engine.engine(__file__)

def print_fc_facts():
    '''
        This function runs the forward-chaining (rules.krb).
    '''
    engine.reset()
    engine.activate("fc_rules")  # Runs all applicable forward-chaining rules.

    # Print FC facts
    engine.get_kb("facts").dump_specific_facts()


def is_animal(animal):
    engine.reset()
    engine.activate("fc_rules")  # Runs all applicable forward-chaining rules.

    try:
        vars, plan = engine.prove_1_goal("facts.Animal($animal)",animal=animal)
        print("{} is an animal".format(animal))
    except:
        print("{} is not an animal".format(animal))


def who_loves_hellokitty():
    engine.reset()
    engine.activate("fc_rules")  # Runs all applicable forward-chaining rules.

    try:
        vars, plan = engine.prove_1_goal("facts.Loves($person,'HelloKitty')")
        print("{} loves HelloKitty".format(vars['person']))
    except:
        print("Couldn't find such a person")


def who_kills_hellokitty():
    engine.reset()
    engine.activate("fc_rules")  # Runs all applicable forward-chaining rules.

    # Find a goal
    try:
        vars, plan = engine.prove_1_goal("facts.Kills($person,'HelloKitty', True)")
        print("{} kills HelloKitty".format(vars['person']))
    except:
        print("Couldn't find such a person")
    

def main():
    # Is HelloKitty an animal?
    is_animal('HelloKitty')

    # Is Sam an animal?
    is_animal('Sam')

    # Who loves HelloKitty?
    who_loves_hellokitty()
    
    # Who kills HelloKitty?
    who_kills_hellokitty()


main()