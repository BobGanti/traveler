using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NaTruTraveller.ViewModels
{
    class PostDetailsVM
    {
        public Command UpdateCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Post SelectedPost { get; set; }

        public PostDetailsVM()
        {
            UpdateCommand = new Command<string>(UPdate, CanUpdate);
            DeleteCommand = new Command(Delete);
        }

        private bool CanUpdate(string descriptionUpdate)
        {
            if (string.IsNullOrWhiteSpace(descriptionUpdate))
                return false;
            
            return true;
        }

        private async void UPdate(string descriptionUpdate)
        {
            SelectedPost.Description = descriptionUpdate;

            bool result = await Firestore.Update(SelectedPost);
            if (result)
            {
                await App.Current.MainPage.DisplayAlert("Success!", "Item is successfully updated", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failure!", "Item was not updated", "OK");
            }
        }

        private async void Delete(object obj)
        {
            bool result = await Firestore.Delete(SelectedPost);
            if (result)
            {
                await App.Current.MainPage.Navigation.PopAsync();
            }
        }
    }
}
