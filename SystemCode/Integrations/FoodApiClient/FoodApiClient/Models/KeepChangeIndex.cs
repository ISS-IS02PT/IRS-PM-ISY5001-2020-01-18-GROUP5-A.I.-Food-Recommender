using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class KeepChangeIndex
    {
        [JsonPropertyName("food_keep_index")]
        public List<int> Keep { get; set; }

        [JsonPropertyName("food_change_index")]
        public List<int> Change { get; set; }
    }
}
