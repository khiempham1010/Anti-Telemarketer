using AntiTelemarketer.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AntiTelemarketer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolsPage : ContentPage
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        private void BtnTelemarketer_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TelemarketerPage());
        }

        private void BtnSynchronize_Clicked(object sender, EventArgs e)
        {
            Thread t1 = new Thread((obj) => {
                try
                {

                    SynchronizeTelemarketer.SyncUpAsync();
                }
                catch (Exception)
                {

                    throw;
                }
            });


            Thread t2 = new Thread(() => {
                try
                {
                    SynchronizeTelemarketer.SyncDown();
                }
                catch { }
            });

            t1.Start();
            t2.Start();
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