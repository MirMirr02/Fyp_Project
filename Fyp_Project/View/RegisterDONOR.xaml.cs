using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Fyp_Project.Model;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterDONOR : ContentPage
    {
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        public RegisterDONOR()
        {
            InitializeComponent();
        }
        private async void Donor_Register(object sender, EventArgs e)
        {
            var donorusersignup = DONORSignUp.Text;
            var email = DONORSignUpEmail.Text;
            var password = DONORSignUpPassword.Text;

            if (String.IsNullOrEmpty(donorusersignup) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please fill in all the field!", "Ok");
            }

            else if (DONORSignUp.Text.Length < 4)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid Username! (Username should consists of at least 4 characters!)", "Ok");
            }

            else if (!DONORSignUpEmail.Text.Contains("@"))
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid email address!", "Ok");
            }

            else if (DONORSignUpPassword.Text.Length < 6)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid Password! (Password should consists of at least 6 characters!)", "Ok");
            }

            else
            {
                try
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(DONORSignUpEmail.Text, DONORSignUpPassword.Text, DONORSignUp.Text);
                    string gettoken = auth.FirebaseToken;
                    var getuserdetails = authProvider.GetUserAsync(gettoken);
                    string getuid = auth.User.LocalId;
                    string getname = auth.User.DisplayName;
                    string getemail = auth.User.Email;
                    string getPassword = DONORSignUpPassword.Text;
                    FirebaseClient fc = new FirebaseClient("https://fyp-project-c3052-default-rtdb.asia-southeast1.firebasedatabase.app/");
                    var StoreUser = await fc
                        .Child("UserList")
                        .PostAsync(new Person()
                        {
                            UserId = getuid,
                            User = DONORSignUp.Text,
                            UserEmail = DONORSignUpEmail.Text,
                            UserPassword = DONORSignUpPassword.Text,
                            Phone = DONORSignUpPhone.Text
                        }) ;
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