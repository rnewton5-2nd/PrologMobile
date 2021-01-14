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
        private readonly ILogger<OrganizationsController> _logger;
        private readonly IOrganizationService _organizationService;

        public OrganizationsController(ILogger<OrganizationsController> logger, IOrganizationService organizationService)
        {
            _logger = logger;
            _organizationService = organizationService;
        }

        [HttpGet]
        [Route("summaries")]
        public async Task<IEnumerable<OrganizationSummary>> Get()
        {
            return await _organizationService.GetOrganizationsSummaries();
        }
    }
}
