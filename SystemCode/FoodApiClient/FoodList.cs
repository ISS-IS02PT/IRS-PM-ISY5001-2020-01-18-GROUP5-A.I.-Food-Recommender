using System;
using System.Text.Json.Serialization;

namespace FoodApiClient
{
    public class FoodList
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("selfLink")]
        public Uri Uri { get; set; }
    }
}