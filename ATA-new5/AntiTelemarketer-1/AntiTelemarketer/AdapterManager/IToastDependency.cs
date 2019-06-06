using System;
using System.Collections.Generic;
using System.Text;

namespace AntiTelemarketer.AdapterManager
{
    public interface IToastDependency
    {
        void makeToast(String str1);
    }
}
