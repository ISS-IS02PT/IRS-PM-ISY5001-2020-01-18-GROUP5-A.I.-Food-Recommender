//#define SHOW_DEBUG

using FoodApiClient.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodApiClient.FoodApi
{
    public class ApiInterface
    {
        #region Constants
        private const string API_ROOT_URI = "http://127.0.0.1:8000/";
        #endregion

        #region Fields
        private static readonly HttpClient _client = new HttpClient();
        #endregion

        #region Public Methods
        internal static async Task<Models.ApiRoot> ReadApiRoot()
        {
            try
            {
                var streamTask = _client.GetStreamAsync(API_ROOT_URI);
                var apiRoot = await JsonSerializer.DeserializeAsync<Models.ApiRoot>(await streamTask);
#if SHOW_DEBUG
                Console.WriteLine($"ApiRoot:\tUri = {apiRoot.CalcNutrientsLink}");
                Console.WriteLine($"ApiRoot:\tUri = {apiRoot.NutrientsToFoodRecLink}");
                Console.WriteLine($"ApiRoot:\tUri = {apiRoot.UserToFoodRecLink}");
#endif
                return apiRoot;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        internal static async Task<List<Models.FoodList>> ReadFoodList(Uri uriFoodList)
        {
            try
            {
                var streamTask = _client.GetStreamAsync(uriFoodList);
                var foodlist = await JsonSerializer.DeserializeAsync<List<Models.FoodList>>(await streamTask);

#if SHOW_DEBUG
                foreach (var food in foodlist)
                {
                    string tabs = "\t\t\t";
                    Console.WriteLine($"\tFoodList:\tID = {food.ID},");
                    Console.WriteLine($"{tabs}Name = {food.Name},");
                    Console.WriteLine($"{tabs}Uri = {food.Link}");

                    var foodDetail = ReadFoodDetail(food.Link);
                }
#endif

                return foodlist;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        internal static async Task<Models.FoodDetail> ReadFoodDetail(Uri uriFoodDetail)
        {
            try
            {
                var streamTask = _client.GetStreamAsync(uriFoodDetail);
                var foodDetail = await JsonSerializer.DeserializeAsync<Models.FoodDetail>(await streamTask);

#if SHOW_DEBUG
                string tabs = "\t\t\t\t";
                Console.WriteLine($"\t\tFoodDetail:\tID = {foodDetail.ID},");
                Console.WriteLine($"{tabs}Name = {foodDetail.Name},");
                Console.WriteLine($"{tabs}Group = {foodDetail.Group}, ");
                Console.WriteLine($"{tabs}Nutrient Protein = {foodDetail.Proteins} {foodDetail.ProteinUnit},");
                Console.WriteLine($"{tabs}Nutrient Sugar = {foodDetail.Sugar} {foodDetail.SugarUnit},");
                Console.WriteLine($"{tabs}Uri = {foodDetail.Link}");
#endif

                return foodDetail;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        internal static async Task<Models.Nutrients> CalcNutrients(Uri uriCalcNutrients, UserProfile userProfile)
        {
            string strUriRelative = GenerateRelativeUri(userProfile);
            if (strUriRelative == null)
                return null;

            try
            {
                var streamTask = _client.GetStreamAsync(new Uri(uriCalcNutrients + strUriRelative));
                var nutrients = await JsonSerializer.DeserializeAsync<Models.Nutrients>(await streamTask);

                string strDiet = nutrients.Diet;
                CheckDietString(ref strDiet);

#if SHOW_DEBUG
                Console.WriteLine($"Energy = \t{nutrients.Energy}");
                Console.WriteLine($"Proteins = \t{nutrients.Proteins}");
                Console.WriteLine($"Carbohydrates = \t{nutrients.Carbs}");
                Console.WriteLine($"Fats = \t{nutrients.Fats}");
#endif

                return nutrients;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        internal static async Task<List<Models.Food>> FoodRecommend(Uri uriFoodRecommend, UserProfile userProfile)
        {
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
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        internal static async Task<List<Models.Food>> FoodRecommend(Uri uriFoodRecommend, Nutrients nutrients)
        {
            string strUriRelative = GenerateRelativeUri(nutrients);
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
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                return null;
            }
        }

        internal static bool CheckGenderString(ref string strGender)
        {
            string strTemp = strGender.ToLower();
            if ((strTemp == "m") || (strTemp == "male"))
                strTemp = "Male";
            else if ((strTemp == "f") || (strGender == "female"))
                strTemp = "Female";
            else
            {
                Console.WriteLine($"Invalid gender, {strGender}");
                return false;
            }

            strGender = strTemp;
            return true;
        }

        internal static bool CheckActivityString(ref string strActivity)
        {
            if (string.IsNullOrEmpty(strActivity))
                strActivity = "Sedentary";
            else
            {
                string strTemp = strActivity.ToLower();
                if ((strTemp == "none") || (strTemp == "sedentary"))
                {
                    strTemp = "Sedentary";
                }
                else if ((strActivity == "light") || (strActivity == "lightly active"))
                {
                    strTemp = "Lightly Active";
                }
                else if ((strActivity == "moderate") || (strActivity == "moderately active"))
                {
                    strTemp = "Moderately Active";
                }
                else if ((strActivity == "high") || (strActivity == "highly active") ||
                    (strActivity == "very") || (strActivity == "very active"))
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

        internal static bool CheckDietString(ref string strDiet)
        {
            string strTemp = strDiet.ToLower();

            if (string.IsNullOrEmpty(strTemp) || (strTemp == "none") ||
                (strTemp == "norm") || (strTemp == "normal") ||
                (strTemp == "any") || (strTemp == "anything"))
            {
                strTemp = "Anything";
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
            double age = userProfile.Age;
            double height = userProfile.Height;
            double weight = userProfile.Weight;
            string gender = userProfile.Gender;
            string activity = userProfile.Activity;
            string diet = userProfile.Diet;

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
            int nHeight = (int)Math.Round(age);

            if (double.IsNaN(weight) || (weight <= 0))
            {
                Console.WriteLine($"Invalid weight, {weight}");
                return null;
            }

            string strGender = new string(gender);
            if (!CheckGenderString(ref strGender))
                return null;
            else
            {
                userProfile.Gender = new string(strGender);
                strGender = strGender.Replace(' ', '_').ToLower();
            }

            string strActivity = new string(activity);
            if (!CheckActivityString(ref strActivity))
                return null;
            else
            {
                userProfile.Activity = new string(strActivity);
                strActivity = strActivity.Replace(' ', '_').ToLower();
            }

            string strDiet = new string(diet);
            if (!CheckDietString(ref strDiet))
                return null;
            else
            {
                userProfile.Diet = new string(strDiet);
                strDiet = strDiet.Replace(' ', '_').ToLower();
            }

            return $"?age={age}" + $"&height={height}" + $"&weight={weight}" + $"&gender={strGender}" 
                + $"&activity={strActivity}" + $"&diet={strDiet}";
        }

        private static string GenerateRelativeUri(Nutrients nutrients)
        {
            double energy = nutrients.Energy;
            double proteins = nutrients.Proteins;
            double carbs = nutrients.Carbs;
            double fats = nutrients.Fats;
            string diet = nutrients.Diet;

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
                return null;
            else
            {
                nutrients.Diet = new string(strDiet);
                strDiet = strDiet.Replace(' ', '_').ToLower();
            }

            return $"?CarbohydrateAmount_g={carbs}" + $"&EnergyAmount_kcal={energy}" + $"&ProteinAmount_g={proteins}"
                + $"&TotalFatAmount_g={fats}" + $"&diet={strDiet}";
        }
        #endregion
    }
}
