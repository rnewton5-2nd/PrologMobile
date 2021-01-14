using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrologMobileApp.Web.Models.DataTransfer;
using PrologMobileApp.Web.Services;

namespace PrologMobileApp.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationsService _organizationService;

        public OrganizationsController(IOrganizationsService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        [Route("summaries")]
        public async Task<IEnumerable<OrganizationSummary>> GetSummaries()
        {
            return await _organizationService.GetAllSummaries();
        }
    }
}
