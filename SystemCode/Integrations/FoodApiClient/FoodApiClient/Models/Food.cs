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
        [JsonPropertyName("FoodName")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [JsonPropertyName("FoodGroup")]
        [Display(Name = "Group")]
        public string Group { get; set; }

        [JsonPropertyName("EnergyAmount_kcal")]
        [Display(Name = "Energy (kcal)")]
        public double Energy { get; set; }

        [JsonPropertyName("ProteinAmount_g")]
        [Display(Name = "Proteins (g)")]
        public double Proteins { get; set; }

        [JsonPropertyName("CarbohydrateAmount_g")]
        [Display(Name = "Carbohydrates (g)")]
        public double Carbs { get; set; }

        [JsonPropertyName("TotalFatAmount_g")]
        [Display(Name = "Fats (g)")]
        public double Fats { get; set; }
    }
}
