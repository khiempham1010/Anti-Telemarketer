using AntiTelemarketer.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AntiTelemarketer.DatabaseHelper
{
    public class SettingDatabaseHelper
    {
        public SQLiteConnection connection { get; set; }

        public String exceptionError = String.Empty;

        public SettingDatabaseHelper()
        {
            connection = DependencyService.Get<ISQLite>().GetConnection();
            CreateTable();
   
        }

        public SettingDatabaseHelper(string FAKEONE_DELETE_THIS_TO_USE_REAL_ONE)
        {

        }

        public void CreateTable()
        {
            connection.CreateTable<Setting>();
            if(GetAllSetting().Count == 0)
            {
                try
                {
                    InsertSetting(SystemKey.IS_ENABLE, true);
                    InsertSetting(SystemKey.IS_NOTIFICATION, true);
                }
                catch
                {

                }
            }
            
        }

        //SELECT
        private List<Setting> GetAllSetting()
        {
            var settings = (from setting in connection.Table<Setting>() select setting);
            return settings.ToList();
        }


        private string InsertSetting(string name, bool value)
        {
            Setting s = new Setting();
            s._key = name;
            s._value = value;
          
            connection.Insert(s);
            return "success";
        }

        public bool GetKeyValue(string key)
        {
            foreach(Setting k in GetAllSetting())
            {
                if (k._key.Equals(key))
                    return k._value;
            }
            return false;
        }

        
        //UPDATE  
        public string UpdateSetting(string name, bool value)
        {
             connection.Table<Setting>()
            .Delete(P => P._key.Equals(name));
            InsertSetting(name, value);

            return "success";
        }
    }
}
