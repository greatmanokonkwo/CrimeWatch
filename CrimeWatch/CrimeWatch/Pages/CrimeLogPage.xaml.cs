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

            var incidents = await CrimeFilterer.Filter();

            listView.ItemsSource = incidents;

            activityIndicator.IsRunning = false;

            numOfIncidentsLabel.Text = $"{incidents.Count} Incidents";
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

            App.Database.ClearIncidents();

            var incidents = await CrimeFilterer.Filter();

            listView.ItemsSource = new List<Incident>();

            numOfIncidentsLabel.Text = $"0 Incidents";

            activityIndicator.IsRunning = false;
        }
    }
}