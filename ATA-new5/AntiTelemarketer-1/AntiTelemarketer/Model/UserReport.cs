using System;
using SQLite;

namespace AntiTelemarketer.Model
{
    public class UserReport
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string phoneNumber { get; set; }
        public DateTime reportDate { get; set; }
        public bool IsSync { get; set; }
        public string reporterID { get; set; }
    }
}
