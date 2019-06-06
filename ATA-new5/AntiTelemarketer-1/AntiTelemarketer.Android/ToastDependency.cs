using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AntiTelemarketer.AdapterManager;

namespace AntiTelemarketer.Droid
{
   public class ToastDependency :IToastDependency
    {
        public Activity context;
        static int i = 10;

        public void makeToast(String str1)
        {
            Toast t = Toast.MakeText(context, str1, 0);
            t.Show();
            Thread.Sleep(30);
            t.Cancel();
        }

        public ToastDependency(Activity activity)
        {
            context = activity;
        }
    }

  
}