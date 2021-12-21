using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using NaTruTraveller.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NaTruTraveller
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        private HistoryVM vm;
        public HistoryPage()
        {
            InitializeComponent();
            vm = Resources["vm"] as HistoryVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.GetPosts();
        }

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    //postListView.ItemsSource = null;
        //}

        //private void postListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    var selectedPost = postListView.SelectedItem as Post;

        //    if(selectedPost != null)
        //    {
        //        Navigation.PushAsync(new PostDetailsPage(selectedPost));
        //    }
        //}
    }
}