using Foundation;
using NaTruTraveller.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NaTruTraveller.iOS.Dependencies.Auth))]
namespace NaTruTraveller.iOS.Dependencies
{
    class Auth : IAuth
    {
        public Auth()
        {
        }

        public string GetCurrentUserId()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser.Uid;
        }

        public bool IsAuthenticated()
        {
            return Firebase.Auth.Auth.DefaultInstance.CurrentUser != null;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return true;
            }
            catch (NSErrorException e)
            {
                string message = e.Message.Substring(e.Message.IndexOf("NSLocalizedDescription=", StringComparison.CurrentCulture));
                message = message.Replace("NSLocalizedDescription", "").Split(".")[0];
                throw new Exception(message);
            }
            catch(Exception ex)
            {
                throw new Exception("An unknown error has occured!");
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            try
            {
                await Firebase.Auth.Auth.DefaultInstance.CreateUserAsync(email, password);
                return true;
            }
            catch(NSErrorException e)
            {
                string message = e.Message.Substring(e.Message.IndexOf("NSLocalizedDescription=", StringComparison.CurrentCulture));
                message = message.Replace("NSLocalizedDescription", "").Split(".")[0];
                throw new Exception(message);
            }
            catch
            {
                throw new Exception("An unknown error has occured!");
            }
        }
    }
}