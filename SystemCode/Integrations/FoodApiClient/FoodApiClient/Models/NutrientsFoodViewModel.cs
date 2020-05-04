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

        public NutrientsFoodViewModel() { }
        public NutrientsFoodViewModel(NutrientsFoodViewModel model)
        {
            if (model != null)
            {
                Nutrients = new Nutrients(model.Nutrients);
                Options = new FoodOptions(model.Options);
                FoodList = model.FoodList.Select(x => new Food(x)).ToList();
            }
        }
    }
}
