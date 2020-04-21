using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodApiClient.FoodApi;
using FoodApiClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodApiClient.Pages
{
    public class FoodListModel : PageModel
    {
        private static List<Models.FoodList> _foodNames;
        public List<Models.FoodList> FoodNames 
        {
            get { return _foodNames; }
            set { _foodNames = value; }
        }

        public async Task OnGetAsync(Uri link)
        {
            if (link != null)
                FoodNames = await ApiInterface.ReadFoodList(link);
        }
    }
}