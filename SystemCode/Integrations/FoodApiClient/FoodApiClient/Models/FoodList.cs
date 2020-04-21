using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class FoodList
    {
        [JsonPropertyName("id")]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [JsonPropertyName("selfLink")]
        [Display(Name = "Link")]
        public Uri Link { get; set; }
    }
}
