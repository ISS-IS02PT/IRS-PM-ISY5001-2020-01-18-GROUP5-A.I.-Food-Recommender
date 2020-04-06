# bc_rules_bc.py

from pyke import contexts, pattern, bc_rule

pyke_version = '1.1.1'
compiler_version = 1

def rule_calculate_bmr_male(rule, arg_patterns, arg_context):
  engine = rule.rule_base.engine
  patterns = rule.goal_arg_patterns()
  if len(arg_patterns) == len(patterns):
    context = contexts.bc_context(rule)
    try:
      if all(map(lambda pat, arg:
                   pat.match_pattern(context, context,
                                     arg, arg_context),
                 patterns,
                 arg_patterns)):
        rule.rule_base.num_bc_rules_matched += 1
        bmr = calculate_bmr_male(context.lookup_data('weight'), context.lookup_data('height'), context.lookup_data('age'))
        mark2 = context.mark(True)
        if rule.pattern(0).match_data(context, context,
                bmr):
          context.end_save_all_undo()
          rule.rule_base.num_bc_rule_successes += 1
          yield
        else: context.end_save_all_undo()
        context.undo_to_mark(mark2)
        rule.rule_base.num_bc_rule_failures += 1
    finally:
      context.done()

def rule_calculate_bmr_female(rule, arg_patterns, arg_context):
  engine = rule.rule_base.engine
  patterns = rule.goal_arg_patterns()
  if len(arg_patterns) == len(patterns):
    context = contexts.bc_context(rule)
    try:
      if all(map(lambda pat, arg:
                   pat.match_pattern(context, context,
                                     arg, arg_context),
                 patterns,
                 arg_patterns)):
        rule.rule_base.num_bc_rules_matched += 1
        bmr = calculate_bmr_female(context.lookup_data('weight'), context.lookup_data('height'), context.lookup_data('age'))
        mark2 = context.mark(True)
        if rule.pattern(0).match_data(context, context,
                bmr):
          context.end_save_all_undo()
          rule.rule_base.num_bc_rule_successes += 1
          yield
        else: context.end_save_all_undo()
        context.undo_to_mark(mark2)
        rule.rule_base.num_bc_rule_failures += 1
    finally:
      context.done()

def rule_calculate_EnergyAmount_kcal_sedentary(rule, arg_patterns, arg_context):
  engine = rule.rule_base.engine
  patterns = rule.goal_arg_patterns()
  if len(arg_patterns) == len(patterns):
    context = contexts.bc_context(rule)
    try:
      if all(map(lambda pat, arg:
                   pat.match_pattern(context, context,
                                     arg, arg_context),
                 patterns,
                 arg_patterns)):
        rule.rule_base.num_bc_rules_matched += 1
        energy = calculate_EnergyAmount_kcal_sedentary(context.lookup_data('bmr'))
        mark2 = context.mark(True)
        if rule.pattern(0).match_data(context, context,
                energy):
          context.end_save_all_undo()
          rule.rule_base.num_bc_rule_successes += 1
          yield
        else: context.end_save_all_undo()
        context.undo_to_mark(mark2)
        rule.rule_base.num_bc_rule_failures += 1
    finally:
      context.done()

def rule_calculate_EnergyAmount_kcal_lightly_active(rule, arg_patterns, arg_context):
  engine = rule.rule_base.engine
  patterns = rule.goal_arg_patterns()
  if len(arg_patterns) == len(patterns):
    context = contexts.bc_context(rule)
    try:
      if all(map(lambda pat, arg:
                   pat.match_pattern(context, context,
                                     arg, arg_context),
                 patterns,
                 arg_patterns)):
        rule.rule_base.num_bc_rules_matched += 1
        energy = calculate_EnergyAmount_kcal_lightly_active(context.lookup_data('bmr'))
        mark2 = context.mark(True)
        if rule.pattern(0).match_data(context, context,
                energy):
          context.end_save_all_undo()
          rule.rule_base.num_bc_rule_successes += 1
          yield
        else: context.end_save_all_undo()
        context.undo_to_mark(mark2)
        rule.rule_base.num_bc_rule_failures += 1
    finally:
      context.done()

def rule_calculate_EnergyAmount_kcal_moderately_active(rule, arg_patterns, arg_context):
  engine = rule.rule_base.engine
  patterns = rule.goal_arg_patterns()
  if len(arg_patterns) == len(patterns):
    context = contexts.bc_context(rule)
    try:
      if all(map(lambda pat, arg:
                   pat.match_pattern(context, context,
                                     arg, arg_context),
                 patterns,
                 arg_patterns)):
        rule.rule_base.num_bc_rules_matched += 1
        energy = calculate_EnergyAmount_kcal_moderately_active(context.lookup_data('bmr'))
        mark2 = context.mark(True)
        if rule.pattern(0).match_data(context, context,
                energy):
          context.end_save_all_undo()
          rule.rule_base.num_bc_rule_successes += 1
          yield
        else: context.end_save_all_undo()
        context.undo_to_mark(mark2)
        rule.rule_base.num_bc_rule_failures += 1
    finally:
      context.done()

