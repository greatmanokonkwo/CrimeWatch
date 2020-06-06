using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace CrimeWatch.CustomElements
{
    public class CustomMap : Map
    {
        // Go to CrimeWatch.Android/CustomMapRenderer to view implementation of custom map 
        public List<CustomPin> CustomPins { get; set; }
    }
}
