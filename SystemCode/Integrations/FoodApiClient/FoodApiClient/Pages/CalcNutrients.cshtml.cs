using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodApiClient.Pages
{
    public class CalcNutrientsModel : PageModel
    {
        private static Uri _link;

        [BindProperty]
        public Models.UserProfile UserProfile { get; set; }

        public Models.Nutrients Nutrients { get; set; }

        public bool ShowNutrients { get; set; }

        public void OnGet(Uri link)
        {
            if (link != null)
                _link = link;
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid && (_link != null) && (UserProfile != null))
            {
                Nutrients = await FoodApi.ApiInterface.CalcNutrients(_link, UserProfile);

                ShowNutrients = (Nutrients != null);
            }
        }
    }
}