using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class FoodDetail
    {
        [JsonPropertyName("id")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [JsonPropertyName("group")]
        [Display(Name = "Group")]
        public string Group { get; set; }

        [JsonPropertyName("nutrient_protein_amount")]
        [Display(Name = "Proteins")]
        public double Proteins { get; set; }

        [JsonPropertyName("nutrient_protein_unit")]
        [Display(Name = "Protein Unit")]
        public string ProteinUnit { get; set; }

        [JsonPropertyName("nutrient_sugar_amount")]
        [Display(Name = "Sugar")]
        public double Sugar { get; set; }

        [JsonPropertyName("nutrient_sugar_unit")]
        [Display(Name = "Sugar Unit")]
        public string SugarUnit { get; set; }

        [JsonPropertyName("selfLink")]
        [Display(Name = "Link")]
        public Uri Link { get; set; }
    }
}
