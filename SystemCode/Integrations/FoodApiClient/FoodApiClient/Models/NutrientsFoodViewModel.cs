using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class NutrientsFoodViewModel
    {
        public Nutrients Nutrients { get; set; }
        public FoodOptions Options { get; set; }
        public List<Food> FoodList { get; set; }
    }
}
