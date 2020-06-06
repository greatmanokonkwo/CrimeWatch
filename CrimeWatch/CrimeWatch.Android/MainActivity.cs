using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Xamarin.Forms;

namespace CrimeWatch.Droid
{
    [Activity(Label = "Crime Watch", Icon = "@mipmap/CrimeWatchLogo", RoundIcon = "@mipmap/CrimeWatchLogoRound", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.SetFlags("SwipeView_Experimental");
            Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnStart()
        {

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                while (true) 
                {
                    if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                    {
                        RequestPermissions(LocationPermissions, RequestLocationId);
                    }
                    else
                    {
                        break;
                        // Permissions already granted - display a message.
                    }
                }
            }

            base.OnStart();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {

            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted)) 
                {
                    // Permissions granted - display a message.
                }
                else 
                {
                    // Permissions denied - display a message.
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
    }
}