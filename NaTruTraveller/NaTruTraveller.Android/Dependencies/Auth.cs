using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using NaTruTraveller.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(NaTruTraveller.Droid.Dependencies.Auth))]
namespace NaTruTraveller.Droid.Dependencies
{
    class Auth : IAuth
    {
        public Auth()
        {

        }
        public string GetCurrentUserId()
        {
            return Firebase.Auth.FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public bool IsAuthenticated()
        {
            return Firebase.Auth.FirebaseAuth.Instance.CurrentUser != null;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch(FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                throw new Exception("There is no user record corresponding to this identifier");
            }
            catch (Exception ex)
            {

                throw new Exception("There is an unknown error!");
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthWeakPasswordException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                throw new Exception(e.Message);
            }
            catch (FirebaseAuthUserCollisionException e)
            {
                throw new Exception(e.Message);
            }
            catch (Exception ex)
            {

                throw new Exception("There is an unknown error!");
            }
        }
    }
}