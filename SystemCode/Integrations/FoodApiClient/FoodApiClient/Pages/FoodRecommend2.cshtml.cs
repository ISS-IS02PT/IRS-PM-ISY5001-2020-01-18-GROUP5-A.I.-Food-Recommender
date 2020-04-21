using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodApiClient.Pages
{
    public class FoodRecommend2Model : PageModel
    {
        private static Uri _link;

        [BindProperty]
        public Models.Nutrients Nutrients { get; set; }

        public List<Models.Food> FoodList { get; set; }

        public bool ShowFoodList { get; set; }

        public void OnGet(Uri link)
        {
            if (link != null)
                _link = link;
        }

        public async Task OnPostAsync()
        {
            if ((ModelState.IsValid) && (_link != null) && (Nutrients != null))
            {
                FoodList = await FoodApi.ApiInterface.FoodRecommend(_link, Nutrients);

                ShowFoodList = (FoodList != null);
            }
        }
    }
}