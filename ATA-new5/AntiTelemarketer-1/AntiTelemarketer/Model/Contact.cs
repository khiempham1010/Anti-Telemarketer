using System;
namespace AntiTelemarketer.Model
{
    public class Contact
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Duration { get; set; }
    }
}
