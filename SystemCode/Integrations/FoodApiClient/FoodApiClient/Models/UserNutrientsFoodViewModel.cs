﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class UserNutrientsFoodViewModel
    {
        public UserProfile UserProfile { get; set; }
        public Nutrients Nutrients { get; set; }
        public FoodOptions Options { get; set; }
        public List<Food> FoodList { get; set; }

        [Display(Name = "Use Calculated Nutrients")]
        public bool? UseCalcNutrients { get; set; }
    }
}