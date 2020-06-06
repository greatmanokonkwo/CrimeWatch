using Xamarin.Forms.Maps;

namespace CrimeWatch.CustomElements
{
    // Go to CrimeWatch.Android/CustomMapRenderer to view implementation of custom pin 
    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
