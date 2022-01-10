using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Fyp_Project.View;

namespace Fyp_Project
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new TabbedBtm()));
        }

        private async void Button_Clicked2(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new Register()));
        }
    }
}