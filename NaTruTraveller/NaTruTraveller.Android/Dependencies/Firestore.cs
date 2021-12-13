using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Interop;
using Java.Util;
using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NaTruTraveller.Droid.Dependencies.Firestore))]
namespace NaTruTraveller.Droid.Dependencies
{
    class Firestore : Java.Lang.Object, IFirestore, IOnCompleteListener
    {
        private List<Post> posts;
        private bool hasReadPost;

        public Firestore()
        {
            posts = new List<Post>();
        }

        public bool Insert(Post post)
        {
            try
            {
                var postDocument = new Dictionary<string, Java.Lang.Object>()
            {
                { "description", post.Description},
                { "venueName", post.VenueName},
                { "categoryId", post.CategoryId},
                { "categoryName", post.CategoryName},
                { "address", post.Address},
                { "latitude", post.Latitude},
                { "longitude", post.Longitude},
                { "distance", post.Distance},
                { "userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid}
            };

                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("posts");
                collection.Add(new HashMap(postDocument));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Post>> Read()
        {
            try
            {
                hasReadPost = false;
                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("posts");
                var query = collection.WhereEqualTo("userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid);
                query.Get().AddOnCompleteListener(this);

                for (int i = 0; i < 50; i++)
                {
                    await System.Threading.Tasks.Task.Delay(100);
                    if (hasReadPost)
                        break;
                }

                return posts;
            }
            catch (Exception ex)
            {
                return posts;
            }
        }

        // Update
        public async Task<bool> Update(Post post)
        {
            try
            {
                var postDocument = new Dictionary<string, Java.Lang.Object>
                {
                    { "description", post.Description},
                    { "venueName", post.VenueName},
                    { "categoryId", post.CategoryId},
                    { "categoryName", post.CategoryName},
                    { "address", post.Address},
                    { "latitude", post.Latitude},
                    { "longitude", post.Longitude},
                    { "distance", post.Distance},
                    { "userId", Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid}
                };

                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("posts");
                collection.Document(post.Id).Update(postDocument);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Post post)
        {
            try
            {
                var collection = Firebase.Firestore.FirebaseFirestore.Instance.Collection("posts");
                collection.Document(post.Id).Delete();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            if (task.IsSuccessful)
            {
                var documents = (QuerySnapshot)task.Result;

                posts.Clear();
                foreach(var doc in documents.Documents)
                {
                    Post postModel = new Post()
                    {
                        Description = doc.Get("description").ToString(),
                        VenueName = doc.Get("venueName").ToString(),
                        CategoryId = doc.Get("categoryId").ToString(),
                        CategoryName = doc.Get("categoryName").ToString(),
                        Address = doc.Get("address").ToString(),
                        Latitude = (double)doc.Get("latitude"),
                        Longitude = (double)doc.Get("longitude"),
                        Distance = (int)doc.Get("distance"),
                        UserId = doc.Get("userId").ToString(),
                        Id = doc.Id
                    };

                    posts.Add(postModel);
                }
            }
            else
            {
                posts.Clear();
            }

            hasReadPost = true;
        }
    }
}