using CrimeWatch.Pages;
using System;
using System.IO;
using Xamarin.Forms;

namespace CrimeWatch
{
    public partial class App : Application
    {

        static Database database;

        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "incidents.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            Device.SetFlags(new string[] { "Expander_Experimental" });

            MainPage = new NavigationPage(new MainNavigationPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }
    }
}
