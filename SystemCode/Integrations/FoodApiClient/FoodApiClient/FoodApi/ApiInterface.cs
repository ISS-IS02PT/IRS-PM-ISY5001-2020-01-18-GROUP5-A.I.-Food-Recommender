#define SHOW_DEBUG

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
#if SHOW_DEBUG
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
#endif
                //return null;
                throw ex;
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
                //return null;
                throw new ArgumentException("Unable to get link from API", nameof(uriCalcNutrients));
            }

            string strUriRelative = "";
            try
            {
                strUriRelative = GenerateRelativeUri(userProfile);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex.Source);
            }

            if (string.IsNullOrEmpty(strUriRelative))
            {
                //return null;
                throw new ArgumentException("Null relative uri generated", nameof(strUriRelative));
            }

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
                Console.WriteLine($"Carbohydrates = \t{nutrients.Carbohydrates}");
                Console.WriteLine($"Fats = \t{nutrients.TotalFats}");
#endif

                return nutrients;
            }
            catch (Exception ex)
            {
#if SHOW_DEBUG
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
#endif
                //return null;
                throw ex;
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
            List<int> listKeep = null, List<int> listChange = null, Uri uriFoodRecommend = null)
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
                //return null;
                throw new ArgumentException("Unable to get link from API", nameof(uriFoodRecommend));
            }

            string strUriRelative = "";
            try
            {
                strUriRelative = GenerateRelativeUri(nutrients, options, listKeep, listChange);
            }
            catch(Exception ex)
            {
                throw ex;
            }

            if (strUriRelative == null)
            {
                //return null;
                throw new ArgumentException("Null relative uri generated", nameof(strUriRelative));
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
                if (foodList != null)
                {
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
                }

                return foodList;
            }
            catch (Exception ex)
            {
#if SHOW_DEBUG
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
#endif
                //return null;
                throw ex;
            }
        }

        public static void CheckGenderString(ref string strGender)
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
#if SHOW_DEBUG
                Console.WriteLine($"Invalid gender, {strGender}");
#endif
                //return false;
                throw new ArgumentException($"Invalid gender, {strGender}", nameof(strGender));
            }

            strGender = strTemp;
            //return true;
        }

        public static void CheckActivityString(ref string strActivity)
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
                else if ((strTemp == "light") || (strTemp == "lightly") || (strTemp == "lightly active"))
                {
                    strTemp = "Lightly Active";
                }
                else if ((strTemp == "moderate") || (strTemp == "moderately") || (strTemp == "moderately active"))
                {
                    strTemp = "Moderately Active";
                }
                else if ((strTemp == "high") || (strTemp == "highly") || (strTemp == "highly active") ||
                    (strTemp == "very") || (strTemp == "very active"))
                {
                    strTemp = "Very Active";
                }
                else
                {
#if SHOW_DEBUG
                    Console.WriteLine($"Invalid activity, {strActivity}");
#endif
                    //return false;
                    throw new ArgumentException($"Invalid activity, {strActivity}", nameof(strActivity));
                }

                strActivity = strTemp;
            }

            //return true;
        }

        public static void CheckDietString(ref string strDiet)
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
#if SHOW_DEBUG
                Console.WriteLine($"Invalid diet, {strDiet}");
