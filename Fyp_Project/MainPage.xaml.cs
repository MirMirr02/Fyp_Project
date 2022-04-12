using System;
using Xamarin.Forms;
using Firebase.Auth;
using Newtonsoft.Json;
using Fyp_Project.Model;
using Fyp_Project.View;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace Fyp_Project
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public string WebAPIkey = "AIzaSyCnVPEWTPkQxva29e3tutOajPb1ZShiX78";
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            // await Navigation.PushModalAsync(new NavigationPage(new TabbedBtm()));

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(EmailEntry.Text, PasswordEntry.Text);
                var content = await auth.GetFreshAuthAsync();
                var serializedcontnet = JsonConvert.SerializeObject(content);
                Preferences.Set("MyFirebaseRefreshToken", serializedcontnet);
                await Navigation.PushAsync(new TabbedBtm());
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Alert", "Invalid email or password", "Ok");
            }
        }

        private async void Button_Clicked2(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new Register_Gate()));
        }

        private async void Forget_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new Forget_Password()));
        }
    }
}