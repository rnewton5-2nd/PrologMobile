using System.Collections.Generic;
using System.Threading.Tasks;
using PrologMobileApp.Web.Models.DataTransfer;

namespace PrologMobileApp.Web.Services
{
    public interface IOrganizationsService
    {
        Task<IEnumerable<OrganizationSummary>> GetAllSummaries();
    }
}