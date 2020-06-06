using CrimeWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrimeWatch.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrimeLogPage : ContentPage
    {
        // Report Crime Page
        private ReportCrimePage reportCrimePage = new ReportCrimePage();

        public CrimeLogPage()
        {
            InitializeComponent();

            InfoFrame.BackgroundColor = IncidentTypeInfo.GetIncidentColor();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            activityIndicator.IsRunning = true;

            var incidents = (await CrimeFilterer.Filter()).ToArray();

            for (int i = 0; i < incidents.Length; i++)
            {
                Incident incident = incidents[i];

                // Set visual properties for incidents
                incident.Icon = IncidentTypeInfo.GetIncidentIcon(incident.Type);
                incident.StandardTime = Convert.ToDateTime(incident.Time).ToString("dddd, dd MMMM yyyy");
            }

            listView.ItemsSource = incidents;

            activityIndicator.IsRunning = false;

            numOfIncidentsLabel.Text = $"{incidents.Length} Incidents";
            InfoFrame.BackgroundColor = IncidentTypeInfo.GetIncidentColor();
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

        private async void OnClearAllClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;

            CrimeFilterer.IncidentTypeFilter = "None";

            var incidents = await CrimeFilterer.Filter();

            listView.ItemsSource = incidents;

            numOfIncidentsLabel.Text = "0 Incidents";

            activityIndicator.IsRunning = false;
        }
    }
}