using System;
using System.Collections.Generic;
using Android.App;
using Android.Provider;
using AntiTelemarketer.AdapterManager;
using AntiTelemarketer.Model;

namespace AntiTelemarketer.Droid.ContactService
{
    public class ContactService_Android:IContactService_Android
    {
        Activity activity;

        public ContactService_Android()
        {
        }

        public ContactService_Android(Activity activity, Application androidApp)
        {
            this.activity = activity;
        }

        public List<PhoneContact> GetAllContacts()
        {
            List<PhoneContact> phoneContacts = new List<PhoneContact>();

            using (var phones = Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null))
            {
                if (phones != null)
                {
                    while (phones.MoveToNext())
                    {
                        try
                        {
                            string name = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                            string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                            string[] words = name.Split(' ');
                            var contact = new PhoneContact();
                            contact.FirstName = words[0];
                            if (words.Length > 1)
                                contact.LastName = words[1];
                            else
                                contact.LastName = ""; //no last name
                            contact.PhoneNumber = phoneNumber;
                            phoneContacts.Add(contact);
                        }
                        catch (Exception e)
                        {
                            //something wrong with one contact, may be display name is completely empty, decide what to do
                        }
                    }
                    phones.Close();
                }
                // if we get here, we can't access the contacts. Consider throwing an exception to dis play to the user
            }

            return phoneContacts;
        }
    }
}
