using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Model;
using AntiTelemarketer.AdapterManager;
using Rg.Plugins.Popup.Services;
using System.Threading;

namespace AntiTelemarketer
{
    public partial class MainPage : TabbedPage
    {
        #region Declare 
        public List<UserReport> listReport;
        public UserReportDatabaseHelper reportDatabaseHelper;
        public UserReport report;
        public List<Contact> listContacts;
        public ContactStackupHelper contactStackupHelper;
        #endregion

        public MainPage()
        {
            InitializeComponent();

            // LOAD DATA CHO LIST MYREPORT
            
            LoadData();

            #region lv_UserReport


            // HÀNH ĐỘNG NHẬN ITEM ĐÃ TOUCH VÀO
            lv_UserReport.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {
                var item = (UserReport)e.SelectedItem;
                report = item;
            };

            // HÀNH ĐỘNG REFRESH
            lv_UserReport.RefreshCommand = new Command(() =>
            {
                LoadData();
                lv_UserReport.IsRefreshing = false;
            });
            #endregion


            #region lv_CallLog
            lv_CallLog.RefreshCommand = new Command(() =>
            {
                LoadData();
                lv_CallLog.IsRefreshing = false;
            });
            #endregion
        }




        #region ActionOnMainAcitivity


        // HÀNH ĐỘNG NHẤN PHÍM REPORT
        public async void OnReportAsync(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            bool answer = await DisplayAlert("Confirm", "Do you want to report " + mi.CommandParameter.ToString() + " ?", "Yes", "No");
            if (answer)
            {
                report = new UserReport();
                report.phoneNumber = mi.CommandParameter.ToString();
                report.reportDate = DateTime.Now;
                report.IsSync = false;
                reportDatabaseHelper.AddReport(report);
                LoadData();
            }
        }

        // HÀNH ĐỘNG NHẤN PHÍM DELETE
        public async void OnDeleteAsync(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            bool tl = await DisplayAlert("Confirm", "Do you want to delete " + mi.CommandParameter.ToString() + " ?", "Yes", "No");

            if (tl)
            {
                reportDatabaseHelper.DeleteReportByPhoneNumber(mi.CommandParameter.ToString().Trim());
                LoadData();
            }

        }

        // LẤY LẠI DỮ LIỆU TẠI DB
        private void LoadData()
        {
                try
                {
                    reportDatabaseHelper = new UserReportDatabaseHelper();

                    var users = reportDatabaseHelper.GetAllReport();
                    lv_UserReport.ItemsSource = users;
              
                }
                catch {
          

                }
  
            try
            {
                //LẤY DANH BẠ
                List<PhoneContact> phoneContacts = ContactService_AndroidManager.Current.GetAllContacts();
                contactStackupHelper = new ContactStackupHelper();
                List<Contact> list = ContactsAdapterManager.Current.GetCurrentsConntact();
                list.Reverse();

                //XÓA NHỮNG SỐ THUỘC DANH BẠ RA KHỎI LỊCH SỬ
                //CỰC KỲ LƯU Ý VIỆC XỬ LÝ ĐẦU SỐ
                for(int i = 0; i < phoneContacts.Count; i++)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if(phoneContacts[i].PhoneNumber.Replace(" ", string.Empty).Replace("-", string.Empty).Equals(list[j].Number))
                        {
                            list.RemoveAt(j);
                            j--;
                        }
                    }
                }

                lv_CallLog.ItemsSource = contactStackupHelper.ConvertToStackUp(list);
            }
            catch (Exception ex)
             {
                Thread.Sleep(500);
                LoadData();
            }
        }


        async void Handle_ItemTappedAsync(object sender, Xamarin.Forms.ItemTappedEventArgs e)

        {
            // GET SELECTED ITEM
            ListView listView = (ListView)sender;
            ContactStackUp contactStackUp = (ContactStackUp)listView.SelectedItem;
            List<Contact> list = ContactsAdapterManager.Current.GetCurrentsConntact();
            list.Reverse();
            await Navigation.PushAsync(new DetailPage(list,contactStackUp.Number));


        }

        #endregion

        private void MenuSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingPage());
        }

        private void MenuPrivacyStatemets_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrivacyPage());
        }

        private AddPage addPage;
        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            addPage = new AddPage();
            PopupNavigation.Instance.PushAsync(addPage);
            LoadData();
        }
        
    }
}
