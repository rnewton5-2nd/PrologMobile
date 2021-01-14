using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PrologMobileApp.Web.Models.ExternalApi;

namespace PrologMobileApp.Web.External
{
    public class MockApiHttpClient : IMockApiHttpClient
    {
        public const string ORGANIZATIONS_URL = "https://5f0ddbee704cdf0016eaea16.mockapi.io/organizations";
        public const string USERS_URL_TEMPLATE = "https://5f0ddbee704cdf0016eaea16.mockapi.io/organizations/{0}/users";
        public const string PHONES_URL_TEMPLATE = "https://5f0ddbee704cdf0016eaea16.mockapi.io/organizations/{0}/users/{1}/phones";

        private readonly HttpClient _httpClient;

        public MockApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            var response = await _httpClient.GetAsync(ORGANIZATIONS_URL);
            string jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Organization>>(jsonString);
        }

        public async Task<IEnumerable<User>> GetUsers(string organizationId)
        {
            var response = await _httpClient.GetAsync(string.Format(USERS_URL_TEMPLATE, organizationId));
            string jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<User>>(jsonString);
        }

        public async Task<IEnumerable<Phone>> GetPhones(string organizationId, string userId)
        {
            var response = await _httpClient.GetAsync(string.Format(PHONES_URL_TEMPLATE, organizationId, userId));
            string jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Phone>>(jsonString);
        }

        public async Task<IEnumerable<User>> GetUsers(IEnumerable<Organization> organizations)
        {
            var tasks = organizations.Select(org => GetUsers(org.id)).ToArray();
            var results = await Task.WhenAll(tasks);
            return results.SelectMany(result => result);
        }

        public async Task<IEnumerable<Phone>> GetPhones(IEnumerable<User> users)
        {
            var tasks = users.Select(user => GetPhones(user.organizationId, user.id));
            var results = await Task.WhenAll(tasks);
            return results.SelectMany(result => result);
        }
    }
}
