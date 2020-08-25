using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MiniStore.Core.Helpers;
using MiniStore.Core.Interface;
using MiniStore.Core.Security;
using MiniStore.Core.ViewModels;
using MiniStore.Data.Entities;
using MiniStore.Utility.Logger;
using MiniStore.Utility.StaticData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Core.Services
{
    public class AuthenticationService : IAuthentication
    {
        private readonly JwtSettings _jwtSettings;
        private readonly EncryptionService _encryptionService;
        private readonly IEnvironmentalVariables _environmentalVariables;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILog _logger;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;
        private readonly ICommonHelper _commonHelper;
        private readonly IMapper _mapper;


        public AuthenticationService(JwtSettings jwtSettings, SignInManager<ApplicationUser> signInManager, EncryptionService encryptionService, IEnvironmentalVariables environmentalVariables, UserManager<ApplicationUser> userManager, ILog logger,ICommonHelper commonHelper, IEmailService emailService, IWebHostEnvironment env, RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _env = env;
            _jwtSettings = jwtSettings;
            _encryptionService = encryptionService;
            _environmentalVariables = environmentalVariables;
            _userManager = userManager;
            _logger = logger;
            _emailService = emailService;
            _roleManager = roleManager;
            _commonHelper = commonHelper;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<UserAccountModel> RegisterCustomer(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                Address = model.Address,
                City = model.City,
                State = model.State,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Gender = model.Gender,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password. Email ==> " + user.Email + ", Name ==> " + user.FirstName +" " + user.LastName);

                //creating and assigning roles => customer
                await CreateRoles(user, StaticRole.CustomerUser);
                _logger.LogInformation(StaticRole.CustomerUser + "is assigned to the user" + user.Email);

                //send confirmation email to the created User
               if( await SendVerificationLink(user))
               {
                    _logger.LogInformation("Email confirmation sent to the registered email" + user.Email);
                }
                else
                {
                    _logger.LogInformation("Email confirmation not sent to the registered email. An error occured." + user.Email);
                }
               
              

                return new UserAccountModel { IdentityResult = result, User = user };

            }
            else
            {
                _logger.LogInformation("An error occurred while trying to register the user" + result.ToString());
                return new UserAccountModel { User = null, IdentityResult = result };
               
            }
        }


        public async Task<bool> SendVerificationLink(ApplicationUser user)
        {
            //generating the email confirmation token
            var activationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var activationUrlCode = _encryptionService.UrlEncodedEncryptText(JsonConvert.SerializeObject(new AccountVerificationModel
            {
                Code = activationCode,
                UserId = user.Id
            }));

            //the redirect url to be sent to the mail.
            var activateUrl = string.Format(_environmentalVariables.ClientUrl + "{0}/{1}?token={2}", "authentication", "confirm-account", activationUrlCode);
            System.IO.File.WriteAllText("emailconfirm.txt", activateUrl);

            string Message = "Please confirm your account";

            var subject = "Confirm Account Registration";

            //get webroot path 

            var webroot = _env.WebRootPath;

            //Get TemplateFile located at wwwroot/templates/ConfirmAccount.html
            var pathToFile = webroot
                         + Path.DirectorySeparatorChar.ToString()
                         + "templates"
                         + Path.DirectorySeparatorChar.ToString()
                         + "ConfirmAccount.html";

            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }
            //{0} : Subject
            //{1}:  DateTime
            //{2}:  Name
            //{3}: Message
            //{4}: callbackUrl

            string messageBody = string.Format(builder.HtmlBody,
                subject,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                user.FirstName,
                Message,
                activateUrl
                );

            await _emailService.SendEmailAsync(user.Email, subject, messageBody);

            return true;

        }

        public async Task CreateRoles(ApplicationUser user, string applicationRole)
        {
                var roles = new List<ApplicationRole>
                {
                    new ApplicationRole{Name = StaticRole.CustomerUser},
                    new ApplicationRole{Name = StaticRole.ManagerUser},
                    new ApplicationRole{Name = StaticRole.StaffUser},
                 
                };

            foreach(var role in roles)
            {
                //checking if the roles already exist and creating them
                if(!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    _roleManager.CreateAsync(role).Wait();
                }

              
            }

            await _userManager.AddToRoleAsync(user, applicationRole);

        }

        public async Task<UserResponseModel> CheckValidUser(LoginModel model)
        {
            //checking if the user exists on the platform.
            var user =await  _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                //the user does not exist on the platform
                _logger.LogInformation(model.Email + "does not exist on the platform");
                return new UserResponseModel { ResponseMessage = _commonHelper.OutputMessage(false, "UserName or Password is Invalid") };
            }
            else
            {
                //attempting to sign the user in with the password
                var result =await  _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return new UserResponseModel { ResponseMessage = _commonHelper.OutputMessage(true, "User Logged in Successfully"), User = user };

                }
                if(result.IsLockedOut)
                {
                    _logger.LogInformation("User is Locked out.");
                    return new UserResponseModel { ResponseMessage = _commonHelper.OutputMessage(false, "Account is now lockedout due maximum failed attempts being reached. Contact the admin") };

                }
                if (result.IsNotAllowed)
                {
                    _logger.LogInformation("User is Not Allowed to log in.");
                    return new UserResponseModel { ResponseMessage = _commonHelper.OutputMessage(false, "Please kindly confirm your account.") };
                }
                else
                {
                    _logger.LogInformation("The passwod of the user is incorrect");
                    return new UserResponseModel { ResponseMessage = _commonHelper.OutputMessage(false, "Invalid Login attempt") };
                }

            }
        }

        public async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var utcNow = DateTime.UtcNow;
            var allClaimsAssignedToThisUser = UserClaims(user: user);

            List<Claim> claims = new List<Claim>();

            claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.FirstName}{user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Acr, user.UserName),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                

            }.Union(allClaimsAssignedToThisUser).ToList();

           

            //including the roles in the token
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtSettings.Audience,
                Issuer = _jwtSettings.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.Lifespan),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private IEnumerable<Claim> UserClaims(ApplicationUser user)
        {
            var claims = _userManager.GetClaimsAsync(user: user).Result;
            return claims;
        }


        //method to confirm account
        public async Task<UserAccountModel> ConfirmAccount(string token)
        {
            var data = new UserAccountModel();
            var code = _encryptionService.DecryptText(token);
            var result = new IdentityResult();
            if (!string.IsNullOrEmpty(code))
            {
                var activateObj = JsonConvert.DeserializeObject<AccountVerificationModel>(code);
                var user = await _userManager.FindByIdAsync(activateObj.UserId.ToString());

                result = await _userManager.ConfirmEmailAsync(user, activateObj.Code);
                data.User = user;
                data.IdentityResult = result;

               
            }
            return data;
        }

        public async Task<bool> SendResetPasswordLink(ApplicationUser user)
        {
            var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordUrlCode = _encryptionService.UrlEncodedEncryptText(JsonConvert.SerializeObject(new AccountVerificationModel
            {
                Code = passwordResetCode,
                UserId = user.Id
            }));

            //redirect url to be sent to the mail

            var resetPasswordUrl = string.Format(_environmentalVariables.ClientUrl + "{0}/{1}?token={2}", "authentication", "reset-password", passwordUrlCode);
            System.IO.File.WriteAllText("passwordreset.txt", resetPasswordUrl);

            string Message = "Click on the Link to reset your Password";

            var subject = "Reset Password";

            //get webroot path 

            var webroot = _env.WebRootPath;

            //Get TemplateFile Located at wwwroot/templates/ConfirmAccount.html
            var pathToFile = webroot
                            + Path.DirectorySeparatorChar.ToString()
                            + "templates"
                            + Path.DirectorySeparatorChar.ToString()
                            + "ForgotPassword.html";
            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            //{0} : Subject
            //{1}:  DateTime
            //{2}:  Name
            //{3}: Message
            //{4}: callbackUrl

            string messageBody = string.Format(builder.HtmlBody,
                subject,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                user.FirstName,
                Message,
                resetPasswordUrl
                );

            await _emailService.SendEmailAsync(user.Email, subject, messageBody);

            return true;
        }

        //reset password
        //method to reset password
        public async Task<ResetPasswordModel> ResetPasword(string token)
        {
            var data = new ResetPasswordModel();
            var code = _encryptionService.DecryptText(token);
            var result = new IdentityResult();
            if (!string.IsNullOrEmpty(code))
            {
                var resetObj = JsonConvert.DeserializeObject<AccountVerificationModel>(code);
                var user = await _userManager.FindByIdAsync(resetObj.UserId.ToString());

           
                data.User = user;
                data.Token = code;
            }

            return data;
        }

        public async Task<ResponseMessage> PerformPasswordChange(PasswordResetModel model)
        {
            var code = _encryptionService.UrlDecodeEncryptText(model.Token);
            var resetPwdObj = JsonConvert.DeserializeObject<AccountVerificationModel>(code);
            var user = await _userManager.FindByIdAsync(resetPwdObj.UserId.ToString());

            if(user != null)
            {
                LoginModel loginModel = new LoginModel()
                {
                    Email = user.Email,
                    Password = model.NewPassword
                };

                //trying to sign the user in to the application to check password

                var result = await CheckValidUser(loginModel);

                if (result.ResponseMessage.IsSuccessful)
                {
                    return _commonHelper.OutputMessage(false, "You can not use a recent Password.Enter a different Password");
                }
                else
                {
                    var passwordResult = _userManager.ResetPasswordAsync(user, resetPwdObj.Code, model.NewPassword);

                    if (passwordResult.Result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                        }

                        return _commonHelper.OutputMessage(true, "Successful! Kindly proceed to login");
                    }
                    else
                    {
                        return _commonHelper.OutputMessage(false, passwordResult.Result.Errors.FirstOrDefault().Description);
                    }
                }
                
               
            }
            else
            {
                return _commonHelper.OutputMessage(false, "User does not exist");
            }
        }
    }
}
