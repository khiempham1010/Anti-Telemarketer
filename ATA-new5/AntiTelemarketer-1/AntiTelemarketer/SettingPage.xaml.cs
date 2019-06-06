using AntiTelemarketer.AdapterManager;
using AntiTelemarketer.API;
using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AntiTelemarketer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        private int hidenTool = 0;
        SettingDatabaseHelper settingDatabaseHelper;
        bool isEnable;
        bool isNotification;
        bool isWorking = false;
        public SettingPage()
        {
            InitializeComponent();
            isWorking = false;
            isEnable = false;
            isNotification = false;

            settingDatabaseHelper = new SettingDatabaseHelper();
            try
            {
                isEnable = settingDatabaseHelper.GetKeyValue(SystemKey.IS_ENABLE);
                isNotification = settingDatabaseHelper.GetKeyValue(SystemKey.IS_NOTIFICATION);
            }
            catch
            {

            }

            swNotifiaction.IsToggled = isNotification;
            sw_Enable.IsToggled = isEnable;
            isWorking = true;

        }


        private void MenuSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingPage());
        }

        private void MenuPrivacyStatemets_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrivacyPage());
        }

        public async void BtnSync_Clicked(object sender, EventArgs e)
        {

        }

        private void SwEnable_Toggled(object sender, ToggledEventArgs e)
        {
            if (isWorking)
            {
                settingDatabaseHelper.UpdateSetting(SystemKey.IS_ENABLE, !isEnable);
                isEnable = !isEnable;
            }
        }

        private void SwNotifiaction_Toggled(object sender, ToggledEventArgs e)
        {
            if (isWorking)
            {
                settingDatabaseHelper.UpdateSetting(SystemKey.IS_NOTIFICATION, !isNotification);
                isNotification = !isNotification;
            }



            if (hidenTool == 8)
            {
                Navigation.PushAsync(new ToolsPage());
                hidenTool = 0;
            }
            if (detect == null)
            {
                detect = new Thread(() =>
                {
                    while (true)
                    {
                        time++;
                        Thread.Sleep(100);
                    }
                });
                detect.Start();
            }
            if (time < 10)
            {
                hidenTool++;
                if (hidenTool >= 5)
                {
                    ToastManager.Current.makeToast((8 - hidenTool).ToString() + " more");
                }
                time = 0;
            }
            else
            {
                detect.Abort();
                detect = null;
                time = 0;
                hidenTool = 0;
            }
        }
        int time = 0;
        Thread detect = null;
    }

}

