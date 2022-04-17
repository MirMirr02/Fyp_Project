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
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Fyp_Project.Model;
using System.Collections.ObjectModel;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public ObservableCollection<Person> DatabaseItems { get; set; } = new
            ObservableCollection<Person>();
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        FirebaseClient fc = new FirebaseClient("https://fyp-project-c3052-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public MenuPage()
        {
            InitializeComponent();
            GetProfileInformationAndRefreshToken();
            BindingContext = this;
            var collection = fc
                .Child("Donate ID")
                .AsObservable<Person>()
                .Subscribe((dbevent) =>
                {
                    if (dbevent.Object != null)
                    {
                        DatabaseItems.Add(dbevent.Object);
                    }
                });
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
                MenuName.Text = GetUserName.User;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Alert", "Oh no !  Token expired", "OK");
            }
        }
    }
}
