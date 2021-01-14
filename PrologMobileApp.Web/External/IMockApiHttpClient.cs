using System.Collections.Generic;
using System.Threading.Tasks;
using PrologMobileApp.Web.Models.ExternalApi;

namespace PrologMobileApp.Web.External
{
    public interface IMockApiHttpClient
    {
        Task<IEnumerable<Organization>> GetOrganizations();
        Task<IEnumerable<User>> GetUsers(string organizationId);
        Task<IEnumerable<User>> GetUsers(IEnumerable<Organization> organizations);
        Task<IEnumerable<Phone>> GetPhones(string organizationId, string userId);
        Task<IEnumerable<Phone>> GetPhones(IEnumerable<User> users);
    }
}