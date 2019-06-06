using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Provider;

using AntiTelemarketer.AdapterManager;
using AntiTelemarketer.Model;

namespace AntiTelemarketer.Droid.ContactApdapter
{
    public class ContactsAdapter : IContactsAdapter
    {

        Activity activity;
        public List<Contact> contactList { get; set; }

        public List<Contact> GetCurrentsConntact()
        {
            GetAllRecentContactHistory();
            return contactList;
        }

        public ContactsAdapter(Activity activity, Application androidApp)
        {
            this.activity = activity;
            GetAllRecentContactHistory();
        }

        private double ToTime(string time)
        {
            time = time.Remove(10);
            return (((double.Parse(time) + (7 * 3600)) / 60 / 60 / 24) + 25569);
        }


        public void GetAllRecentContactHistory()
        {

            var uri = CallLog.Calls.ContentUri;

            string[] projection =
            {
                CallLog.Calls.Number,
                CallLog.Calls.Date,
                CallLog.Calls.Duration,
                CallLog.Calls.Type
            };


            var loader = new CursorLoader(activity, uri, projection, null, null, null);
            try
            {
                var cursor = (ICursor)loader.LoadInBackground();
                contactList = new List<Contact>();
                if (cursor.MoveToFirst())
                {
                    do
                    {
                        Contact contact = new Contact
                        {
                            Number = cursor.GetString(cursor.GetColumnIndex(projection[0])).ToString(),
                            Date = DateTime.FromOADate(ToTime(cursor.GetLong(cursor.GetColumnIndex(projection[1])).ToString())),
                            Duration = cursor.GetString(cursor.GetColumnIndex(projection[2])),
                            Type = cursor.GetString(cursor.GetColumnIndex(projection[3])),
                        };
                        if (contact.Type.Equals("1"))
                        {
                            contact.Type = "Incoming";
                        }
                        if (contact.Type.Equals("2"))
                        {
                            contact.Type = "Outgoing";
                        }
                        if (contact.Type.Equals("3"))
                        {
                            contact.Type = "Missed";
                        }
                        if (contact.Type.Equals("5"))
                        {
                            contact.Type = "Rejected";
                        }
                        contactList.Add(contact);
                    } while (cursor.MoveToNext());
                }
            } catch {

                Thread.Sleep(500); 
                GetAllRecentContactHistory();


            }
        }

    }
}
