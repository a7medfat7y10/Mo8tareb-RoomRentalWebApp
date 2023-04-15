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

namespace Mo8tareb_RoomRentalWebApp.Api.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;

         // IMapper mapper
        //  IEmailSender emailSender,
        public AccountsController(UserManager<AppUser> userManager, JwtHandler jwtHandler,
            IUnitOfWork unitOfWork, IEmailSender emailSender)  
        {
            _userManager = userManager;
            //_mapper = mapper;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
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

                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
            }


            SigningCredentials? signingCredentials = _jwtHandler.GetSigningCredentials();
            List<Claim>? claims = await _jwtHandler.GetClaims(user);
            JwtSecurityToken? tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
            string? token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            await _userManager.ResetAccessFailedCountAsync(user);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            // AppUser? user = _mapper.Map<AppUser>(userForRegistration);
            AppUser? user = new AppUser
            {
                FirstName = userForRegistration.FirstName,
                LastName = userForRegistration.LastName,
                Email = userForRegistration.Email,
                UserName = userForRegistration.Email
            };

            IdentityResult? result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                IEnumerable<string>? errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, Authorization.User);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"Mo8tareb", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);
            var imageUrl = "https://img0.etsystatic.com/000/0/5229903/il_fullxfull.270122038.jpg";
            Message message = new Message(
                new string[] { userForRegistration.Email },
                "Mo8tareb Room Rental Web App: Email Confirmation",
                $@"<html>
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
    <p>If you did not register to Mo8tareb Room Rental Web App, please ignore this message.</p>
    <p>Best regards,</p>
    <p>Mo8tareb Room Rental Web App Team</p>
    <h4>P.S. - You can check out our website to find more information about our services and offers: <a href=""[Mo8tareb Web App URL]"">Mo8tareb Web App URL</a></h4>
  </body>
</html>",
                null,
                true);
            await _emailSender.SendEmailAsync(message);

            return StatusCode(201);
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

            Dictionary<string, string> param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

            var message = new Message(new string[] { user.Email }, "Reset password token", callback, null, false);
            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

      
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            AppUser? user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            IdentityResult? resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                IEnumerable<string>? errors = resetPassResult.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));
            return Ok();
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");

            IdentityResult? confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");

            return Ok();
        }

        [Authorize(Roles ="Admin")]
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

        #endregion

    }
}