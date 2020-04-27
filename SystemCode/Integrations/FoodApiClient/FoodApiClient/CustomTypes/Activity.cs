using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApiClient.CustomTypes
{
    public enum Activity
    {
        Sedentary,
        [Display(Name = "Lightly Active")]
        Lightly,
        [Display(Name = "Moderately Active")]
        Moderately_Active,
        [Display(Name = "Very Active")]
        Very_Active
    }
}
