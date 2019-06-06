using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntiTelemarketer.Model
{
   public  class Telemarketer
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string phoneNumber { get; set; }

        public DateTime reportDate { get; set; }
    }
}
