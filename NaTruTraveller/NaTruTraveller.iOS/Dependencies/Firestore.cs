using Foundation;
using NaTruTraveller.Helpers;
using NaTruTraveller.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NaTruTraveller.iOS.Dependencies.Firestore))]
namespace NaTruTraveller.iOS.Dependencies
{
    class Firestore : IFirestore
    {
        public bool Insert(Post post)
        {
            try
            {
                var keys = new[]
                {
                    new NSString("description"),
                    new NSString("venueName"),
                    new NSString("categoryId"),
                    new NSString("categoryName"),
                    new NSString("address"),
                    new NSString("latitude"),
                    new NSString("longitude"),
                    new NSString("distance"),
                    new NSString("userId")
                };

                var values = new NSObject[]
                {
                    new NSString(post.Description),
                    new NSString(post.VenueName),
                    new NSString(post.CategoryId),
                    new NSString(post.CategoryName),
                    new NSString(post.Address),
                    new NSNumber(post.Latitude),
                    new NSNumber(post.Longitude),
                    new NSNumber(post.Distance),
                   // new NSString(Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid),
                    new NSString(new Auth().GetCurrentUserId())
                };

                var document = new NSDictionary<NSString, NSObject>(keys, values);

                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
                collection.AddDocument(document);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<List<Post>> Read()
        {
            try
            {
                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
                //var query = collection.WhereEqualsTo("userId", new Auth().GetCurrentUserId());
                var query = collection.WhereEqualsTo("userId", Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid);
                var documents = await query.GetDocumentsAsync();

                var posts = new List<Post>();
                foreach (var doc in documents.Documents)
                {
                    var dictionary = doc.Data;

                    var postModel = new Post()
                    {
                        Description = dictionary.ValueForKey(new NSString("description")) as NSString,
                        VenueName = dictionary.ValueForKey(new NSString("venueName")) as NSString,
                        CategoryId = dictionary.ValueForKey(new NSString("categoryId")) as NSString,
                        CategoryName = dictionary.ValueForKey(new NSString("categoryName")) as NSString,
                        Address = dictionary.ValueForKey(new NSString("address")) as NSString,
                        Latitude = (double)(dictionary.ValueForKey(new NSString("latitude")) as NSNumber),
                        Longitude = (double)(dictionary.ValueForKey(new NSString("longitude")) as NSNumber),
                        Distance = (int)(dictionary.ValueForKey(new NSString("distance")) as NSNumber),
                        UserId = dictionary.ValueForKey(new NSString("userId")) as NSString,
                        Id = doc.Id
                    };

                    posts.Add(postModel);
                }

                return posts;
            }
            catch (Exception)
            {

                return new List<Post>();
            }
        }

        public async Task<bool> Update(Post post)
        {
            try
            {
                var keys = new[]
                {
                    new NSString("description"),
                    new NSString("venuName"),
                    new NSString("categoryId"),
                    new NSString("categoryName"),
                    new NSString("address"),
                    new NSString("latitude"),
                    new NSString("longitude"),
                    new NSString("distance"),
                    new NSString("userId")
                };

                var values = new NSObject[]
                {
                    new NSString(post.Description),
                    new NSString(post.VenueName),
                    new NSString(post.CategoryId),
                    new NSString(post.CategoryName),
                    new NSString(post.Address),
                    new NSNumber(post.Latitude),
                    new NSNumber(post.Longitude),
                    new NSNumber(post.Distance),
                    new NSString(Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid),
                    //new NSString(new Auth().GetCurrentUserId())
                };

                var document = new NSDictionary<NSObject, NSObject>(keys, values);

                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
                await collection.GetDocument(post.Id).UpdateDataAsync(document);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Post post)
        {
            try
            {
                var collection = Firebase.CloudFirestore.Firestore.SharedInstance.GetCollection("posts");
                await collection.GetDocument(post.Id).DeleteDocumentAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}