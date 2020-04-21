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
    public class FoodDetailsModel : PageModel
    {
        public Models.FoodDetail Food { get; set; }

        public async Task OnGetAsync(Uri link)
        {
            if (link != null)
                Food = await ApiInterface.ReadFoodDetail(link);
        }
    }
}