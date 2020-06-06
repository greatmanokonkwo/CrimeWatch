using CrimeWatch.CustomElements;
using CrimeWatch.Models;
using CrimeWatch.Pages;
using CrimeWatch.Services;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

/** EarthquakeCityMap
 * A mobile application that maps out crimes for the user on an interactive map, with primary focus on deliviring information on local crime. 
 * Crime data coverage only for the U.S
 * Author: Greatman Okonwko
 * @author Your name here
 * Date: June 5, 2020
 * */

namespace CrimeWatch
{
    [DesignTimeVisible(false)]
    public partial class MapPage : ContentPage
    {
        // TO STORE SETTINGS DATA USE APPLICATION PERSISTENCE PROPERTIES

        // object for making calls to crimeometer API and getting back data 
        private RestService _restService;

        private bool runAnimation = true;

        private IGeolocator userLocator;

        // Variable that determines if the user can make url requests or not
        private static bool offline = true;

        // Report Crime Page
        private ReportCrimePage reportCrimePage = new ReportCrimePage();

        public MapPage()
        {

            InitializeComponent();

            InfoFrame.BackgroundColor = IncidentTypeInfo.GetIncidentColor();

            _restService = new RestService();

            TopLabel.Text = $"Crimes Within A {CrimeFilterer.DistanceFilter}km Radius";

            SearchForCrime();

            userLocator = CrossGeolocator.Current;
        }


        protected async override void OnAppearing()
        {
            /*
            Move map to user location

            var position = await userLocator.GetPositionAsync();
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(0.444));
            */

            TopLabel.Text = $"Crimes Within A {CrimeFilterer.DistanceFilter}km Radius";

            await CreatePins();
        }

        protected async Task CreatePins()
        {
            activityIndicator.IsRunning = true;

            // Get list of crime incidents from local database, filtering according to the requested filter 
            Incident[] incidents = (await CrimeFilterer.Filter()).ToArray();

            numOfIncidentsLabel.Text = $"{incidents.Length} Incidents";
            InfoFrame.BackgroundColor = IncidentTypeInfo.GetIncidentColor();

            crimeMap.Pins.Clear();

            // Create a pin for each for incident
            for (int i = 0; i < incidents.Length; i++)
            {

                Incident incident = incidents[i];

                // initialize pin
                CustomPin pin = new CustomPin
                {
                    Type = PinType.Place,
                    Position = new Position(incident.Latitude, incident.Longitude),
                    Label = $"{incident.Type}",
                    Address = $"{incident.Description}"
                };

                // add pin
                crimeMap.Pins.Add(pin);

            }

            if (crimeMap.Pins.Count > 0)
            {
                var lastPin = crimeMap.Pins.First();

                // Move map to Location with crime pins
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(lastPin.Position.Latitude, lastPin.Position.Longitude), Distance.FromKilometers(4));
                crimeMap.MoveToRegion(mapSpan);
            }

            activityIndicator.IsRunning = false;
        }

        async void SearchForCrime()
        {

            IncidentTypeInfo.Type = "All";

            // Search Animation. Set runAnimation to have it do a search animations
            runAnimation = false;
            RunAnimation();

            IncidentData incidentData;

            // get crime data 
            if (offline)
            {
                incidentData = _restService.GetCrimeData();
            }
            else 
            {
                incidentData = await _restService.GetCrimeDataAsync(await GenerateRequestUri());
            }

            Incident[] incidents = incidentData.Incidents;

            for (int i = 0; i < incidents.Length; i++)
            {
                Incident incident = incidents[i];

                // Set properties for incidents
                incident.Icon = IncidentTypeInfo.GetIncidentIcon(incident.Type);
                incident.StandardTime = Convert.ToDateTime(incident.Time).ToString("dddd, dd MMMM yyyy");
            }

            /*

            App.Database.ClearIncidents();

            // For each incident, initialize a corresponding pin and add to map's list of pins
            for (int i=0; i < incidents.Length; i++) 
            {
                Incident incident = incidentData.Incidents[i];

                DetectedLabel.Text = $"{incident.Type} Found";

                // Save Incident to Database
                await App.Database.SaveIncidentAsync(incident);
            }

            */

            // stop search animation
            runAnimation = false;

            // create map pins
            await CreatePins();
        }

        async Task RunAnimation()
        {
            overlay.IsVisible = true;
            circle1.IsVisible = true;
            circle2.IsVisible = true;
            circle3.IsVisible = true;
            searchIcon.IsVisible = true;

            while (runAnimation)
            {
                await Task.WhenAll(
                circle1.ScaleTo(2, 1000),
                circle2.ScaleTo(2, 1000),
                circle3.ScaleTo(2, 1000)
                );
                await Task.WhenAll(
                    circle1.ScaleTo(1, 1000),
                    circle2.ScaleTo(1, 1000),
                    circle3.ScaleTo(1, 1000)
                );
            }

            overlay.IsVisible = false;
            circle1.IsVisible = false;
            circle2.IsVisible = false;
            circle3.IsVisible = false;
            searchIcon.IsVisible = false;
        }

        // build uri for request
        async Task<string> GenerateRequestUri()
        {
            // get user's location
            var position = await userLocator.GetPositionAsync();

            // uri request
            string requestUri = $"v1/incidents/raw-data?lat={position.Latitude}" +
                                                        $"&lon={position.Longitude}" +
                                                        $"&distance=km" +
                                                        $"&datetime_ini={DateTime.Now.AddDays(-(Int32.Parse("2"))):yyyy-MM-ddTHH:mm:ss.fffZ}" +
                                                        $"&datetime_end={DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffZ}";
            return requestUri;
        }

        private async void OnUserLocateClicked(object sender, EventArgs e)
        {
            var position = await userLocator.GetPositionAsync();
            MapSpan mapSpan = MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(4));
            crimeMap.MoveToRegion(mapSpan);
        }

        private void OnCrimeSearchButtonClicked(object sender, EventArgs e)
        {
            SearchForCrime();
        }

        private async void OnFilterClicked(object sender, EventArgs e)
        {
            CrimeFilterer.filterPage.Parent = null;
            await Navigation.PushAsync(CrimeFilterer.filterPage);
        }

        private async void OnReportClicked(object sender, EventArgs e)
        {
            reportCrimePage.Parent = null;
            await Navigation.PushAsync(reportCrimePage);
        }
    }
}


