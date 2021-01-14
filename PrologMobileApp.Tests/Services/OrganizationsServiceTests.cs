using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PrologMobileApp.Web.Controllers;
using PrologMobileApp.Web.External;
using PrologMobileApp.Web.Models.DataTransfer;
using PrologMobileApp.Web.Models.ExternalApi;
using PrologMobileApp.Web.Services;
using Xunit;

namespace PrologMobileApp.Tests.Services
{
    public class OrganizationsServiceTests
    {
        [Fact]
        public async Task GetAllSummaries_returns_a_list_of_organization_summaries()
        {
            var mockApiHttpClientMock = new Mock<IMockApiHttpClient>();

            List<Organization> organizations = new List<Organization>();
            List<User> users = new List<User>();
            List<Phone> phones = new List<Phone>();

            mockApiHttpClientMock
                .Setup(o => o.GetOrganizations())
                .ReturnsAsync(() => new List<Organization>());
            mockApiHttpClientMock
                .Setup(o => o.GetUsers(organizations))
                .ReturnsAsync(() => new List<User>());
            mockApiHttpClientMock
                .Setup(o => o.GetPhones(users))
                .ReturnsAsync(() => new List<Phone>());

            var service = new OrganizationService(mockApiHttpClientMock.Object);

            var result = (await service.GetAllSummaries()).ToList();

            Assert.True(result.GetType() == typeof(List<OrganizationSummary>), "GetAllSummaries does not return a list of OranizationSummary objects");
        }

        [Fact]
        public async Task GetAllSummaries_formats_organization_summaries_correctly()
        {
            var mockApiHttpClientMock = new Mock<IMockApiHttpClient>();

            List<Organization> organizations = new List<Organization>
            {
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
                }
            };
            List<User> users = new List<User> {
                new User
                {
                    id = "1",
                    createdAt = new DateTime(2020, 3, 15),
                    name = "John Doe",
                    organizationId = "1",
                    email = "user1@example.com"
                },
                new User
                {
                    id = "2",
                    createdAt = new DateTime(2015, 8, 10),
                    name = "Jane Doe",
                    organizationId = "2",
                    email = "user2@example.com"
                },
                new User
                {
                    id = "3",
                    createdAt = new DateTime(2011, 8, 10),
                    name = "Brian Doe",
                    organizationId = "2",
                    email = "user3@example.com"
                },
            };
            List<Phone> phones = new List<Phone>
            {
                new Phone
                {
                    id = "1",
                    createdAt = new DateTime(2020, 3, 15),
                    Blacklist = true,
                    IMEI = 1234,
                    userId = "1"
                },
                new Phone
                {
                    id = "2",
                    createdAt = new DateTime(2015, 8, 10),
                    Blacklist = false,
                    IMEI = 4321,
                    userId = "1"
                },
                new Phone
                {
                    id = "3",
                    createdAt = new DateTime(2012, 8, 10),
                    Blacklist = true,
                    IMEI = 4321,
                    userId = "2"
                },
                new Phone
                {
                    id = "4",
                    createdAt = new DateTime(2010, 8, 10),
                    Blacklist = true,
                    IMEI = 4321,
                    userId = "2"
                },
                new Phone
                {
                    id = "5",
                    createdAt = new DateTime(2010, 8, 10),
                    Blacklist = false,
                    IMEI = 4321,
                    userId = "3"
                },
            };

            mockApiHttpClientMock
                .Setup(o => o.GetOrganizations())
                    .ReturnsAsync(() => organizations);
            mockApiHttpClientMock
                .Setup(o => o.GetUsers(organizations))
                .ReturnsAsync(() => users);
            mockApiHttpClientMock
                .Setup(o => o.GetPhones(users))
                .ReturnsAsync(() => phones);

            var service = new OrganizationService(mockApiHttpClientMock.Object);

            var result = (await service.GetAllSummaries()).ToList();

            Assert.Equal(2, result.Count);

            Assert.Equal("1", result[0].id);
            Assert.Equal("demo org 1", result[0].name);
            Assert.Equal("1", result[0].blacklistTotal);
            Assert.Equal("2", result[0].totalCount);
            Assert.True(DateTime.Equals(new DateTime(2020, 3, 15), result[0].createdAt));
            Assert.Equal(1, result[0].users.Count);
            Assert.Equal("John Doe", result[0].users[0].name);

            Assert.Equal("2", result[1].id);
            Assert.Equal("demo org 2", result[1].name);
            Assert.Equal("2", result[1].blacklistTotal);
            Assert.Equal("3", result[1].totalCount);
            Assert.True(DateTime.Equals(new DateTime(2015, 8, 10), result[1].createdAt));
            Assert.Equal(2, result[1].users.Count);
            Assert.Equal("Jane Doe", result[1].users[0].name);
        }
    }
}
