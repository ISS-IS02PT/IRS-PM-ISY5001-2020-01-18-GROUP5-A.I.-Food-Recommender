using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodApiClient.Models
{
    public class UserProfile
    {
        [Display(Name = "Age (years)"), Required, Range(1, int.MaxValue)]
        public double Age { get; set; }

        [Display(Name = "Height (cm)"), Required, Range(1, int.MaxValue)]
        public double Height { get; set; }

        [Display(Name = "Weight (kg)"), Required, Range(1, int.MaxValue)]
        public double Weight { get; set; }

        [Display(Name = "Gender"), Required]
        public CustomTypes.Gender Gender { get; set; }

        [Display(Name = "Activity"), Required]
        public CustomTypes.Activity Activity { get; set; }

        [Display(Name = "Diet"), Required]
        public CustomTypes.Diet Diet { get; set; }

        public UserProfile() { }
        public UserProfile(UserProfile userProfile)
        {
            if (userProfile != null)
            {
                Age = userProfile.Age;
                Height = userProfile.Height;
                Weight = userProfile.Weight;
                Gender = userProfile.Gender;
                Activity = userProfile.Activity;
                Diet = userProfile.Diet;
            }
        }
    }
}
