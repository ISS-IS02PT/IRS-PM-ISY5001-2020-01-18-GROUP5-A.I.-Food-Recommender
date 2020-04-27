using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FoodApiClient.Controllers
{
    public class FoodRecommendController : Controller
    {
        public async Task<IActionResult> UserNutrients(Models.UserNutrientsViewModel model)
        {
            if (model.UserProfile != null)
            {
                model.Nutrients = await FoodApi.ApiInterface.CalcNutrients(model.UserProfile);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> NutrientsFood(Models.NutrientsFoodViewModel model)
        {
            if (model.Nutrients != null)
            {
                model.FoodList = await FoodApi.ApiInterface.FoodRecommend(model.Nutrients, model.Options);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> UserFood(Models.UserNutrientsFoodViewModel model)
        {
            return View();
        }

        public async Task<IActionResult> CalcNutrients(Models.UserProfile userProfile)
        {
            if (userProfile != null)
            {
                return View(new Models.UserNutrientsViewModel()
                {
                    UserProfile = userProfile,
                    Nutrients = await FoodApi.ApiInterface.CalcNutrients(userProfile)
                });
            }
            else
            {
                return View();
            }
        }
    }
}