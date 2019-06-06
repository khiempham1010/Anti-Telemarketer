using System;
using System.Collections.Generic;
using AntiTelemarketer.Model;

namespace AntiTelemarketer.AdapterManager
{
    public interface IContactService_Android
    {
        List<PhoneContact> GetAllContacts();
    }
}
