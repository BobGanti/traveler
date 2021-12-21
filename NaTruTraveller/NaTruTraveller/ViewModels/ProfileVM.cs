using NaTruTraveller.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NaTruTraveller.ViewModels
{
    public class ProfileVM : INotifyPropertyChanged
    {
        private string datePath = "mobileApps-21/12/21";
        public ObservableCollection<CategoryCount> Categories { get; set; }
        
        private int postCount;
        public int PostCount
        {
            get => postCount;
            set
            {
                postCount = value;
                OnPropertyChanged("PostCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProfileVM()
        {
            Categories = new ObservableCollection<CategoryCount>();
        }

        public async void GetPosts()
        {
            Categories.Clear();
            var posts = await Firestore.Read();
            PostCount = posts.Count();

            var venueCategories = posts
                         .OrderBy(p => p.CategoryId)
                         //.Select(p => p.CategoryName)
                         .Distinct()
                         .ToList();

            foreach (var venueCategory in venueCategories)
            {
                var categories = posts
                    .Where(p => p.CategoryName == venueCategory.CategoryName)
                    .ToList();

                var count = categories.Count;

                Categories.Add(new CategoryCount()
                {
                    CategoryName = venueCategory.CategoryName,
                    VenueName = venueCategory.VenueName,
                    Count = count

                });
            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class CategoryCount
        {
            public string CategoryName { get; set; }
            public string VenueName { get; set; }
            public int Count { get; set; }
        }
    }
}
