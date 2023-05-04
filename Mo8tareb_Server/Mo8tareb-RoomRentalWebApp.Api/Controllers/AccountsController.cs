using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Mo8tareb_RoomRentalWebApp.Api.JwtFeatures;
using Mo8tareb_RoomRentalWebApp.Api.Services.Email;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts;
using Mo8tareb_RoomRentalWebApp.DAL;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Mo8tareb_RoomRentalWebApp.DAL.Constants;
using Authorization = Mo8tareb_RoomRentalWebApp.DAL.Constants.Authorization;
using static Mo8tareb_RoomRentalWebApp.DAL.Constants.Enums;
using System.Web;
using System.Text;

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, JwtHandler jwtHandler,
            IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

       

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            AppUser? user = await _userManager.FindByEmailAsync(userForAuthentication.Email);

            if (user == null)
                return BadRequest("Authentication failed. Wrong Username or Password");

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                if (await _userManager.IsInRoleAsync(user, Authorization.Owner))
                {
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "This account still under review" });
                }
                else
                {
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Email is not confirmed" });
                }
            }

            //you can check here if the account is locked out in case the user enters valid credentials after locking the account.
            if (await _userManager.IsLockedOutAsync(user))
            {
                string? content = $"Your account is locked out. To reset the password click this link: {userForAuthentication.clientURI}";
                Message? message = new Message(new string[] { userForAuthentication.Email }, "Locked out account information", content, null, false);
                await _emailSender.SendEmailAsync(message);

                return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
            }

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                await _userManager.AccessFailedAsync(user);

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Wrong password" });
            }


            SigningCredentials? signingCredentials = _jwtHandler.GetSigningCredentials();
            List<Claim>? claims = await _jwtHandler.GetClaims(user);
            JwtSecurityToken? tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            string? token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            await _userManager.AddClaimsAsync(user, claims);
            await _userManager.ResetAccessFailedCountAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            string RolesString = roles.Count == 1 ? roles[0] : string.Join(',', roles);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token, Role = RolesString, Email = user.Email });
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || userForRegistration.Password != userForRegistration.ConfirmPassword || !ModelState.IsValid || Enum.TryParse<Gender>(userForRegistration.Gender, out Gender gender) == false)
                return BadRequest(ModelState);

            // AppUser? user = _mapper.Map<AppUser>(userForRegistration);
            AppUser? user = new AppUser
            {
                FirstName = userForRegistration.FirstName,
                LastName = userForRegistration.LastName,
                Email = userForRegistration.Email,
                PhoneNumber = userForRegistration.phone,
                UserName = userForRegistration.UserName,
                Gender = gender
            };

            IdentityResult? result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                IEnumerable<string>? errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, Authorization.User);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var TokenBytes = Encoding.UTF8.GetBytes(token);
            var encodedTokens = Convert.ToBase64String(TokenBytes);

            var param = new Dictionary<string, string>
            {
                {"token", encodedTokens },
                {"email", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);

            var imageUrl = "https://assets.stickpng.com/images/5a04b8b69cf05203c4b603b6.png";

            string messageBody = GetEmailConfirmationMessage(imageUrl, callback, userForRegistration.Email);
            Message message = new Message(new string[] { userForRegistration.Email },
                                           subject: "Mo8tareb Room Rental Web App: Email Confirmation",
                                           messageBody,
                                           attachments: null,
                                           isHtml: true
                                          );

            await _emailSender.SendEmailAsync(message);

            return StatusCode(201);
        }


        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request 1");

            var tokenArray = Convert.FromBase64String(token);
            var DecodedToken = Encoding.UTF8.GetString(tokenArray);

            IdentityResult? confirmResult = await _userManager.ConfirmEmailAsync(user, DecodedToken);
            if (!confirmResult.Succeeded)
                return BadRequest($"Invalid Email Confirmation Request 2 token={token} user, {user.Email}");



            SigningCredentials? signingCredentials = _jwtHandler.GetSigningCredentials();
            List<Claim>? claims = await _jwtHandler.GetClaims(user);
            JwtSecurityToken? tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            string? Jwttoken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            await _userManager.AddClaimsAsync(user, claims);
            await _userManager.ResetAccessFailedCountAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            string RolesString = roles.Count == 1 ? roles[0] : string.Join(',', roles);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = Jwttoken, Role = RolesString });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            AppUser? user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Email Not Found");

            string? token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var tokenArrayBytes = Encoding.UTF8.GetBytes(token);
            var encodedToken = Convert.ToBase64String(tokenArrayBytes);

            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"token", encodedToken },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);
            var imageUrl = "https://assets.stickpng.com/images/5a04b8b69cf05203c4b603b6.png";
            string messageBody = GenerateForgotPasswordEmailBody(imageUrl, callback);
            Message message = new Message(new string[] { forgotPasswordDto.Email },
                                           subject: "Mo8tareb Room Rental Web App: Email Confirmation",
                                           messageBody,
                                           attachments: null,
                                           isHtml: true
                                          );

            await _emailSender.SendEmailAsync(message);

            return StatusCode(201);
        }


        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid || resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
                return BadRequest();

            AppUser? user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var tokenBytes = Convert.FromBase64String(resetPasswordDto.Token ?? "");
            var decodedToken = Encoding.UTF8.GetString(tokenBytes);

            IdentityResult? resetPassResult = await _userManager.ResetPasswordAsync(user, decodedToken ?? "", resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                IEnumerable<string>? errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));
            return Ok();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            var claims = User.Claims
                .Select(c => new { c.Type, c.Value })
                .ToList();

            return Ok(claims);
        }

        #region OwnerRegistration

        #endregion

        #region ExternalLogin

        [HttpPost("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
        {
            var payload = await _jwtHandler.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest("Invalid External Authentication.");

            UserLoginInfo? info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

            AppUser? user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new AppUser { Email = payload.Email, UserName = payload.Email, FirstName = payload.GivenName, LastName = payload.FamilyName };
                    await _userManager.CreateAsync(user);

                    //TODO: prepare and send an email for the email confirmation

                    await _userManager.AddToRoleAsync(user, Authorization.User);
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, Authorization.Owner) &&
                        !user.EmailConfirmed)
                    {
                        return BadRequest("Invalid External Authentication., your account is under review");
                    }

                    await _userManager.AddLoginAsync(user, info);
                }
            }

            string? token = await _jwtHandler.GenerateToken(user);
            //byte[]? TokenBytes = Encoding.UTF8.GetBytes(token);
            //string? encodedTokens = Convert.ToBase64String(TokenBytes);

            return Ok(new AuthResponseDto { Token = token, IsAuthSuccessful = true });
        }

        #endregion

        private string GetEmailConfirmationMessage(string imageUrl, string callback, string email)
        {
            return $@"<html>
               <head>
                 <style>
                   body {{
                     font-family: Arial, sans-serif;
                     font-size: 16px;
                     color: #333;
                     text-align: center;
                   }}
                   img {{
                     display: block;
                     margin: 0 auto;
                     width: 50%;
                     height: 50%;
                   }}
                 </style>
               </head>
               <body>
                 <h2>Thank you for registering to Mo8tareb Room Rental Web App, the leading online platform for finding affordable rooms in Egypt.</h2>
                 <img src=""{imageUrl}"" alt=""Image Description"" width=""400"" height=""300"">
                 <p>Please confirm your email address by clicking on the following link:</p>
                 <p><a href=""{callback}"">Click Here</a></p>
                 <p>Please note that this link will expire in 24 hours.</p>
                 <p>If you did not register to Mo8tareb Room Rental Web App using this email address ({email}), please ignore this message.</p>
                 <p>Best regards,</p>
                 <p>Mo8tareb Room Rental Web App Team</p>
                 <h4>P.S. - You can check out our website to find more information about our services and offers: <a href=""[Mo8tareb Web App URL]"">Mo8tareb Web App URL</a></h4>
               </body>
             </html>";
        }
        private static string GenerateForgotPasswordEmailBody(string imageUrl, string callback)
        {
            return $@"<html>
                      <head>
                        <style>
                          body {{
                            font-family: Arial, sans-serif;
                            font-size: 16px;
                            color: #333;
                            text-align: center;
                          }}
                          img {{
                            display: block;
                            margin: 0 auto;
                            width: 50%;
                            height: 50%;
                          }}
                        </style>
                      </head>
                      <body>
                        <h2>Thank you for Using Mo8tareb Room Rental Web App, the leading online platform for finding affordable rooms in Egypt.</h2>
                        <img src=""{imageUrl}"" alt=""Image Description"" width=""400"" height=""300"">
                        <p>Please confirm Reseting your password by clicking on the following link:</p>
                        <p><a href=""{callback}"">Click Here</a></p>
                        <p>Please note that this link will expire in 24 hours.</p>
                        <p>If you did not register to Mo8tareb Room Rental Web App, please ignore this message.</p>
                        <p>Best regards,</p>
                        <p>Mo8tareb Room Rental Web App Team</p>
                        <h4>P.S. - You can check out our website to find more information about our services and offers: <a href=""[Mo8tareb Web App URL]"">Mo8tareb Web App URL</a></h4>
                      </body>
                    </html>";
        }


    }
}