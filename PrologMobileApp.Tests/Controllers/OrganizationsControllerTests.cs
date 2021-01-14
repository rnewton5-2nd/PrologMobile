using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using PrologMobileApp.Web.Controllers;
using PrologMobileApp.Web.Models.DataTransfer;
using PrologMobileApp.Web.Services;
using Xunit;

namespace PrologMobileApp.Tests.Controllers
{
    public class OrganizationsControllerTests
    {
        [Fact]
        public async Task GetSummaries_should_return_list_of_OranizationSummary_objects()
        {
            var organizationsServiceMock = new Mock<IOrganizationsService>();
            organizationsServiceMock
                .Setup(o => o.GetAllSummaries())
                .ReturnsAsync(() => new List<OrganizationSummary>());

            var controller = new OrganizationsController(organizationsServiceMock.Object);

            var result = await controller.GetSummaries();

            Assert.True(result.GetType() == typeof(List<OrganizationSummary>), "GetSummaries does not return a list of OranizationSummary objects");
        }
    }
}
