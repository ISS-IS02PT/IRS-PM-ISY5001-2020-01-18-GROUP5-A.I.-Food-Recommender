using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class Food
    {
        [JsonPropertyName("FoodIndex")]
        [Display(Name = "Index")]
        public int Index { get; set; }

        [JsonPropertyName("FoodName")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [JsonPropertyName("FoodGroup")]
        [Display(Name = "Group")]
        public string Group { get; set; }

        [JsonPropertyName("CarbohydrateAmount_g")]
        [Display(Name = "Carbohydrates (g)")]
        public double Carbohydrates { get; set; }

        [JsonPropertyName("EnergyAmount_kcal")]
        [Display(Name = "Energy (kcal)")]
        public double Energy { get; set; }

        [JsonPropertyName("ProteinAmount_g")]
        [Display(Name = "Proteins (g)")]
        public double Proteins { get; set; }

        [JsonPropertyName("TotalFatAmount_g")]
        [Display(Name = "Total Fats (g)")]
        public double TotalFats { get; set; }

        [JsonPropertyName("IsVegan")]
        [Display(Name = "Vegan")]
        public bool IsVegan { get; set; }

        [JsonPropertyName("IsVegetarian")]
        [Display(Name = "Vegetarian")]
        public bool IsVegetarian { get; set; }

        [JsonPropertyName("IsHalal")]
        [Display(Name = "Halal")]
        public bool IsHalal { get; set; }

        [JsonPropertyName("ContainsBeef")]
        [Display(Name = "Contains Beef")]
        public bool ContainsBeef { get; set; }

        [JsonPropertyName("IsAlcohol")]
        [Display(Name = "Contains Alcohol")]
        public bool IsAlcohol { get; set; }

        [JsonPropertyName("FoodMealRanking")]
        [Display(Name = "Meal Type")]
        public string MealType { get; set; }

        [JsonPropertyName("food_keep_index")]
        public string Keep { get; set; }

        [JsonPropertyName("food_change_index")]
        public string Change { get; set; }
    }
}
