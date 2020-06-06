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
    public partial class ReportCrimePage : ContentPage
    {
        public ReportCrimePage()
        {
            InitializeComponent();
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}