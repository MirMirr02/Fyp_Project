using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;
using Firebase.Auth;
using Firebase.Database;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fyp_Project.Model;
using Firebase.Database.Query;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Upload : ContentPage
    {
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        public Upload()
        {
            InitializeComponent();
            BindingContext = new Person();
        }

        async void Upload_Clicked(System.Object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
            var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
            Guid guid = Guid.NewGuid();
            var Authorid = savedfirebaseauth.User.LocalId;
            var photo = await Xamarin.Essentials.MediaPicker.PickPhotoAsync();
            if (photo == null)
                return;

            var task = await new FirebaseStorage("fyp-project-c3052.appspot.com",
                new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                })
                .Child("Photo")
                .Child("FOOD-" + guid.ToString("N").Substring(0, 8).ToUpper() + ".png")
                .PutAsync(await photo.OpenReadAsync());
            guidtext.Text = guid.ToString("N").Substring(0, 8).ToUpper();

            var downloadlink = task;
            downloadLink.Text = downloadlink;
            var stream = await photo.OpenReadAsync();
            mediapath.Source = ImageSource.FromStream(() => stream);
        }



        async void post_Clicked(object sender, EventArgs args)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            var savedfirebaseauth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("MyFirebaseRefreshToken", ""));
            var RefreshedContent = await authProvider.RefreshAuthAsync(savedfirebaseauth);
            Preferences.Set("MyFirebaseRefreshToken", JsonConvert.SerializeObject(RefreshedContent));
            var authoruid = savedfirebaseauth.User.LocalId;
            FirebaseClient fc = new FirebaseClient("https://fyp-project-c3052-default-rtdb.asia-southeast1.firebasedatabase.app/");
            Guid guid = Guid.NewGuid();
            var GetUserName = (await fc
                               .Child("UserList")
                               .OnceAsync<Person>()).Select(items => new Person
                               {
                                   UserId = items.Object.UserId,
                                   User = items.Object.User,
                                   UserEmail = items.Object.UserEmail
                               }).Where(a => a.UserId == authoruid).FirstOrDefault();
            if (guidtext.Text == null || descp.Text == null || downloadLink.Text == null)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Please Fill In The Details", "OK");
                return;
            }
            else
            {
                var result = await fc
                    .Child("Donate ID")
                    .PostAsync(new Person()
                     { 
                        NewsDateTime = DateTime.Now.ToString("dd MMM yyyy"),
                        GUID = guidtext.Text,
                        User = GetUserName.User,
                    });

                await App.Current.MainPage.DisplayAlert("Congratulation", "Uploaded", "OK");
                await Navigation.PushAsync(new TabbedBtm());
            }
        }
    }
}