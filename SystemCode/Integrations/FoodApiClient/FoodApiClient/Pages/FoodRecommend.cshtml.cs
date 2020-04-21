using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodApiClient.Pages
{
    public class FoodRecommendModel : PageModel
    {
        private static Uri _link, _linkNutrients;

        [BindProperty]
        public Models.UserProfile UserProfile { get; set; }

        public Models.Nutrients Nutrients { get; set; }

        public List<Models.Food> FoodList { get; set; }

        public bool ShowNutrients { get; set; }

        public bool ShowFoodList { get; set; }

        public void OnGet(Uri link, Uri linkNutrients = null)
        {
            if (link != null)
                _link = link;

            if (linkNutrients != null)
                _linkNutrients = linkNutrients;
        }

        public async Task OnPostAsync()
        {
            if (!ModelState.IsValid)
                return;

            if (UserProfile != null)
            {
                if (_linkNutrients != null)
                {
                    Nutrients = await FoodApi.ApiInterface.CalcNutrients(_linkNutrients, UserProfile);
                    ShowNutrients = (Nutrients != null);
                }

                if (_link != null)
                {
                    FoodList = await FoodApi.ApiInterface.FoodRecommend(_link, UserProfile);
                    ShowFoodList = (FoodList != null);
                }
            }
        }
    }
}