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
        [Display(Name = "Energy (kcal)"), Required]
        public double Energy { get; set; }

        [JsonPropertyName("ProteinAmount_g")]
        [Display(Name = "Proteins (g)"), Required]
        public double Proteins { get; set; }

        [JsonPropertyName("CarbohydrateAmount_g")]
        [Display(Name = "Carbohydrates (g)"), Required]
        public double Carbs { get; set; }

        [JsonPropertyName("TotalFatAmount_g")]
        [Display(Name = "Fats (g)"), Required]
        public double Fats { get; set; }

        [JsonPropertyName("diet")]
        [Display(Name = "Diet Type"), Required]
        [RegularExpression(@"\b[nN][oO][nN][eE]\b|\b[nN][oO][rR][mM]\b|\b[nN][oO][rR][mM][aA][lL]\b|" +
                           @"\b[aA][nN][yY]\b|\b[aA][nN][yY][tT][hH][iI][nN][gG]\b|" +
                           @"\b[kK][eE][tT][oO]\b|\b[kK][eE][tT][oO][gG][eE][nN][iI][cC]\b")]
        public string Diet { get; set; }
    }
}
