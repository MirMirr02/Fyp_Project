using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fyp_Project.Model;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterNGO : ContentPage
    {
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        public RegisterNGO()
        {
            InitializeComponent();
        }

        private async void NGO_Register(object sender, EventArgs e)
        {
            var donorusersignup = NGOSignUp.Text;
            var email = NGOSignUpEmail.Text;
            var password = NGOSignUpPassword.Text;

            if (String.IsNullOrEmpty(donorusersignup) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please fill in all the field!", "Ok");
            }

            else if (NGOSignUp.Text.Length < 4)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid Username! (Username should consists of at least 4 characters!)", "Ok");
            }

            else if (!NGOSignUpEmail.Text.Contains("@"))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid email address!", "Ok");
            }

            else if (NGOSignUpPassword.Text.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid Password! (Password should consists of at least 6 characters!)", "Ok");
            }

            else
            {
                try
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(NGOSignUpEmail.Text, NGOSignUpPassword.Text, NGOSignUp.Text);
                    string gettoken = auth.FirebaseToken;
                    var getuserdetails = authProvider.GetUserAsync(gettoken);
                    string getuid = auth.User.LocalId;
                    string getname = auth.User.DisplayName;
                    string getemail = auth.User.Email;
                    string getPassword = NGOSignUpPassword.Text;
                    FirebaseClient fc = new FirebaseClient("https://fyp-project-c3052-default-rtdb.asia-southeast1.firebasedatabase.app/");
                    var StoreUser = await fc
                        .Child("Account List NGO")
                        .Child("NGO List")
                        .PostAsync(new Person()
                        {
                            UserId = getuid,
                            User = NGOSignUp.Text,
                            UserEmail = NGOSignUpEmail.Text,
                            UserPassword = NGOSignUpPassword.Text,
                            Phone = NGOSignUpPhone.Text
                        });
                    await App.Current.MainPage.DisplayAlert("Congratulations!", "Account created successful!", "Confirmed");
                }

                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                }
            }

        }
    }
}