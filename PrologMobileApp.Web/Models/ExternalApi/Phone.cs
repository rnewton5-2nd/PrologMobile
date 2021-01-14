using System;

namespace PrologMobileApp.Web.Models.ExternalApi
{
    public class Phone
    {
        public string id;
        public string userId;
        public DateTime createdAt;
        public int IMEI;
        public bool Blacklist;
    }
}