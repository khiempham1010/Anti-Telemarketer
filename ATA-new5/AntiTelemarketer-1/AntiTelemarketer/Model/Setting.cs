using SQLite;
using System;
namespace AntiTelemarketer.Model
{
    public class Setting
    {
        [PrimaryKey]
        public string _key { get; set; }
        public bool _value { get; set; } 

        }
}
