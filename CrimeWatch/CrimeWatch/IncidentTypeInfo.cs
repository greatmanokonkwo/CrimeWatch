using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CrimeWatch
{
    public static class IncidentTypeInfo
    {
        public static string Type { get; set; } = "All";

        // Returns the color for an incident and also sets the colors for all other filter buttons to gray
        public static Color GetIncidentColor()
        {

            switch (Type)
            {
                case "Arson":

                    return Color.FromHex("#c3a82a");

                case "Assault":

                    return Color.FromHex("#a8372b");

                case "Burglary":

                    return Color.FromHex("#4279b5");

                case "Disruption":

                    return Color.FromHex("#d57316");

                case "DUI":

                    return Color.FromHex("#593c2c");

                case "Drug":

                    return Color.FromHex("#c07aa9");

                case "Abduction":

                    return Color.FromHex("#b15b26");

                case "Homicide":

                    return Color.FromHex("#7446a3");

                case "Sex Offense":

                    return Color.FromHex("#007272");

                case "Suspicious":

                    return Color.FromHex("#8f8f8f");

                case "Weapon":

                    return Color.FromHex("#586d8f");

                case "Vandalism":

                    return Color.FromHex("#839456");

                case "Theft":

                    return Color.FromHex("#ab4d57");

                case "Vehicle Theft":

                    return Color.FromHex("#538b54");

                case "All":

                    return Color.Accent;

                default:

                    return Color.FromHex("#3ca98e");
            }

        }

        public static string GetIncidentIcon(string Type)
        {

            switch (Type)
            {
                case "Arson":

                    return "ArsonIcon.png";

                case "Assault Offenses":

                    return "AssaultIcon.png";

                case "Burglary/Breaking & Entering":

                    return "BurglaryIcon.png";

                case "Disorderly Conduct":

                    return "DisruptionIcon.png";

                case "Driving Under the Influence":

                    return "DUIIcon.png";

                case "Drug/Narcotic Offenses":

                    return "DrugIcon.png";

                case "Kidnapping/Abduction":

                    return "AbductionIcon.png";

                case "Homicide Offenses":

                    return "HomicideIcon.png";

                case "Sex Offenses, Forcible":

                    return "SexOffenseIcon.png";

                case "Suspicious Activity":

                    return "SuspiciousIcon.png";

                case "Weapon Law Violations":

                    return "WeaponIcon.png";

                case "Destruction/Damage/Vandalism of Property":

                    return "VandalismIcon.png";

                case "Larceny/Theft Offenses":

                    return "TheftIcon.png";

                case "Robbery":

                    return "TheftIcon.png";

                case "Motor Vehicle Theft":

                    return "VehicleTheftIcon.png";

                default:

                    return "OtherIcon.png";
            }
        }
    }
}
