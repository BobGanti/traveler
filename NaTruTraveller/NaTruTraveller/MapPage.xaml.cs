
using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Position = Xamarin.Forms.Maps.Position;

namespace NaTruTraveller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        IGeolocator locator = CrossGeolocator.Current;

        public MapPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            GetLocation();
            GetPosts();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await locator.StopListeningAsync();
        }

        private async void GetLocation()
        {
            var status = await CheckAndRequestLocationPermission();

            if (status == PermissionStatus.Granted && locator.IsGeolocationEnabled == true)
            {
                var location = await Geolocation.GetLocationAsync();

                locator.PositionChanged += Locator_PositionChanged;
                if (locator.IsListening == false)
                {
                    await locator.StartListeningAsync(new TimeSpan(0, 1, 0), 100);
                }

                locationsMap.IsShowingUser = true;

                CenterMap(location.Latitude, location.Longitude);
            }
            else
            {
                if (locator.IsGeolocationEnabled == false)
                {
                    string msg = "";
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        msg += "Go to Settings\nPrivacy\nLocation Services";
                    }

                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        msg = "Go to Settings\nLocation";
                    }

                    await Navigation.PushAsync(new HomePage());
                    await DisplayAlert("Location is off: ", msg, "Ok");
                }
            }
        }

        private void DisplayOnMap(List<Post> posts)
        {
            foreach(var post in posts)
            {
                try
                {
                    var pinCoordinates = new Position(post.Latitude, post.Longitude);

                    var pin = new Pin()
                    {
                        Position = pinCoordinates,
                        Label = post.VenueName,
                        Address = post.Address,
                        Type = PinType.SavedPin
                    };

                    locationsMap.Pins.Add(pin);
                }
                catch (NullReferenceException nre)
                {
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async void GetPosts()
        {
            var posts = await Firestore.Read();
            DisplayOnMap(posts);
        }

        private async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            //// test data
            //e.Position.Latitude = 53.350140;
            //e.Position.Longitude = -6.266155;

            CenterMap(e.Position.Latitude, e.Position.Longitude);
        }

        private void CenterMap(double latitude, double longitude)
        {
            Position center = new Position(latitude, longitude);
            MapSpan span = new MapSpan(center, 1, 1);

            locationsMap.MoveToRegion(span);
        }
    }
}