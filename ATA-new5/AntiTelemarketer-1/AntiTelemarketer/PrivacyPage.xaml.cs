using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AntiTelemarketer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PrivacyPage : ContentPage
	{
		public PrivacyPage ()
		{
			InitializeComponent ();
		}

        private void MenuSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingPage());
        }

        private void MenuPrivacyStatemets_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrivacyPage());
        }
    }
}