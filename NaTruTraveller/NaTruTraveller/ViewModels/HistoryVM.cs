using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NaTruTraveller.ViewModels
{
    class HistoryVM
    {
        public ObservableCollection<Post> Posts { get; set; }

        private Post selectedPost;

        public Post SelectedPost
        {
            get { return selectedPost; }
            set 
            { selectedPost = value;
                if (selectedPost != null)
                    App.Current.MainPage.Navigation.PushAsync(new PostDetailsPage(selectedPost));
            }
        }

        public HistoryVM()
        {
            Posts = new ObservableCollection<Post>();
        }

        public async void GetPosts()
        {
            Posts.Clear();
            var posts = await Firestore.Read();

            foreach(var post in posts)
            {
                Posts.Add(post);
            }
        }
    }
}
