using CrimeWatch.Models;
using CrimeWatch.Pages;
using CrimeWatch.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrimeWatch
{
    public static class CrimeFilterer
    {
        // object for making calls to crimeometer API and getting back data 
        private static RestService _restService = _restService = new RestService();

        // "Filter By" Page
        public static ContentPage filterPage = new FilterPage();

        // List of offense types for filtering
        // Read the FBI National Incident-Based Reporting System (NIBRS) for more information: https://ucr.fbi.gov/nibrs/2012/resources/nibrs-offense-definitions
        public static string[] incidentTypes = { "Arson", "Assault Offenses", "Burglary/Breaking & Entering", "Disorderly Conduct", "Driving Under the Influence",
                                            "Drug/Narcotic Offenses", "Kidnapping/Abduction", "Homicide Offenses", "Sex Offenses, Forcible", "Weapon Law Violations", "Motor",
                                            "Destruction/Damage/Vandalism of Property", "Larceny/Theft Offenses", "Robbery", "Motor Vehicle Theft", "Suspicious Activity"};

        public static string IncidentTypeFilter { get; set; } = "All";

        public static int DistanceFilter { get; set; } = 3000;

        public static string TimeFrameFilter { get; set; } = "1 Week";

        // Filter according to incideny type
        public static async Task<List<Incident>> Filter()
        {
            if (!IncidentTypeFilter.Equals("None"))
            {
                List<Incident> unfilteredList = _restService.GetCrimeData().Incidents.ToList();
                List<Incident> filteredList = null;
                // First filter incidents by distance from user
                IGeolocator userLocator = CrossGeolocator.Current;
                var userLocation = await userLocator.GetPositionAsync(); // get users position
                filteredList = (from incident in unfilteredList
                                where userLocation.CalculateDistance(new Position(incident.Latitude, incident.Longitude), GeolocatorUtils.DistanceUnits.Kilometers) < DistanceFilter
                                select incident).ToList();

                // Second filter incidents by time in which they occurred
                filteredList = (from incident in filteredList
                                where DateTime.Now.Subtract(Convert.ToDateTime(incident.Time)).TotalHours < GetTimeFrameInHours()
                                select incident).ToList();



                // Third filter incidents by incident type
                // Skip over third filtering if the incident type filter is set to All
                if (!IncidentTypeFilter.Equals("All"))
                {
                    // If the incident type is not set to other then filter based on the incident type
                    if (!IncidentTypeFilter.Equals("Other"))
                    {
                        filteredList = (from incident in filteredList
                                        where IncidentTypeFilter.Contains(incident.Type)
                                        select incident).ToList();
                    }
                    // Other represents the crimes that are not explicitly mentioned in the incidentTypes array. These incident types are printed when the filter is sett to other
                    else
                    {
                        filteredList = (from incident in filteredList
                                        where !incidentTypes.Contains(incident.Type)
                                        select incident).ToList();
                    }
                }

                return filteredList;
            }
            else 
            {
                return new List<Incident>();
            }   
            
        }

        // returns the number of hours for each time frame 
        private static int GetTimeFrameInHours() 
        {
            int hours = 0;

            switch (TimeFrameFilter) 
            {
                case "12 Hours":
                    hours = 12;
                    break;
                case "24 Hours":
                    hours = 24;
                    break;
                case "2 Days":
                    hours = 48;
                    break;
                case "4 Days":
                    hours = 96;
                    break;
                case "1 Week":
                    hours = 168;
                    break;
                case "2 Weeks":
                    hours = 336;
                    break;
                case "1 Month":
                    hours = 744;
                    break;
            }

            return hours;
        }
    }
}
