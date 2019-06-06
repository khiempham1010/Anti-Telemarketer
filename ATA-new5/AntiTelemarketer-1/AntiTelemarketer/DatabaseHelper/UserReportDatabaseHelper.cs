using System;
using System.Collections.Generic;
using System.Linq;
using AntiTelemarketer.Model;
using SQLite;
using Xamarin.Forms;

namespace AntiTelemarketer.DatabaseHelper
{
    public class UserReportDatabaseHelper
    {
        public SQLiteConnection connection { get; set; }


        public String exceptionError = String.Empty;

        public UserReportDatabaseHelper()
            {
          
                connection = DependencyService.Get<ISQLite>().GetConnection();
                CreateTable();
            }

        public UserReportDatabaseHelper(string FAKEONE_DELETE_THIS_TO_USE_REAL_ONE)
        {

        }

        public void CreateTable()
        {
            connection.CreateTable<UserReport>();
        }

        //SELECT
        public List<UserReport> GetAllReport()
        {
            var reports = (from report in connection.Table<UserReport>() select report);
            return reports.ToList();
        }

        public List<UserReport> GetAllReport_FAKE()
        {
            List<UserReport> ml = new List<UserReport>();
            UserReport u1 = new UserReport();
            u1.phoneNumber = "0984182205";
            u1.reportDate = DateTime.Now;
            ml.Add(u1);
            return ml;

        }
        //INSERT  
        public string AddReport(UserReport report)
        {
            report.reporterID = "1";
            connection.Insert(report);
            return "success";
        }
        //DELETE  
        public string DeleteReportByPhoneNumber(string phonenNumber)
        {
            connection.Table<UserReport>()
            .Delete(P => P.phoneNumber.Equals(phonenNumber));
            return "success";
        }

        public string DeleteReportByID(int id)
        {
            connection.Table<UserReport>()
            .Delete(P => P.id.Equals(id));
            return "success";
        }

        //UPDATE  
        public string UpdateFlag(UserReport userReport)
        {
            DeleteReportByID(userReport.id);
            userReport.IsSync = true;
            AddReport(userReport);
           
            return "success";
        }


    }
}
