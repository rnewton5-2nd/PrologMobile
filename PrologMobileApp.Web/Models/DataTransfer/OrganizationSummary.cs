using System;
using System.Collections.Generic;

namespace PrologMobileApp.Web.Models.DataTransfer
{
    public class OrganizationSummary
    {
        public string id { get; set; }
        public string name { get; set; }
        public string blacklistTotal { get; set; }
        public string totalCount { get; set; }
        public DateTime createdAt { get; set; }
        public List<OrganizationSummaryUser> users { get; set; }
    }
}
