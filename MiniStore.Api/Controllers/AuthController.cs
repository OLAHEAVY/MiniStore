using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthentication authService, ILog logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
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
                    response.Message = $"A Verification email has been sent to {result.User.Email}.";
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
                    _logger.LogInformation($"Error with User Login:: ${result.ResponseMessage.Message}. Data ==>> {JsonConvert.SerializeObject(model.Email)}");
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

        //controller method to confirm account
        [HttpGet]
        [Route("confirmaccount")]
        public async Task<IActionResult> ConfirmAccount(string token)
        {
            var response = new ApiResult<string>();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var confirm = await _authService.ConfirmAccount(token);
                    if (confirm.IdentityResult.Succeeded)
                    {
                        response.HasError = false;
                        response.Message = "Your email " + confirm.User.Email + "has been confirmed";
                    }
                    else
                    {
                        response.HasError = true;
                        response.Message = confirm.IdentityResult.Errors.FirstOrDefault().Description;
                        
                    }

                }

            }
            catch(Exception ex)
            {
                _logger.LogException("Email could not be confirmed due to an error" + ex.Message, ex);
                response.HasError = true;
                response.Message = "An error occured";
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("resendLink")]
        public async Task<IActionResult> ResendConfirmationLink(ResendConfirmModel model)
        {
            var response = new ApiResult<string>();
            try
            {
               
                var user = await _userManager.FindByNameAsync(model.Email);

                if(user == null)
                {
                    response.HasError = true;
                    response.Message = "Invalid Request";
                }
                else
                {
                    if (user.EmailConfirmed)
                    {
                        response.HasError = false;
                        response.Message = "Email has already been Confirmed. Please login";
                    }
                    else
                    {
                        var sendLinkAgain = await _authService.SendVerificationLink(user);

                        if (sendLinkAgain)
                        {
                            response.HasError = false;
                            response.Message = "Email Confirmation Link sent successfully";
                        }
                        else
                        {
                            response.HasError = true;
                            response.Message = "Email Confirmation Link could not be sent";
                        }
                    }

                }


            }
            catch(Exception ex)
            {
                _logger.LogException("Error sending email confirmation:" + ex.Message, ex);
                response.HasError = true;
                response.Message = "An error occuured";
            }

            return Ok(response);
            
        }

        [HttpPost]
        [Route("sendpasswordresetlink")]
        public async Task<IActionResult> SendPasswordResetLink(ResendConfirmModel model)
        {
            var response = new ApiResult<string>();
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    response.HasError = true;
                    response.Message = "Invalid Request";
                }
                else
                {
                    var sendPasswordResetLink = await _authService.SendResetPasswordLink(user);

                    if (sendPasswordResetLink)
                    {
                        response.HasError = false;
                        response.Message = "Password Reset Link sent to your mail";
                    }
                    else
                    {
                        response.HasError = true;
                        response.Message = "Password Reset Link could not be sent";
                    }
                }

            } catch (Exception ex)
            {
                _logger.LogException("Error sending password confirmation link" + ex.Message, ex);
                response.HasError = true;
                response.Message = "An error occurred";
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("confirmpasswordresetcode")]
        public async Task<IActionResult> ConfirmPaswordResetCode(string token)
        {
            var response = new ApiResult<string>();
            try
            {
                var result = await _authService.ResetPasword(token);

                if(result.User != null)
                {
                    response.HasError = false;
                    response.Message = "Please enter your new password";
                    response.Result = result.User.Email;
                }
                else
                {
                    response.HasError = true;
                    response.Message = "Invalid request!";
                    
                }
               

            }catch(Exception ex)
            {
                _logger.LogException("An exception occured : " + ex.Message, ex);
                response.HasError = true;
                response.Message = "An error occurred";
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetModel model)
        {
            var response = new ApiResult<string>();

            if (string.IsNullOrEmpty(model.Token))
                return BadRequest();

            try
            {
                var reset = await _authService.PerformPasswordChange(model);
                if (reset.IsSuccessful)
                {
                    response.HasError = false;
                    response.Message = "Successful! Kindly proceed to login";
                }
                else
                {
                    response.HasError = true;
                    response.Message = reset.Message;
                }

               
            }catch(Exception ex)
            {
                _logger.LogException($"User Account Confirmation Failed::" + ex.Message, ex);
            }

            return Ok(response);
        }
        
    }

    
}