using System;
using System.Collections.Generic;

namespace PrologMobileApp.Web.Models.DataTransfer
{
    public class OrganizationSummary
    {
        public string id;
        public string name;
        public string blacklistTotal;
        public string totalCount;
        public DateTime createdAt;
        public List<OrganizationSummaryUser> users;
    }
}
