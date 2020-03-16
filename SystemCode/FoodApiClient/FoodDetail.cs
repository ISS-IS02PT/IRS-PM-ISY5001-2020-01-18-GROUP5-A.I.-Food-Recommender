using System;
using System.Text.Json.Serialization;

namespace FoodApiClient
{
    public class FoodDetail
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("nutrient_protein_amount")]
        public double ProteinAmount { get; set; }

        [JsonPropertyName("nutrient_protein_unit")]
        public string ProteinUnit { get; set; }

        [JsonPropertyName("nutrient_sugar_amount")]
        public double SugarAmount { get; set; }

        [JsonPropertyName("nutrient_sugar_unit")]
        public string SugarUnit { get; set; }

        [JsonPropertyName("selfLink")]
        public Uri Uri { get; set; }
    }
}