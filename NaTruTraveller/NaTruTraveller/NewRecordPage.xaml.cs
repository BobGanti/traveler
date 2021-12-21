
using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using Plugin.Geolocator;
using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaTruTraveller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRecordPage : ContentPage
    {
        public NewRecordPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var locator = CrossGeolocator.Current;
                var status = await CheckAndRequestLocationPermission();

                if (status == PermissionStatus.Granted && locator.IsGeolocationEnabled == true)
                {
                    var position = await locator.GetPositionAsync();
                    var venues = await Venue.GetVenues(position.Latitude, position.Longitude);

                    venueListView.ItemsSource = venues;
                }
                else
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
            catch (Exception ex)
            {
               
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selectedVenue = venueListView.SelectedItem as Venue;
                var firstCategory = selectedVenue.categories.FirstOrDefault();

                Post post = new Post
                {
                    Description = descriptionEntry.Text,
                    CategoryId = firstCategory.id,
                    CategoryName = firstCategory.name,
                    Address = selectedVenue.location.address,
                    Distance = selectedVenue.location.distance,
                    Latitude = selectedVenue.location.lat,
                    Longitude = selectedVenue.location.lng,
                    VenueName = selectedVenue.name
                };

                bool result = Firestore.Insert(post);
                if (result)
                {
                    descriptionEntry.Text = string.Empty;
                    DisplayAlert("Success!", "Item is successfully saved", "OK");
                    //Navigation.PushAsync(new HomePage());
                }
                else
                {
                    DisplayAlert("Failure!", "Item was not saved", "OK");
                }
            }
            catch (NullReferenceException nre)
            {
                Console.WriteLine(nre.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            return status;
        }
    }
}