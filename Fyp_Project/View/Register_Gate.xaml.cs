using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fyp_Project.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register_Gate : ContentPage
    {
        public Register_Gate()
        {
            InitializeComponent();
        }

        private async void Button_Clicked3(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new RegisterNGO()));
        }
        private async void Button_Clicked4(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new RegisterDONOR()));
        }
    }
}