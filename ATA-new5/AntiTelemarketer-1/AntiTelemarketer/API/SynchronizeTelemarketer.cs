using AntiTelemarketer.DatabaseHelper;
using AntiTelemarketer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AntiTelemarketer.API
{
    public class SynchronizeTelemarketer
    {
        public static List<Telemarketer> GetResultAsync()
        {
            using (var client = new HttpClient())
            {
                var content = client.GetStringAsync("https://test.primasvn.net/ata-api/Telemarketer/Action");
                var x = content.Result;
                return JsonConvert.DeserializeObject<List<Telemarketer>>(x);
            }
        }
        public static void SyncDown()
        {
            List<Telemarketer> telemarketers = SynchronizeTelemarketer.GetResultAsync();
            TelemarketerDatabaseHelper telemarketerDatabaseHelper = new TelemarketerDatabaseHelper();

            telemarketerDatabaseHelper.DeleteAll();



            foreach (Telemarketer telemarketer in telemarketers)
                telemarketerDatabaseHelper.AddTele(telemarketer);
        }

        public static async void SyncUpAsync()
        {
            UserReportDatabaseHelper reportDatabaseHelper = new UserReportDatabaseHelper();

            List<UserReport> userReports = reportDatabaseHelper.GetAllReport();

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://test.primasvn.net/ata-api/UserReport/Action");
            foreach (UserReport user in userReports)
            {

                if (!user.IsSync)
                {
                    string jsonData = "{\"PhoneNumber\" : \"" + user.phoneNumber + "\",\"ReportDate\":\"" + user.reportDate.Month + "/"+ user.reportDate.Day + "/"+user.reportDate.Year+" "+user.reportDate.Hour+":" + user.reportDate.Minute + ":" + user.reportDate.Second + "\", \"DeleteFlag\":1 ,\"ReporterId\":" + user.reporterID + "}";


                    try
                    {
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PostAsync("", content);
                        var result = await response.Content.ReadAsStringAsync();
                        
                        reportDatabaseHelper.UpdateFlag(user);
                    }
                    catch (Exception er)
                    {

                    }
                }
            }
        }
    }
}
