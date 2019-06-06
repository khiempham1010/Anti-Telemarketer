using AntiTelemarketer.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace AntiTelemarketer.DatabaseHelper
{
    public class TelemarketerDatabaseHelper
    {
        public SQLiteConnection connection { get; set; }


        public String exceptionError = String.Empty;

        public TelemarketerDatabaseHelper()
        {

            connection = DependencyService.Get<ISQLite>().GetConnection();
            CreateTable();
        }

        public TelemarketerDatabaseHelper(string FAKEONE_DELETE_THIS_TO_USE_REAL_ONE)
        {

        }

        public void CreateTable()
        {
            connection.CreateTable<Telemarketer>();
        }

        //SELECT
        public List<Telemarketer> GetAllTelemarketer()
        {
            var telemarketers = (from telemerketer in connection.Table<Telemarketer>() select telemerketer);
            return telemarketers.ToList();
        }

        public List<Telemarketer> GetAllTelemarketer_FAKE()
        {
            List<Telemarketer> ml = new List<Telemarketer>();
            Telemarketer u1 = new Telemarketer();
            u1.phoneNumber = "0793699821";
            u1.reportDate = DateTime.Now;
            ml.Add(u1);
            return ml;

        }
        //INSERT  
        public string AddTele(Telemarketer telemarketer)
        {
            connection.Insert(telemarketer);
            return "success";
        }

        public void DeleteAll()
        {
            connection.DropTable<Telemarketer>();
            CreateTable();

        }


    }
}
