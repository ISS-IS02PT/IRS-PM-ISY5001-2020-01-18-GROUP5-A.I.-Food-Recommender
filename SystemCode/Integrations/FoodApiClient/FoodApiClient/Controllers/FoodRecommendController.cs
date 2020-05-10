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
        private static readonly UserNutrientsViewModel _modelUserNutrients = new UserNutrientsViewModel();
        private static readonly NutrientsFoodViewModel _modelNutrientsFood = new NutrientsFoodViewModel();
        private static readonly UserNutrientsFoodViewModel _modelUserNutrientsFood = new UserNutrientsFoodViewModel();

        public async Task<IActionResult> UserNutrients(UserNutrientsViewModel model)
        {
            if (model.UserProfile != null)
            {
                _modelUserNutrients.UserProfile = model.UserProfile;

                _modelUserNutrientsFood.UserProfile =
                    new UserProfile(_modelUserNutrients.UserProfile);
            }

            if (_modelUserNutrients.UserProfile != null)
            {
                try
                {
                    _modelUserNutrients.Nutrients =
                        await FoodApi.ApiInterface.CalcNutrients(_modelUserNutrients.UserProfile);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_modelUserNutrients.UserProfile), ex.Message);
                }

                _modelNutrientsFood.Nutrients = new Nutrients(_modelUserNutrients.Nutrients);

                _modelUserNutrientsFood.Nutrients = new Nutrients(_modelUserNutrients.Nutrients);
            }

            ViewData.Model = _modelUserNutrients;
            return View();
        }

        public async Task<IActionResult> NutrientsFood(NutrientsFoodViewModel model)
        {
            if (model.Nutrients != null)
            {
                _modelNutrientsFood.Nutrients = model.Nutrients;

                _modelUserNutrientsFood.Nutrients = new Nutrients(_modelNutrientsFood.Nutrients);
            }

            if (model.Options != null)
            {
                _modelNutrientsFood.Options = model.Options;

                _modelUserNutrientsFood.Options = new FoodOptions(_modelNutrientsFood.Options);
            }

            if (_modelNutrientsFood.Nutrients != null)
            {
                try
                {
                    _modelNutrientsFood.FoodList =
                        await FoodApi.ApiInterface.FoodRecommend(_modelNutrientsFood.Nutrients,
                        _modelNutrientsFood.Options);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(nameof(_modelNutrientsFood.Nutrients), ex.Message);
                }

                _modelUserNutrientsFood.FoodList =
                    _modelNutrientsFood.FoodList.Select(x => new Food(x)).ToList();
            }

            ViewData.Model = _modelNutrientsFood;
            return View();
        }

        public async Task<IActionResult> UserNutrientsFood(UserNutrientsFoodViewModel model,
            string function)
        {
            ModelState.Clear();
            if (model.UserProfile != null)
            {
                _modelUserNutrientsFood.UserProfile = model.UserProfile;

                _modelUserNutrients.UserProfile = new UserProfile(_modelUserNutrientsFood.UserProfile);
            }

            if (model.Nutrients != null)
            {
                _modelUserNutrientsFood.Nutrients = model.Nutrients;

                _modelNutrientsFood.Nutrients = new Nutrients(_modelUserNutrientsFood.Nutrients);
            }

            if (model.Options != null)
            {
                _modelUserNutrientsFood.Options = model.Options;

                _modelNutrientsFood.Options = new FoodOptions(_modelUserNutrientsFood.Options);
            }

            /*if (model.FoodList != null)
            {
                _modelUserNutrientsFood.FoodList = model.FoodList.Select(x => new Food(x)).ToList();

                _modelNutrientsFood.FoodList = _modelUserNutrientsFood.FoodList.Select(x => new Food(x)).ToList();
            }*/
            /*if (model.UseCalcNutrients.HasValue)
               _modelUserNutrientsFood.UseCalcNutrients = model.UseCalcNutrients;
            if (!_modelUserNutrientsFood.UseCalcNutrients.HasValue)
               _modelUserNutrientsFood.UseCalcNutrients = true;*/

            if (!string.IsNullOrEmpty(function))
            {
                if (function.ToLower().Contains("calculate nutrients") &&
                    (_modelUserNutrientsFood.UserProfile != null))
                {
                    try
                    {
                        _modelUserNutrientsFood.Nutrients =
                            await FoodApi.ApiInterface.CalcNutrients(_modelUserNutrientsFood.UserProfile);
                        _modelUserNutrientsFood.FoodList = null;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(nameof(_modelUserNutrientsFood.UserProfile), ex.Message);
                    }

                    _modelNutrientsFood.Nutrients = new Nutrients(_modelUserNutrientsFood.Nutrients);
                }
                else if ((function.ToLower().Contains("food recommendation") ||
                    function.ToLower().Contains("refresh")) &&
                    (_modelUserNutrientsFood.Nutrients != null))
                {
                    string strName = nameof(_modelUserNutrientsFood.Nutrients);
                    if (function.ToLower().Contains("refresh"))
                    {
                        strName = nameof(_modelUserNutrientsFood.FoodList);
                    }

                    List<int> listKeep = new List<int>(), listChange = new List<int>();
                    if (model.FoodList != null)
                    {
                        for (int i = 0; i < model.FoodList.Count; i++)
                        {
                            if (i < _modelUserNutrientsFood.FoodList.Count)
                            {
                                int nIndex = _modelUserNutrientsFood.FoodList[i].Index;
                                if (model.FoodList[i].CheckKeep)
                                {
                                    if (!listKeep.Contains(nIndex))
                                    {
                                        listKeep.Add(nIndex);
                                    }
                                }
                                else
                                {
                                    if (!listChange.Contains(nIndex))
                                    {
                                        listChange.Add(nIndex);
                                    }
                                }
                            }
                        }
                    }

                    List<Food> foodList = null;
                    try
                    {
                        foodList =
                            await FoodApi.ApiInterface.FoodRecommend(_modelUserNutrientsFood.Nutrients,
                            _modelUserNutrientsFood.Options, listKeep, listChange);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(strName, ex.Message);
                    }

                    if ((foodList == null) || (foodList.Count <= 0))
                    {
                        try
                        {
                            foodList =
                                await FoodApi.ApiInterface.FoodRecommend(_modelUserNutrientsFood.Nutrients,
                                _modelUserNutrientsFood.Options, listKeep);
                        }
                        catch (Exception ex)
                        {

                            ModelState.AddModelError(strName, ex.Message);
                        }
                    }

                    if (foodList != null)
                    {
                        _modelUserNutrientsFood.FoodList = foodList;
                    }
                    else
                    {
                        if (_modelUserNutrientsFood.FoodList != null)
                        {
                            _modelUserNutrientsFood.FoodList.Clear();
                        }
                    }

                    if (_modelUserNutrientsFood.FoodList != null)
                    {
                        _modelNutrientsFood.FoodList =
                        _modelUserNutrientsFood.FoodList.Select(x => new Food(x)).ToList();
                    }

                    if ((_modelUserNutrientsFood.FoodList == null) || (_modelUserNutrientsFood.FoodList.Count <= 0))
                    {
                        ModelState.AddModelError(strName,
                            "Insufficient food data to fulfill the nutrients requirement now, please come back later.");
                    }
                }
                else 
                {
                    string strUri = new string(function);
                    if (!Uri.IsWellFormedUriString(strUri, UriKind.Absolute))
                    {
                        strUri = $"http://{strUri}/";
                    }

                    if (Uri.IsWellFormedUriString(strUri, UriKind.Absolute))
                    {
                        try
                        {
                            await FoodApi.ApiInterface.ReadApiRoot(strUri);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(nameof(_modelUserNutrientsFood), ex.Message);
                        }
                    }
                }
            }

            ViewData.Model = _modelUserNutrientsFood;
            return View();
        }
    }
}