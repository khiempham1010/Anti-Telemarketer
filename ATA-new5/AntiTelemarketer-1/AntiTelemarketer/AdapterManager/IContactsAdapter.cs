using System.Collections.Generic;
using AntiTelemarketer.Model;

namespace AntiTelemarketer.AdapterManager
{
    public interface IContactsAdapter
    {
        void GetAllRecentContactHistory();
        List<Contact> GetCurrentsConntact();
    }
}
