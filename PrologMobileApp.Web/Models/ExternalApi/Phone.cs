using System;

namespace PrologMobileApp.Web.Models.ExternalApi
{
    public class Phone
    {
        public string id { get; set; }
        public string userId { get; set; }
        public DateTime createdAt { get; set; }
        public int IMEI { get; set; }
        public bool Blacklist { get; set; }
    }
}