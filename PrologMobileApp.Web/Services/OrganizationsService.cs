using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrologMobileApp.Web.External;
using PrologMobileApp.Web.Models.DataTransfer;
using PrologMobileApp.Web.Models.ExternalApi;

namespace PrologMobileApp.Web.Services
{
    public class OrganizationService : IOrganizationsService
    {
        private readonly IMockApiHttpClient _mockApiHttpClient;

        public OrganizationService(IMockApiHttpClient mockApiHttpClient)
        {
            _mockApiHttpClient = mockApiHttpClient;
        }

        public async Task<IEnumerable<OrganizationSummary>> GetAllSummaries()
        {
            var organizations = (await _mockApiHttpClient.GetOrganizations()).ToList();
            var users = (await _mockApiHttpClient.GetUsers(organizations)).ToList();
            var phones = (await _mockApiHttpClient.GetPhones(users)).ToList();

            return organizations.Select(org => new OrganizationSummary
            {
                id = org.id,
                name = org.name,
                createdAt = org.createdAt,
                blacklistTotal = GetBlacklistPhoneTotal(org.id, users, phones),
                totalCount = GetPhoneTotal(org.id, users, phones),
                users = GetOrganizationUserSummaries(org.id, users, phones),
            });
        }

        private string GetBlacklistPhoneTotal(string organizationId, List<User> users, List<Phone> phones)
        {
            List<User> orgUsers = GetOrganizationUsers(organizationId, users);
            return phones
                .Where(phone => phone.Blacklist && orgUsers.Any(user => user.id == phone.userId))
                .Count()
                .ToString();
        }

        private string GetPhoneTotal(string organizationId, List<User> users, List<Phone> phones)
        {
            List<User> orgUsers = GetOrganizationUsers(organizationId, users);
            return phones
                .Where(phone => orgUsers.Any(user => user.id == phone.userId))
                .Count()
                .ToString();
        }

        private List<OrganizationSummaryUser> GetOrganizationUserSummaries(string organizationId, List<User> users, List<Phone> phones)
        {
            List<User> orgUsers = GetOrganizationUsers(organizationId, users);
            return orgUsers.Select(user => new OrganizationSummaryUser
            {
                id = user.id,
                email = user.email,
                name = user.name,
                phoneCount = phones.Where(phone => phone.userId == user.id).Count()
            }).ToList();
        }

        private List<User> GetOrganizationUsers(string organizationId, List<User> users)
        {
            return users.Where(user => user.organizationId == organizationId).ToList();
        }
    }
}