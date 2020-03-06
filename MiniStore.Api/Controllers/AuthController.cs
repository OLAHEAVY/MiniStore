using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniStore.Core.Dto;
using MiniStore.Core.Interface;
using MiniStore.Core.Services;
using MiniStore.Core.ViewModels;
using MiniStore.Data.Entities;
using MiniStore.Utility.Logger;
using Newtonsoft.Json;

namespace MiniStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authService;
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public AuthController(IAuthentication authService, ILog logger, IMapper mapper)
        {
            _authService = authService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("registeruser")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterModel model)
        {
            var response = new ApiResult<ApplicationUser>();
            try
            {
                //user to be returned
                var user = new ApplicationUser
                {
                    Address = model.Address,
                    City = model.City,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender
                };

                //calling the register customer method in the auth service
                var result = await _authService.RegisterCustomer(model);

                if (result.IdentityResult.Succeeded)
                {
                    response.Errors = null;
                    response.HasError = false;
                    response.Message = $"A Verification email has been sent to {result.User.Email}. Kindly confirm your account and setup your password.";
                    response.Result = user;
                }
                else
                {
                    response.HasError = true;
                    response.Message = result.IdentityResult.Errors.FirstOrDefault().Description;
                }

               

            }
            catch (Exception ex)
            {
                _logger.LogException($"Create User:: ${ex.Message}. Data ==>> {JsonConvert.SerializeObject(model)}", ex);
                response.HasError = true;
                response.Message = "An Error Occured";
               
            }

            return Ok(response);

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login ([FromBody] LoginModel model)
        {
            var response = new ApiResult<LoginResponseModel>();
            try
            {
                var result = await _authService.CheckValidUser(model);
                if (result.ResponseMessage.IsSuccessful)
                {
                    var token = await _authService.GenerateAccessToken(result.User);

                    var appUser = _mapper.Map<LoginDto>(result.User);


                    var data = new LoginResponseModel
                    {
                        Token = token,
                        AppUser = appUser
                        
                    };

                    response.HasError = false;
                    response.Result = data;
                    response.Message = result.ResponseMessage.Message;
                }
                else
                {
                    response.HasError = true;
                    response.Message = result.ResponseMessage.Message;
                    _logger.LogInformation($"Error with User Login:: ${result.ResponseMessage.Message}. Data ==>> {JsonConvert.SerializeObject(model.UserName)}");
                }

            }
            catch(Exception ex)
            {
                _logger.LogException($"User can not log in :: ${ex.Message}. Data ==>> {JsonConvert.SerializeObject(model)}", ex);
                response.HasError = true;
                response.Message = "An Error Occured";
                
            }

            return Ok(response);


        }

    }
}