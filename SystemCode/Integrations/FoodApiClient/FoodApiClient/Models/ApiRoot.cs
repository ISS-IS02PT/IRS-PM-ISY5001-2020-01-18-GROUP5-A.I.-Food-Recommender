using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class ApiRoot
    {
        [JsonPropertyName("calculate-nutrient-needs-from-profile")]
        [Display(Name = "Calculate Nutrients From User Profle")]
        public Uri CalcNutrientsLink { get; set; }

        [JsonPropertyName("food-recommendation-from-nutrient-needs")]
        [Display(Name = "Food Recommendation From Nutrients")]
        public Uri NutrientsToFoodRecLink { get; set; }

        [JsonPropertyName("food-recommendation-from-profile")]
        [Display(Name = "Food Recommendation From User Profile")]
        public Uri UserToFoodRecLink { get; set; }
    }
}
