using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AntiTelemarketer
{
    public partial class FirstPage : ContentPage
    {
        public FirstPage()
        {
            InitializeComponent();

         
            Navigation.PushModalAsync(new NavigationPage(new MainPage()) {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.FromHex("#2f8ff0"),
                BackgroundColor = Color.White,
            });

        }
    }
}
