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
    public partial class PostDetailsPage : ContentPage
    {
        public PostDetailsPage(Post selectedPost)
        {
            InitializeComponent();

            (Resources["vm"] as PostDetailsVM).SelectedPost = selectedPost;
            descriptionEntry.Text = selectedPost.Description;
        }
    }
}