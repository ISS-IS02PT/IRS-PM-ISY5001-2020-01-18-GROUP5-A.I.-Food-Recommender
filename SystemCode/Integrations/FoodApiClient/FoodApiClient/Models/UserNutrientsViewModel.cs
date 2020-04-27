using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class UserNutrientsViewModel
    {
        public Models.UserProfile UserProfile { get; set; }
        public Models.Nutrients Nutrients { get; set; }
    }
}
