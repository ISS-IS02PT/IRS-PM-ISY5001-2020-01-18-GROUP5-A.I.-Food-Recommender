using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class Nutrients
    {
        [JsonPropertyName("EnergyAmount_kcal")]
        [Display(Name = "Energy (kcal)"), Required, Range(0.0, double.MaxValue)]
        public double Energy { get; set; }

        [JsonPropertyName("ProteinAmount_g")]
        [Display(Name = "Proteins (g)"), Required, Range(0.0, double.MaxValue)]
        public double Proteins { get; set; }

        [JsonPropertyName("CarbohydrateAmount_g")]
        [Display(Name = "Carbohydrates (g)"), Required, Range(0.0, double.MaxValue)]
        public double Carbohydrates { get; set; }

        [JsonPropertyName("TotalFatAmount_g")]
        [Display(Name = "Total Fats (g)"), Required, Range(0.0, double.MaxValue)]
        public double TotalFats { get; set; }

        [JsonPropertyName("diet")]
        [JsonConverter(typeof(Converters.DietConverter))]
        [Display(Name = "Diet"), Required]
        public CustomTypes.Diet Diet { get; set; }
    }
}
