using System;
using System.Collections.Generic;
using AntiTelemarketer.Model;
using Xamarin.Forms;

namespace AntiTelemarketer
{
    public partial class DetailPage : ContentPage
    {

        private List<Contact> contacts;
        private String phoneNumber = string.Empty;
        public DetailPage(List<Contact> contacts, String phoneNumber)
        {
            InitializeComponent();
            this.contacts = contacts;
            this.phoneNumber = phoneNumber;



            lb_PhoneNumber.Text = phoneNumber;


             for(int i = 0; i < contacts.Count;i++)
                {
                if (!contacts[i].Number.Equals(phoneNumber))
                    {
                    contacts.RemoveAt(i);
                    i--;
                    }
                }

            lv_CallLog.ItemsSource = contacts;
        }


    }
}
