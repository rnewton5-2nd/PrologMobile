using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
        private static Semaphore _pool;

        public MockApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _pool = new Semaphore(1, 1);
        }

        public async Task<IEnumerable<Organization>> GetOrganizations()
        {
            string response = await SendRequest(ORGANIZATIONS_URL);
            return JsonConvert.DeserializeObject<IEnumerable<Organization>>(response);
        }

        public async Task<IEnumerable<User>> GetUsers(string organizationId)
        {
            string response = await SendRequest(string.Format(USERS_URL_TEMPLATE, organizationId));
            return JsonConvert.DeserializeObject<IEnumerable<User>>(response);
        }

        public async Task<IEnumerable<Phone>> GetPhones(string organizationId, string userId)
        {
            string response = await SendRequest(string.Format(PHONES_URL_TEMPLATE, organizationId, userId));
            return JsonConvert.DeserializeObject<IEnumerable<Phone>>(response);
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

        private async Task<string> SendRequest(string url)
        {
            // getting "Over Rate Limit" message when sending
            //   all requests as fast as possible. Here, we 
            //   check to see if a request was successful before
            //   moving on. If it wasn't, we wait one second
            //   and try again.
            try
            {
                _pool.WaitOne();
                string jsonString = "";
                bool successfulRequest = false;
                do
                {
                    var response = await _httpClient.GetAsync(url);
                    jsonString = await response.Content.ReadAsStringAsync();
                    successfulRequest = jsonString.ToLower() != "over rate limit";
                    if (!successfulRequest)
                        Thread.Sleep(1000);
                } while (!successfulRequest);
                return jsonString;
            }
            finally
            {
                _pool.Release(1);
            }
        }
    }
}