#endif
                //return false;
                throw new ArgumentException($"Invalid diet, {strDiet}", nameof(strDiet));
            }

            strDiet = strTemp;
            //return true;
        }
        #endregion

        #region Private Methods
        private static string GenerateRelativeUri(UserProfile userProfile)
        {
            if (userProfile == null)
            {
                //return null;
                throw new ArgumentException("Null user profile", nameof(userProfile));
            }

            if (double.IsNaN(userProfile.Age) || (userProfile.Age <= 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid age, {userProfile.Age}");
#endif
                //return null;
                throw new ArgumentException($"Invalid age, {userProfile.Age}",
                    nameof(userProfile.Age));
            }
            int nAge = (int)Math.Round(userProfile.Age);

            if (double.IsNaN(userProfile.Height) || (userProfile.Height <= 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid height, {userProfile.Height}");
#endif
                //return null;
                throw new ArgumentException($"Invalid height, {userProfile.Height}",
                    nameof(userProfile.Height));
            }
            int nHeight = (int)Math.Round(userProfile.Height);

            if (double.IsNaN(userProfile.Weight) || (userProfile.Weight <= 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid weight, {userProfile.Weight}");
#endif
                //return null;
                throw new ArgumentException($"Invalid weight, {userProfile.Weight}", nameof(userProfile.Weight));
            }
            int nWeight = (int)Math.Round(userProfile.Weight);

            string strGender = userProfile.Gender.ToString();
            try
            {
                CheckGenderString(ref strGender);
                strGender = strGender.Replace(' ', '_').ToLower();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            string strActivity = userProfile.Activity.ToString();
            try
            {
                CheckActivityString(ref strActivity);
                strActivity = strActivity.Replace(' ', '_').ToLower();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            string strDiet = userProfile.Diet.ToString();
            try
            {
                CheckDietString(ref strDiet);
                strDiet = strDiet.Replace(' ', '_').ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return $"?age={nAge}" +
                $"&height={nHeight}" +
                $"&weight={nWeight}" +
                $"&gender={strGender}" +
                $"&activity={strActivity}" +
                $"&diet={strDiet}";
        }

        private static string GenerateRelativeUri(Nutrients nutrients, FoodOptions options,
            List<int> listKeep = null, List<int> listChange = null)
        {
            if (nutrients == null)
            {
                //return null;
                throw new ArgumentException("Null nutrients", nameof(nutrients));
            }

            if (options == null)
            {
                options = new FoodOptions();
            }

            if (double.IsNaN(nutrients.Energy) || (nutrients.Energy < 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid energy, {nutrients.Energy}");
#endif
                //return null;
                throw new ArgumentException($"Invalid energy, {nutrients.Energy}",
                    nameof(nutrients.Energy));
            }
            double dbEnergy = Math.Round(nutrients.Energy, 2);

            if (double.IsNaN(nutrients.Proteins) || (nutrients.Proteins <= 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid proteins, {nutrients.Proteins}");
#endif
                //return null;
                throw new ArgumentException($"Invalid proteins, {nutrients.Proteins}",
                    nameof(nutrients.Proteins));
            }
            double dbProteins = Math.Round(nutrients.Proteins, 2);

            if (double.IsNaN(nutrients.Carbohydrates) || (nutrients.Carbohydrates <= 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid carbohydrates, {nutrients.Carbohydrates}");
#endif
                //return null;
                throw new ArgumentException($"Invalid carbohydrates, {nutrients.Carbohydrates}",
                    nameof(nutrients.Carbohydrates));
            }
            double dbCarbohydrates = Math.Round(nutrients.Carbohydrates, 2);

            if (double.IsNaN(nutrients.TotalFats) || (nutrients.TotalFats <= 0))
            {
#if SHOW_DEBUG
                Console.WriteLine($"Invalid fats, {nutrients.TotalFats}");
#endif
                //return null;
                throw new ArgumentException($"Invalid fats, {nutrients.TotalFats}",
                    nameof(nutrients.TotalFats));
            }
            double dbTotalFats = Math.Round(nutrients.TotalFats, 2);

            string strDiet = nutrients.Diet.ToString();
            try {
                CheckDietString(ref strDiet);
                strDiet = strDiet.Replace(' ', '_').ToLower();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            string strKeep = "", strChange = "";
            if (listKeep != null)
            {
                strKeep = string.Join(",", listKeep.ToArray());
            }
            if (listChange != null)
            {
                strChange = string.Join(",", listChange.ToArray());
            }
            return $"?CarbohydrateAmount_g={dbCarbohydrates}"
                + $"&EnergyAmount_kcal={dbEnergy}"
                + $"&ProteinAmount_g={dbProteins}"
                + $"&TotalFatAmount_g={dbTotalFats}"
                + $"&diet={strDiet}"
                + $"&isVegan={options.IsVegan}"
                + $"&isVegetarian={options.IsVegetarian}"
                + $"&isHalal={options.IsHalal}"
                + $"&containsBeef={options.ContainsBeef}"
                + $"&isAlcohol={options.IsAlcohol}"
                + $"&food_keep_index={strKeep}"
                + $"&food_change_index={strChange}";
        }
        #endregion

    }
}
