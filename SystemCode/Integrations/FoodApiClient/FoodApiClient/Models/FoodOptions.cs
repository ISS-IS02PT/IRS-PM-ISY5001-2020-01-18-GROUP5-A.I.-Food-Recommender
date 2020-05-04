using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class FoodOptions
    {
        [Display(Name = "Vegan")]
        public bool IsVegan { get; set; } = false;

        [Display(Name = "Vegetarian")]
        public bool IsVegetarian { get; set; } = false;

        [Display(Name = "Halal")]
        public bool IsHalal { get; set; } = false;

        [Display(Name = "Contains Beef")]
        public bool ContainsBeef { get; set; } = false;

        [Display(Name = "Contains Alcohol")]
        public bool IsAlcohol { get; set; } = false;

        public FoodOptions() { }
        public FoodOptions(FoodOptions options)
        {
            if (options != null)
            {
                IsVegan = options.IsVegan;
                IsVegetarian = options.IsVegetarian;
                IsHalal = options.IsHalal;
                ContainsBeef = options.ContainsBeef;
                IsAlcohol = options.IsAlcohol;
            }
        }
    }
}
