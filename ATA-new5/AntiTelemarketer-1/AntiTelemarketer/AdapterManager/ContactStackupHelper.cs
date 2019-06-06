using System;
using System.Collections.Generic;
using AntiTelemarketer.Model;

namespace AntiTelemarketer.AdapterManager
{
    public class ContactStackupHelper
    {
      public List<ContactStackUp> ConvertToStackUp(List<Contact> contacts)
        {
            List<ContactStackUp> contactStackUps = new List<ContactStackUp>();

            ContactStackUp _contactStackUp;

            while(contacts.Count > 0)
            {
                _contactStackUp = new ContactStackUp();
                // FOUR BASIC COMPONENTS WHICH INHERITANCE FORM CONTACT CLASS 
                _contactStackUp.Date = contacts[0].Date;
                _contactStackUp.Duration = contacts[0].Duration;
                _contactStackUp.Number = contacts[0].Number;
                _contactStackUp.Type = contacts[0].Type;

                // COUNT TIMES AND REMOVE;
                // FIRST WE HAVE TO SET THE DEFAULT NUMBER TO ZERO
                _contactStackUp.times = 0;

                // WE NEED TO BE CAREFUL FORM HERE BECAUSE THE FUNCTION Remove() COULD BE NOT WORKING

                for(int i = 0; i<contacts.Count; i++)
                {
                    if (contacts[i].Number.Equals(_contactStackUp.Number) && contacts[i].Type.Equals(_contactStackUp.Type))
                    {
                        contacts.RemoveAt(i);
                        _contactStackUp.times++;
                        i--;
                    }
                }

                contactStackUps.Add(_contactStackUp);
            }

            return contactStackUps;
        }



    }
}
