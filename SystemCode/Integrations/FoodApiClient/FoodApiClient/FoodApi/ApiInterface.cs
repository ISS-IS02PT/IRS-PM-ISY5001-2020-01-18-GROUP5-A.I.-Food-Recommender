//#define SHOW_DEBUG

using FoodApiClient.Converters;
using FoodApiClient.Models;
using Microsoft.CodeAnalysis.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodApiClient.FoodApi
{
    internal class ApiInterface
    {
        #region Constants
        private const string API_ROOT_URI = "http://127.0.0.1:8000/";
        #endregion

        #region Fields
        private static readonly HttpClient _client = new HttpClient();
        private static ApiRoot _apiRoot;
        private static readonly List<int> _listFoodHistory = new List<int>();
        private static KeepChangeIndex _keepChangeIndex = new KeepChangeIndex();
        #endregion

        #region Public Methods
        public static async Task<ApiRoot> ReadApiRoot()
        {
            try
            {
                Task<System.IO.Stream> streamTask = _client.GetStreamAsync(API_ROOT_URI);
                ApiRoot apiRoot = await JsonSerializer.DeserializeAsync<ApiRoot>(await streamTask);
                _apiRoot = new ApiRoot(apiRoot);

                return apiRoot;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        public static async Task<Nutrients> CalcNutrients(UserProfile userProfile, Uri uriCalcNutrients = null)
        {
            if (uriCalcNutrients == null)
            {
                if (_apiRoot == null)
                {
                    _ = await ReadApiRoot();
                }

                if ((_apiRoot != null) && (_apiRoot.CalcNutrientsLink != null))
                {
                    uriCalcNutrients = new Uri(_apiRoot.CalcNutrientsLink.OriginalString);
                }
            }

            if (uriCalcNutrients == null)
            {
                return null;
            }

            string strUriRelative = GenerateRelativeUri(userProfile);
            if (strUriRelative == null)
                return null;

            try
            {
                var streamTask = _client.GetStreamAsync(new Uri(uriCalcNutrients + strUriRelative));
                var nutrients = await JsonSerializer.DeserializeAsync<Nutrients>(await streamTask,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                string strDiet = nutrients.Diet.ToString();
                CheckDietString(ref strDiet);

#if SHOW_DEBUG
                Console.WriteLine($"Energy = \t{nutrients.Energy}");
                Console.WriteLine($"Proteins = \t{nutrients.Proteins}");
                Console.WriteLine($"Carbohydrates = \t{nutrients.Carbs}");
                Console.WriteLine($"Fats = \t{nutrients.Fats}");
#endif

                return nutrients;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        /*public static async Task<List<Food>> FoodRecommend(UserProfile userProfile, Uri uriFoodRecommend = null)
        {
            if (uriFoodRecommend == null)
            {
                if (_apiRoot == null)
                {
                    _ = await ReadApiRoot();
                }

                if ((_apiRoot != null) && (_apiRoot.UserToFoodRecLink != null))
                {
                    uriFoodRecommend = new Uri(_apiRoot.UserToFoodRecLink.OriginalString);
                }
            }

            if (uriFoodRecommend == null)
            {
                return null;
            }

            string strUriRelative = GenerateRelativeUri(userProfile);
            if (strUriRelative == null)
                return null;

            try
            {
                var streamTask = _client.GetStreamAsync(new Uri(uriFoodRecommend + strUriRelative));
                var foodList = await JsonSerializer.DeserializeAsync<List<Models.Food>>(await streamTask);

#if SHOW_DEBUG
                Console.WriteLine($"Name = \t{foodList[0].Name}");
                Console.WriteLine($"Group = \t{foodList[0].Group}");
                Console.WriteLine($"Energy = \t{foodList[0].Energy}");
                Console.WriteLine($"Proteins = \t{foodList[0].Proteins}");
                Console.WriteLine($"Carbohydrates = \t{foodList[0].Carbs}");
                Console.WriteLine($"Fats = \t{foodList[0].Fats}");
#endif

                return foodList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }*/

        public static async Task<List<Food>> FoodRecommend(Nutrients nutrients, FoodOptions options,
            Uri uriFoodRecommend = null)
        {
            if (uriFoodRecommend == null)
            {
                if (_apiRoot == null)
                {
                    _ = await ReadApiRoot();
                }

                if ((_apiRoot != null) && (_apiRoot.NutrientsToFoodRecLink != null))
                {
                    uriFoodRecommend = new Uri(_apiRoot.NutrientsToFoodRecLink.OriginalString);
                }
            }

            if (uriFoodRecommend == null)
            {
                return null;
            }

            string strUriRelative = GenerateRelativeUri(nutrients, options);
            if (strUriRelative == null)
            {
                return null;
            }

            try
            {
                /*var streamTask = _client.GetStreamAsync(new Uri(uriFoodRecommend + strUriRelative));
                string strStreamRead = string.Empty;
                using (StreamReader sr = new StreamReader(await streamTask))
                {
                    string strLine;
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        strStreamRead += strLine;
                    }
                }*/

                var streamTask = _client.GetStreamAsync(new Uri(uriFoodRecommend + strUriRelative));
                var foodList = await JsonSerializer.DeserializeAsync<List<Food>>(await streamTask);
                _keepChangeIndex = new KeepChangeIndex();
                if (foodList.Last().Keep != null)
                {
                    string[] strKeepIndex = foodList.Last().Keep.Split(',');
                    if (strKeepIndex != null)
                    {
                        foreach (string strIndex in strKeepIndex)
                        {
                            if (int.TryParse(strIndex, out int nIndex))
                            {
                                if (_keepChangeIndex.Keep == null)
                                {
                                    _keepChangeIndex.Keep = new List<int>();
                                }

                                if (!_keepChangeIndex.Keep.Contains(nIndex))
                                {
                                    _keepChangeIndex.Keep.Add(nIndex);
                                }
                            }
                        }
                    }
                }
                if (foodList.Last().Change != null)
                {
                    string[] strChangeIndex = foodList.Last().Change.Split(',');
                    if (strChangeIndex != null)
                    {
                        foreach (string strIndex in strChangeIndex)
                        {
                            if (int.TryParse(strIndex, out int nIndex))
                            {
                                if (_keepChangeIndex.Change == null)
                                {
                                    _keepChangeIndex.Change = new List<int>();
                                }

                                if (!_keepChangeIndex.Change.Contains(nIndex))
                                {
                                    _keepChangeIndex.Change.Add(nIndex);
                                }
                            }
                        }
                    }
                }
                foodList.RemoveAt(foodList.Count - 1);
                foodList.Sort(delegate (Food x, Food y)
                {
                    if (x.MealType.ToLower() == y.MealType.ToLower()) return 0;
                    else if ((x.MealType.ToLower() == "breakfast") || (y.MealType.ToLower() == "dinner")) return -1;
                    else if ((x.MealType.ToLower() == "dinner") || (y.MealType.ToLower() == "breakfast")) return 1;
                    else return 0;
                });

                foreach (Food food in foodList)
                {
                    if (!_listFoodHistory.Contains(food.Index))
                    {
                        _listFoodHistory.Add(food.Index);
                    }
                }
                return foodList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        public static bool CheckGenderString(ref string strGender)
        {
            string strTemp = strGender.Replace('_', ' ').ToLower();
            if ((strTemp == "m") || (strTemp == "male"))
            {
                strTemp = "Male";
            }
            else if ((strTemp == "f") || (strTemp == "female"))
            {
                strTemp = "Female";
            }
            else
            {
                Console.WriteLine($"Invalid gender, {strGender}");
                return false;
            }

            strGender = strTemp;
            return true;
        }

        public static bool CheckActivityString(ref string strActivity)
        {
            if (string.IsNullOrEmpty(strActivity))
            {
                strActivity = "Sedentary";
            }
            else
            {
                string strTemp = strActivity.Replace('_', ' ').ToLower();
                if ((strTemp == "none") || (strTemp == "sedentary"))
                {
                    strTemp = "Sedentary";
                }
                else if ((strTemp == "light") || (strTemp == "lightly active"))
                {
                    strTemp = "Lightly Active";
                }
                else if ((strTemp == "moderate") || (strTemp == "moderately active"))
                {
                    strTemp = "Moderately Active";
                }
                else if ((strTemp == "high") || (strTemp == "highly active") ||
                    (strTemp == "very") || (strTemp == "very active"))
                {
                    strTemp = "Very Active";
                }
                else
                {
                    Console.WriteLine($"Invalid activity, {strActivity}");
                    return false;
                }

                strActivity = strTemp;
            }

            return true;
        }

        public static bool CheckDietString(ref string strDiet)
        {
            string strTemp = strDiet.Replace('_', ' ').ToLower();

            if (string.IsNullOrEmpty(strTemp) || (strTemp == "none") ||
                (strTemp == "norm") || (strTemp == "normal") ||
                (strTemp == "any") || (strTemp == "anything") || (strTemp == "standard"))
            {
                //strTemp = "Anything";
                strTemp = "Standard";
            }
            else if ((strTemp == "keto") || (strTemp == "ketogenic"))
            {
                strTemp = "Ketogenic";
            }
            else
            {
                Console.WriteLine($"Invalid diet, {strDiet}");
                return false;
            }

            strDiet = strTemp;
            return true;
        }
        #endregion

        #region Private Methods
        private static string GenerateRelativeUri(UserProfile userProfile)
        {
            if (userProfile == null)
                return null;

            double age = userProfile.Age;
            double height = userProfile.Height;
            double weight = userProfile.Weight;
            string gender = userProfile.Gender.ToString();
            string activity = userProfile.Activity.ToString();
            string diet = userProfile.Diet.ToString();

            if (double.IsNaN(age) || (age <= 0))
            {
                Console.WriteLine($"Invalid age, {age}");
                return null;
            }
            int nAge = (int)Math.Round(age);

            if (double.IsNaN(height) || (height <= 0))
            {
                Console.WriteLine($"Invalid height, {height}");
                return null;
            }
            int nHeight = (int)Math.Round(height);

            if (double.IsNaN(weight) || (weight <= 0))
            {
                Console.WriteLine($"Invalid weight, {weight}");
                return null;
            }
            int nWeight = (int)Math.Round(weight);

            string strGender = new string(gender);
            if (!CheckGenderString(ref strGender))
            {
                return null;
            }
            else
            {
                strGender = strGender.Replace(' ', '_').ToLower();
            }

            string strActivity = new string(activity);
            if (!CheckActivityString(ref strActivity))
            {
                return null;
            }
            else
            {
                strActivity = strActivity.Replace(' ', '_').ToLower();
            }

            string strDiet = new string(diet);
            if (!CheckDietString(ref strDiet))
            {
                return null;
            }
            else
            {
                strDiet = strDiet.Replace(' ', '_').ToLower();
            }

            return $"?age={nAge}" + $"&height={nHeight}" + $"&weight={nWeight}" + $"&gender={strGender}"
                + $"&activity={strActivity}" + $"&diet={strDiet}";
        }

        private static string GenerateRelativeUri(Nutrients nutrients, FoodOptions options)
        {
            if (nutrients == null)
            {
                return null;
            }

            if (options == null)
            {
                options = new FoodOptions();
            }

            double energy = nutrients.Energy;
            double proteins = nutrients.Proteins;
            double carbs = nutrients.Carbohydrates;
            double fats = nutrients.TotalFats;
            string diet = nutrients.Diet.ToString();

            if (double.IsNaN(energy) || (energy < 0))
            {
                Console.WriteLine($"Invalid energy, {energy}");
                return null;
            }
            energy = Math.Round(energy, 2);

            if (double.IsNaN(proteins) || (proteins <= 0))
            {
                Console.WriteLine($"Invalid proteins, {proteins}");
                return null;
            }
            proteins = Math.Round(proteins, 2);

            if (double.IsNaN(carbs) || (carbs <= 0))
            {
                Console.WriteLine($"Invalid carbohydrates, {carbs}");
                return null;
            }
            carbs = Math.Round(carbs, 2);

            if (double.IsNaN(fats) || (fats <= 0))
            {
                Console.WriteLine($"Invalid fats, {fats}");
                return null;
            }
            fats = Math.Round(fats, 2);

            string strDiet = new string(diet);
            if (!CheckDietString(ref strDiet))
            {
                return null;
            }
            else
            {
                strDiet = strDiet.Replace(' ', '_').ToLower();
            }

            return $"?CarbohydrateAmount_g={carbs}"
                + $"&EnergyAmount_kcal={energy}"
                + $"&ProteinAmount_g={proteins}"
                + $"&TotalFatAmount_g={fats}"
                + $"&diet={strDiet}"
                + $"&isVegan={options.IsVegan}"
                + $"&isVegetarian={options.IsVegetarian}"
                + $"&isHalal={options.IsHalal}"
                + $"&containsBeef={options.ContainsBeef}"
                + $"&isAlcohol={options.IsAlcohol}"
                + $"&food_keep_index="
                + $"&food_change_index={string.Join(",", _listFoodHistory.ToArray())}";
        }
        #endregion

    }
}
