using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodApiClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodApiClient.Controllers
{
    public class FoodRecommendController : Controller
    {
        private static readonly UserNutrientsFoodViewModel _model = new UserNutrientsFoodViewModel();

        public async Task<IActionResult> UserNutrients(UserNutrientsViewModel model)
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

        public async Task<IActionResult> NutrientsFood(NutrientsFoodViewModel model)
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

        public async Task<IActionResult> UserNutrientsFood(UserNutrientsFoodViewModel model)
        {
            if (model.UserProfile != null)
                _model.UserProfile = model.UserProfile;
            if (model.Nutrients != null)
                _model.Nutrients = model.Nutrients;
            if (model.Options != null)
                _model.Options = model.Options;
            if (model.UseCalcNutrients.HasValue)
                _model.UseCalcNutrients = model.UseCalcNutrients;
            if (!_model.UseCalcNutrients.HasValue)
                _model.UseCalcNutrients = true;

            if (model.UserProfile != null)
                _model.Nutrients = await FoodApi.ApiInterface.CalcNutrients(_model.UserProfile);
            else if (model.Nutrients != null)
                _model.FoodList = await FoodApi.ApiInterface.FoodRecommend(_model.Nutrients, _model.Options);

            ViewData.Model = _model;
            return View();
        }
    }
}