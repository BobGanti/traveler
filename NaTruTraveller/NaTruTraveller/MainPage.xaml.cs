using NaTruTraveller.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NaTruTraveller
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var assembly = typeof(MainPage);
            iconImage.Source = ImageSource.FromResource("NaTruTraveller.Assets.Images.cropped-favicon.png", assembly);
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
       {
            bool isEmailEmpty = string.IsNullOrEmpty(emailEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);
            if(isEmailEmpty || isPasswordEmpty)
            {
                _ = DisplayAlert("Error!", "Entries cannot be empty", "OK");
            }
            else
            {
                // authenticate user
                bool result = await Auth.LoginUser(emailEntry.Text, passwordEntry.Text);

                // navigate
                if(result)
                    await Navigation.PushAsync(new HomePage());
            }
        }
    }
}
