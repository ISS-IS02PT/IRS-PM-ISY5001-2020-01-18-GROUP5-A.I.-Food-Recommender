using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoodApiClient
{
    class Program
    {
        #region Fields
        private static readonly HttpClient client = new HttpClient();
        private const string URI_FOOD_PROJECT = "http://127.0.0.1:8000/";
        #endregion

        static async Task Main(string[] args)
        {
            string strUri = URI_FOOD_PROJECT;
            if (args.GetLength(0) > 0)
            {
                if (Uri.TryCreate(args[0], UriKind.Absolute, out Uri uri))
                {
                    strUri = args[0];
                }
            }

            await ProcessFoodData(strUri);
        }

        private static async Task ProcessFoodData(string strUri)
        {
            try
            {
                var streamTask = client.GetStreamAsync(strUri);
                var apiRoot = await JsonSerializer.DeserializeAsync<ApiRoot>(await streamTask);
                Console.WriteLine($"ApiRoot:\tUri = {apiRoot.Uri}");

                streamTask = client.GetStreamAsync(apiRoot.Uri);
                var foodlist = await JsonSerializer.DeserializeAsync<List<FoodList>>(await streamTask);

                foreach (var food in foodlist)
                {
                    string tabs = "\t\t\t";
                    Console.WriteLine($"\tFoodList:\tID = {food.ID},");
                    Console.WriteLine($"{tabs}Name = {food.Name},");
                    Console.WriteLine($"{tabs}Uri = {food.Uri}");

                    streamTask = client.GetStreamAsync(food.Uri);
                    var foodDetail = await JsonSerializer.DeserializeAsync<FoodDetail>(await streamTask);
                    tabs = "\t\t\t\t";
                    Console.WriteLine($"\t\tFoodDetail:\tID = {foodDetail.ID},");
                    Console.WriteLine($"{tabs}Name = {foodDetail.Name},");
                    Console.WriteLine($"{tabs}Group = {foodDetail.Group}, ");
                    Console.WriteLine($"{tabs}Nutrient Protein = {foodDetail.ProteinAmount} {foodDetail.ProteinUnit},");
                    Console.WriteLine($"{tabs}Nutrient Sugar = {foodDetail.SugarAmount} {foodDetail.SugarUnit},");
                    Console.WriteLine($"{tabs}Uri = {foodDetail.Uri}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
            }
        }
    }
}
