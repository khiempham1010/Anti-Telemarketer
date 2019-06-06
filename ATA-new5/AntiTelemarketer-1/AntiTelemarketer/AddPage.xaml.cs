using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Model;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public partial class AddPage : PopupPage
    {
        public List<UserReport> Persons;
        public UserReportDatabaseHelper reportDatabaseHelper;
        public UserReport report;

        public AddPage()
        {
            InitializeComponent();

        }


        bool isPhoneNumber(String str)
        {
            if (str == null || str.Trim().Length == 0)
                return false;
            if (str.Trim().Equals(string.Empty))
                return false;
            if (str[0] == '+')
                str = str.Substring(1, str.Length - 1);
            try
            {
                Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }
         
        }


        void OnButtonClicked(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(txtSDT.Text))
                return;
            if (isPhoneNumber(txtSDT.Text))
            {
                report = new UserReport();
                reportDatabaseHelper = new UserReportDatabaseHelper();

                report.phoneNumber = txtSDT.Text;
                report.reportDate = DateTime.Now;
                report.IsSync = false;
                reportDatabaseHelper.AddReport(report);
                CloseAllPopup();
            }
            else
            {
                DisplayAlert("Error", "Wrong fortmat!!!", "OK");
            }
        }


        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }
    }
}
