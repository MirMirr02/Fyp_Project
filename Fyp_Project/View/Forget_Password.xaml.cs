using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Forget_Password : ContentPage
    {
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        public Forget_Password()
        {
            InitializeComponent();
        }

        public async Task<bool> ResetPassword(string email)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            await authProvider.SendPasswordResetEmailAsync(emailEntry.Text);
            return true;
        }
        private async void reset_Clicked(object sender, EventArgs e)
        {
            string email = emailEntry.Text;

            if (String.IsNullOrEmpty(email))
            {
                await DisplayAlert("Alert", "Please enter your email!", "Ok");
            }

            else
            {
                try
                {
                    bool isSend = await ResetPassword(email);
                    if (isSend)
                    {
                        await DisplayAlert("Reset Password", "A link has been sent to your email address", "Ok");
                    }

                    else
                    {
                        await DisplayAlert("Reset Password", "Link Sent Fail", "Ok");
                    }
                }

                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                }
            }
        }

    }
}
