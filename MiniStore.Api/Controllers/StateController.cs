using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Core.Interface;
using MiniStore.Core.ViewModels;
using MiniStore.Data.Entities;
using MiniStore.Data.Repository.IRepository;
using MiniStore.Utility.Logger;

namespace MiniStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly ILog _logger;

        public StateController(IStateService stateService,ILog logger)
        {
            _stateService = stateService;
            _logger = logger;
        }

        [Route("getallstates")]
        [HttpGet]
        public IActionResult GetAllStates()
        {
            var response = new ApiResult<IEnumerable<SelectListItem>>();
            var states = _stateService.GetAllStatesDropDown();

            if(states.Count() != 0)
            {
                response.HasError = false;
                response.Errors = null;
                response.Message = "Sucessfully retrieved State from the Database";
                response.Result = states;
            }
            else
            {
                response.HasError = true;
                response.Message = "An error occurred";
               
            }

           return Ok(response);
        }


    }
}