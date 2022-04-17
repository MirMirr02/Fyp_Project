using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using Fyp_Project.Model;
using Xamarin.Essentials;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        FirebaseClient fc = new FirebaseClient("https://fyp-project-c3052-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public Profile()
        {
            InitializeComponent();
            GetProfileInformationAndRefreshToken();
        }

        async private void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                //This is the saved firebaseauthentication that was saved during the time of login
                var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
                //Here we are Refreshing the token
                var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
                Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
                //Now lets grab user information
                var authoruid = savedfirebaseauth.User.LocalId;
                FirebaseClient fc = new FirebaseClient("https://fyp-project-c3052-default-rtdb.asia-southeast1.firebasedatabase.app/");
                var GetUserName = (await fc
                               .Child("UserList")
                               .OnceAsync<Person>()).Select(items => new Person
                               {
                                   UserId = items.Object.UserId,
                                   User = items.Object.User

                               }).Where(a => a.UserId == authoruid).FirstOrDefault();
                profilename.Text = GetUserName.User;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }
        }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            Preferences.Remove("MyFirebaseRefreshToken");
            App.Current.MainPage = new NavigationPage(new MainPage());

        }
    }
}