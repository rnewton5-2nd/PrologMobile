using System;

namespace PrologMobileApp.Web.Models.ExternalApi
{
    public class User
    {
        public string id { get; set; }
        public string organizationId { get; set; }
        public DateTime createdAt { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}