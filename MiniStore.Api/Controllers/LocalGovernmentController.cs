using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Core.Interface;
using MiniStore.Core.ViewModels;
using MiniStore.Utility.Logger;

namespace MiniStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalGovernmentController : ControllerBase
    {
        private readonly ILocalGovernmentService _localGovernmentService;
        private readonly ILog _logger;

        public LocalGovernmentController(ILocalGovernmentService localGovernmentService, ILog logger)
        {
            _localGovernmentService = localGovernmentService;
            _logger = logger;
        }

        [Route("getalllocalgovernments")]
        [HttpPost]
        public IActionResult GetAllLocalGovernments([FromBody] string stateId)
        {
            var response = new ApiResult<IEnumerable<SelectListItem>>();
            var localGovernments = _localGovernmentService.GetLocalGovernmentDropdownList(stateId);
            if(localGovernments.Count() != 0)
            {
                response.HasError = false;
                response.Errors = null;
                response.Message = "Local government retrieved successfully";
                response.Result = localGovernments;
            }
            else
            {
                response.HasError = true;
                response.Message = "An error occured";
            }

            return Ok(response);
        }
    }
}