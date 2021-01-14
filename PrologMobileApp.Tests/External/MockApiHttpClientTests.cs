using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using System.Text.Json;
using PrologMobileApp.Web.External;
using PrologMobileApp.Web.Models.ExternalApi;
using Xunit;

namespace PrologMobileApp.Tests.External
{
    public class MockApiHttpClientTests
    {
        [Fact]
        public async Task GetOrganizations_deserializes_a_list_of_organizations()
        {
            Organization testOrg1 = new Organization
            {
                id = "1",
                createdAt = new DateTime(2020, 3, 15),
                name = "demo org 1"
            };
            Organization testOrg2 = new Organization
            {
                id = "2",
                createdAt = new DateTime(2015, 8, 10),
                name = "demo org 2"
            };
            List<Organization> organizations = new List<Organization>
            {
                testOrg1,
                testOrg2,
            };

            string payload = JsonSerializer.Serialize(organizations);
            var mockHttpClient = CreateMockHttpClient(payload);
            var mockApiHttpClient = new MockApiHttpClient(mockHttpClient);

            var result = (await mockApiHttpClient.GetOrganizations()).ToList();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.True(OrganizationObjectsAreEqual(testOrg1, result[0]), "Organization objects are not equal");
            Assert.True(OrganizationObjectsAreEqual(testOrg2, result[1]), "Organization objects are not equal");
        }

        [Fact]
        public async Task GetUsers_with_organization_id_deserializes_a_list_of_users()
        {
            User testUser1 = new User
            {
                id = "1",
                createdAt = new DateTime(2020, 3, 15),
                name = "John Doe",
                organizationId = "123",
                email = "user1@example.com"
            };
            User testUser2 = new User
            {
                id = "1",
                createdAt = new DateTime(2015, 8, 10),
                name = "Jane Doe",
                organizationId = "321",
                email = "user2@example.com"
            };
            List<User> users = new List<User>
            {
                testUser1,
                testUser2,
            };

            string payload = JsonSerializer.Serialize(users);
            var mockHttpClient = CreateMockHttpClient(payload);
            var mockApiHttpClient = new MockApiHttpClient(mockHttpClient);

            var result = (await mockApiHttpClient.GetUsers("1")).ToList();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.True(UserObjectsAreEqual(testUser1, result[0]), "User objects are not equal");
            Assert.True(UserObjectsAreEqual(testUser2, result[1]), "User objects are not equal");
        }

        [Fact]
        public async Task GetUsers_with_organizations_deserializes_a_list_of_users()
        {
            User testUser = new User
            {
                id = "1",
                createdAt = new DateTime(2020, 3, 15),
                name = "John Doe",
                organizationId = "123",
                email = "user1@example.com"
            };
            List<User> users = new List<User>
            {
                testUser,
            };

            string payload = JsonSerializer.Serialize(users);
            var mockHttpClient = CreateMockHttpClient(payload);
            var mockApiHttpClient = new MockApiHttpClient(mockHttpClient);

            var organizations = new List<Organization> {
                new Organization
                {
                    id = "1",
                    createdAt = new DateTime(2020, 3, 15),
                    name = "demo org 1"
                },
                new Organization
                {
                    id = "2",
                    createdAt = new DateTime(2015, 8, 10),
                    name = "demo org 2"
                },
            };

            var result = (await mockApiHttpClient.GetUsers(organizations)).ToList();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.True(UserObjectsAreEqual(testUser, result[0]), "User objects are not equal");
            Assert.True(UserObjectsAreEqual(testUser, result[1]), "User objects are not equal");
        }

        [Fact]
        public async Task GetPhones_with_organization_id_and_user_id_deserializes_a_list_of_phones()
        {
            Phone testPhone1 = new Phone
            {
                id = "1",
                createdAt = new DateTime(2020, 3, 15),
                Blacklist = true,
                IMEI = 1234,
                userId = "1"
            };
            Phone testPhone2 = new Phone
            {
                id = "2",
                createdAt = new DateTime(2015, 8, 10),
                Blacklist = false,
                IMEI = 4321,
                userId = "1"
            };
            List<Phone> phones = new List<Phone>
            {
                testPhone1,
                testPhone2,
            };

            string payload = JsonSerializer.Serialize(phones);
            var mockHttpClient = CreateMockHttpClient(payload);
            var mockApiHttpClient = new MockApiHttpClient(mockHttpClient);

            var result = (await mockApiHttpClient.GetPhones("1", "1")).ToList();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.True(PhoneObjectsAreEqual(testPhone1, result[0]), "Phone objects are not equal");
            Assert.True(PhoneObjectsAreEqual(testPhone2, result[1]), "Phone objects are not equal");
        }

        [Fact]
        public async Task GetPhones_with_users_a_list_of_phones()
        {
            Phone testPhone1 = new Phone
            {
                id = "1",
                createdAt = new DateTime(2020, 3, 15),
                Blacklist = true,
                IMEI = 1234,
                userId = "1"
            };
            List<Phone> phones = new List<Phone>
            {
                testPhone1,
            };

            string payload = JsonSerializer.Serialize(phones);
            var mockHttpClient = CreateMockHttpClient(payload);
            var mockApiHttpClient = new MockApiHttpClient(mockHttpClient);

            var users = new List<User> {
                new User
                {
                    id = "1",
                    createdAt = new DateTime(2020, 3, 15),
                    name = "John Doe",
                    organizationId = "123",
                    email = "user1@example.com"
                },
                new User
                {
                    id = "1",
                    createdAt = new DateTime(2015, 8, 10),
                    name = "Jane Doe",
                    organizationId = "321",
                    email = "user2@example.com"
                }
            };

            var result = (await mockApiHttpClient.GetPhones(users)).ToList();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
            Assert.True(PhoneObjectsAreEqual(testPhone1, result[0]), "User objects are not equal");
            Assert.True(PhoneObjectsAreEqual(testPhone1, result[1]), "User objects are not equal");
        }

        private bool OrganizationObjectsAreEqual(Organization expected, Organization result)
        {
            return expected.id == result.id &&
                expected.name == result.name &&
                expected.createdAt == result.createdAt;
        }

        private bool UserObjectsAreEqual(User expected, User result)
        {
            return expected.id == result.id &&
                expected.name == result.name &&
                expected.createdAt == result.createdAt &&
                expected.email == result.email &&
                expected.organizationId == result.organizationId;
        }

        private bool PhoneObjectsAreEqual(Phone expected, Phone result)
        {
            return expected.id == result.id &&
                expected.Blacklist == result.Blacklist &&
                expected.createdAt == result.createdAt &&
                expected.IMEI == result.IMEI &&
                expected.userId == result.userId;
        }

        private HttpClient CreateMockHttpClient(string payload)
        {
            return new HttpClient(new HttpMessageHandlerStub(async (request, cancellationToken) =>
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(payload)
                };

                return await Task.FromResult(responseMessage);
            }));
        }
    }
}
