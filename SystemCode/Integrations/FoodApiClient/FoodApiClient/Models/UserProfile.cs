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
        [RegularExpression(@"\b[mMfF]\b|\b[mM][aA][lL][eE]\b|\b[fF][eE][mM][aA][lL][eE]\b")]
        public string Gender { get; set; }

        [Display(Name = "Activity"), Required]
        [RegularExpression(@"\b[nN][oO][nN][eE]\b|\b[sS][eE][dD][eE][nN][tT][aA][rR][yY]\b|" +
                           @"\b[lL][iI][gG][hH][tT]\b|\b[lL][iI][gG][hH][tT][lL][yY] [aA][cC][tT][iI][vV][eE]\b|" +
                           @"\b[mM][oO][dD][eE][rR][aA][tT][eE]\b|\b[mM][oO][dD][eE][rR][aA][tT][eE][lL][yY] [aA][cC][tT][iI][vV][eE]\b|" +
                           @"\b[hH][iI][gG][hH]\b|\b[hH][iI][gG][hH][lL][yY] [aA][cC][tT][iI][vV][eE]\b|" +
                           @"\b[vV][eE][rR][yY]\b|\b[vV][eE][rR][yY] [aA][cC][tT][iI][vV][eE]\b")]
        public string Activity { get; set; }

        [Display(Name = "Diet Type"), Required]
        [RegularExpression(@"\b[nN][oO][nN][eE]\b|\b[nN][oO][rR][mM]\b|\b[nN][oO][rR][mM][aA][lL]\b|" +
                           @"\b[aA][nN][yY]\b|\b[aA][nN][yY][tT][hH][iI][nN][gG]\b|" +
                           @"\b[kK][eE][tT][oO]\b|\b[kK][eE][tT][oO][gG][eE][nN][iI][cC]\b")]
        public string Diet { get; set; }
    }
}
