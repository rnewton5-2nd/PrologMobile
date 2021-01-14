using System.Collections.Generic;
using System.Threading.Tasks;
using PrologMobileApp.Web.Models.DataTransfer;

namespace PrologMobileApp.Web.Services
{
    public interface IOrganizationService
    {
        Task<IEnumerable<OrganizationSummary>> GetOrganizationsSummaries();
    }
}