def rule_calculate_EnergyAmount_kcal_very_active(rule, arg_patterns, arg_context):
  engine = rule.rule_base.engine
  patterns = rule.goal_arg_patterns()
  if len(arg_patterns) == len(patterns):
    context = contexts.bc_context(rule)
    try:
      if all(map(lambda pat, arg:
                   pat.match_pattern(context, context,
                                     arg, arg_context),
                 patterns,
                 arg_patterns)):
        rule.rule_base.num_bc_rules_matched += 1
        energy = calculate_EnergyAmount_kcal_very_active(context.lookup_data('bmr'))
        mark2 = context.mark(True)
        if rule.pattern(0).match_data(context, context,
                energy):
          context.end_save_all_undo()
          rule.rule_base.num_bc_rule_successes += 1
          yield
        else: context.end_save_all_undo()
        context.undo_to_mark(mark2)
        rule.rule_base.num_bc_rule_failures += 1
    finally:
      context.done()

def populate(engine):
  This_rule_base = engine.get_create('bc_rules')
  
  bc_rule.bc_rule('rule_calculate_bmr_male', This_rule_base, 'calculate_bmr',
                  rule_calculate_bmr_male, None,
                  (contexts.variable('bmr'),
                   pattern.pattern_literal('male'),
                   contexts.variable('weight'),
                   contexts.variable('height'),
                   contexts.variable('age'),),
                  (),
                  (contexts.variable('bmr'),))
  
  bc_rule.bc_rule('rule_calculate_bmr_female', This_rule_base, 'calculate_bmr',
                  rule_calculate_bmr_female, None,
                  (contexts.variable('bmr'),
                   pattern.pattern_literal('female'),
                   contexts.variable('weight'),
                   contexts.variable('height'),
                   contexts.variable('age'),),
                  (),
                  (contexts.variable('bmr'),))
  
  bc_rule.bc_rule('rule_calculate_EnergyAmount_kcal_sedentary', This_rule_base, 'calculate_EnergyAmount_kcal',
                  rule_calculate_EnergyAmount_kcal_sedentary, None,
                  (contexts.variable('energy'),
                   contexts.variable('bmr'),
                   pattern.pattern_literal('sedentary'),),
                  (),
                  (contexts.variable('energy'),))
  
  bc_rule.bc_rule('rule_calculate_EnergyAmount_kcal_lightly_active', This_rule_base, 'calculate_EnergyAmount_kcal',
                  rule_calculate_EnergyAmount_kcal_lightly_active, None,
                  (contexts.variable('energy'),
                   contexts.variable('bmr'),
                   pattern.pattern_literal('lightly_active'),),
                  (),
                  (contexts.variable('energy'),))
  
  bc_rule.bc_rule('rule_calculate_EnergyAmount_kcal_moderately_active', This_rule_base, 'calculate_EnergyAmount_kcal',
                  rule_calculate_EnergyAmount_kcal_moderately_active, None,
                  (contexts.variable('energy'),
                   contexts.variable('bmr'),
                   pattern.pattern_literal('moderately_active'),),
                  (),
                  (contexts.variable('energy'),))
  
  bc_rule.bc_rule('rule_calculate_EnergyAmount_kcal_very_active', This_rule_base, 'calculate_EnergyAmount_kcal',
                  rule_calculate_EnergyAmount_kcal_very_active, None,
                  (contexts.variable('energy'),
                   contexts.variable('bmr'),
                   pattern.pattern_literal('very_active'),),
                  (),
                  (contexts.variable('energy'),))

def calculate_bmr_male(weight, height, age):
    return 10 * weight + 6.25 * height - 5 * age + 5
def calculate_bmr_female(weight, height, age):
    return 10 * weight + 6.25 * height - 5 * age - 161
def calculate_EnergyAmount_kcal_sedentary(bmr):
    return bmr * 1.2
def calculate_EnergyAmount_kcal_moderately_active(bmr):
    return bmr * 1.375
def calculate_EnergyAmount_kcal_very_active(bmr):
    return bmr * 1.55
def calculate_EnergyAmount_kcal_very_active(bmr):
    return bmr * 1.725

Krb_filename = '../models_pyke/bc_rules.krb'
Krb_lineno_map = (
    ((14, 18), (5, 5)),
    ((20, 20), (7, 7)),
    ((23, 23), (8, 8)),
    ((39, 43), (11, 11)),
    ((45, 45), (13, 13)),
    ((48, 48), (14, 14)),
    ((64, 68), (20, 20)),
    ((70, 70), (22, 22)),
    ((73, 73), (23, 23)),
    ((89, 93), (26, 26)),
    ((95, 95), (28, 28)),
    ((98, 98), (29, 29)),
    ((114, 118), (32, 32)),
    ((120, 120), (34, 34)),
    ((123, 123), (35, 35)),
    ((139, 143), (38, 38)),
    ((145, 145), (40, 40)),
    ((148, 148), (41, 41)),
)
