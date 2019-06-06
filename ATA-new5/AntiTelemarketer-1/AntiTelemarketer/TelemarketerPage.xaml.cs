using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AntiTelemarketer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TelemarketerPage : ContentPage
    {
        public TelemarketerDatabaseHelper telemarketerDatabaseHelper;
        public Telemarketer telemarketer;
        public List<Telemarketer> telemarketers;

        public TelemarketerPage()
        {
            InitializeComponent();
            LoadData();
            
        }


        private void LoadData()
        {
            try
            {
                telemarketerDatabaseHelper = new TelemarketerDatabaseHelper();
                var teles = telemarketerDatabaseHelper.GetAllTelemarketer();
                lv_Telemarketer.ItemsSource = teles;
            }
            catch { }

        }
    }
}
