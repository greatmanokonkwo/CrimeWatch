using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;

namespace CrimeWatch.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPage : ContentPage
    {   
        public FilterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            distanceEditor.Text = $"{CrimeFilterer.DistanceFilter}";
            timeFrame.SelectedItem = CrimeFilterer.TimeFrameFilter;
        }

        void OnIncidentTypeFilterClicked(object sender, EventArgs e)
        {
            Button filterButton = (Button)sender;

            // Change button style
            if (!filterButton.ImageSource.ToString().Substring(6).Contains("2."))
            {
                filterAppliedNotificationText.Text = "Filter Applied";
                ShowFilterAppliedConfirmation();

                filterButton.ImageSource = filterButton.ImageSource.ToString().Substring(6).Replace(".", "2.");
                filterButton.TextColor = Color.White;
                IncidentTypeInfo.Type = filterButton.Text;
                filterButton.BackgroundColor = IncidentTypeInfo.GetIncidentColor();
                SwitchOffOtherButtons(filterButton.Text);

                // each filter button sets the incidentFilter string differently, by incident type, and if no filter is selected string is left empty
                // Goes theough each incident type and checks is the filter button clicked corresponds to the incident type 
                for (int i = 0; i < CrimeFilterer.incidentTypes.Length; i++)
                {
                    if (CrimeFilterer.incidentTypes[i].Contains(filterButton.Text))
                    {
                        Debug.WriteLine($"{CrimeFilterer.incidentTypes[i]}");
                        CrimeFilterer.IncidentTypeFilter = CrimeFilterer.incidentTypes[i];
                        break;
                    }
                    else
                    {
                        CrimeFilterer.IncidentTypeFilter = "Other";
                    }
                }

                if (filterButton.Text.Equals("Theft"))
                {
                    CrimeFilterer.IncidentTypeFilter = "Larceny/Theft Offenses | Robbery";
                }

                if (filterButton.Text.Equals("Disruption"))
                {
                    CrimeFilterer.IncidentTypeFilter = "Disorderly Conduct";
                }

                if (filterButton.Text.Equals("DUI"))
                {
                    CrimeFilterer.IncidentTypeFilter = "Driving Under the Influence";
                }

            }
            else
            {
                filterAppliedNotificationText.Text = "Filter Removed";
                ShowFilterAppliedConfirmation();

                filterButton.ImageSource = filterButton.ImageSource.ToString().Substring(6).Replace("2.", ".");
                filterButton.TextColor = Color.Black;
                filterButton.BackgroundColor = Color.FromHex("#EBEAEA");
                IncidentTypeInfo.Type = "All";
                CrimeFilterer.IncidentTypeFilter = "All";
            }

        }

        async Task ShowFilterAppliedConfirmation()
        {
            await filterAppliedNotification.FadeTo(1,500);
            await filterAppliedNotification.FadeTo(0, 500);
        }

        private void SwitchOffOtherButtons(string type) 
        {
            Button[] buttons = { arson, assault, burglary, disruption, drugs, dui, homicide, abduction, vehicle, 
                                 sexOffense, suspicious, theft, vandalism, weapon, other};

            for (int i = 0; i < buttons.Length; i++) 
            {
                if (!buttons[i].Text.Equals(type)) 
                {
                    buttons[i].ImageSource = buttons[i].ImageSource.ToString().Substring(6).Replace("2.", ".");
                    buttons[i].TextColor = Color.Black;
                    buttons[i].BackgroundColor = Color.FromHex("#EBEAEA");
                }
            }
        }

        async void OnBackClicked(object sender, EventArgs e) 
        {

            CrimeFilterer.DistanceFilter = Math.Abs(int.Parse(distanceEditor.Text));
            CrimeFilterer.TimeFrameFilter = (string)timeFrame.SelectedItem;
           
            await Navigation.PopAsync();
        }

        // BEWARE THIS IS EXTREMELY STUPID CODE. I'm just too tired to make anything nice rn

        private async void OnExpanderTapped(object sender, EventArgs e)
        {
            if (DropDown1.Rotation == 0)
            {
                await DropDown1.RotateTo(180, 1);
            }
            else {
                await DropDown1.RotateTo(0, 1);
            }

        }

        private async void OnExpanderTapped2(object sender, EventArgs e)
        {
            if (DropDown2.Rotation == 0)
            {
                await DropDown2.RotateTo(180, 1);
            }
            else
            {
                await DropDown2.RotateTo(0, 1);
            }

        }

        private async void OnExpanderTapped3(object sender, EventArgs e)
        {
            if (DropDown2.Rotation == 0)
            {
                await DropDown3.RotateTo(180, 1);
            }
            else
            {
                await DropDown3.RotateTo(0, 1);
            }

        }
    }
}
