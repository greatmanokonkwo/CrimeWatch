using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using CrimeWatch.CustomElements;
using CrimeWatch.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace CrimeWatch.Droid
{
    class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            map.UiSettings.ZoomControlsEnabled = false;
            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);

            switch (pin.Label) 
            {
                case "Arson":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.arsonPin));
                    break;
                case "Assault Offenses":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.assaultPin));
                    break;
                case "Burglary/Breaking & Entering":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.burglaryPin));
                    break;
                case "Disorderly Conduct":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.disorderlyConductPin));
                    break;
                case "Driving Under the Influence":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.duiPin));
                    break;
                case "Drug/Narcotic Offenses":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.drugsPin));
                    break;
                case "Kidnapping/Abduction":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.abductionPin));
                    break;
                case "Homicide Offenses":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.homicidePin));
                    break;
                case "Sex Offenses, Forcible":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.sexOffensePin));
                    break;
                case "Suspicious Activity":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.suspiciousActivityPin));
                    break;
                case "Weapon Law Violations":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.weaponsPin));
                    break;
                case "Destruction/Damage/Vandalism of Property":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.vandalismPin));
                    break;
                case "Larceny/Theft Offenses":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.theftPin));
                    break;
                case "Robbery":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.theftPin));
                    break;
                case "Motor Vehicle Theft":
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.vehicleTheftPin));
                    break;
                default:
                    marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.otherPin));
                    break;
            }

            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            if (!string.IsNullOrWhiteSpace(customPin.Url))
            {
                var url = Android.Net.Uri.Parse(customPin.Url);
                var intent = new Intent(Intent.ActionView, url);
                intent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;

            /*
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                if (customPin.Name.Equals("Xamarin"))
                {
                    view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                }

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }

                return view;
            }
            */
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}