using System;
using System.Text.Json.Serialization;

namespace FoodApiClient
{
    public class ApiRoot
    {
        [JsonPropertyName("foods")]
        public Uri Uri { get; set; }
    }
}