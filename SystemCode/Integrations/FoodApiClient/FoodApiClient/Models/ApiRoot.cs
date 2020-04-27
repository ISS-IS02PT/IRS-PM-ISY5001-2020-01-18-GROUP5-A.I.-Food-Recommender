using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    internal class ApiRoot
    {
        [JsonPropertyName("calculate-nutrient-needs-from-profile")]
        [Display(Name = "Calculate Nutrients From User Profle")]
        public Uri CalcNutrientsLink { get; set; }

        [JsonPropertyName("food-recommendation-from-nutrient-needs")]
        [Display(Name = "Food Recommendation From Nutrients")]
        public Uri NutrientsToFoodRecLink { get; set; }

        /*[JsonPropertyName("food-recommendation-from-profile")]
        [Display(Name = "Food Recommendation From User Profile")]
        public Uri UserToFoodRecLink { get; set; }*/

        public ApiRoot() { }

        public ApiRoot(ApiRoot apiRoot)
        {
            if (apiRoot != null)
            {
                if (apiRoot.CalcNutrientsLink != null)
                {
                    CalcNutrientsLink = new Uri(apiRoot.CalcNutrientsLink.OriginalString);
                }

                if (apiRoot.NutrientsToFoodRecLink != null)
                {
                    NutrientsToFoodRecLink = new Uri(apiRoot.NutrientsToFoodRecLink.OriginalString);
                }

                /*if (apiRoot.UserToFoodRecLink != null)
                {
                    UserToFoodRecLink = new Uri(apiRoot.UserToFoodRecLink.OriginalString);
                }*/
            }
        }
    }
}
