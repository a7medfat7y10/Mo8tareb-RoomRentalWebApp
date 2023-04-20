using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mo8tareb_RoomRentalWebApp.BL.Dtos.Accounts;
using Mo8tareb_RoomRentalWebApp.DAL.Models;

namespace Mo8tareb_RoomRentalWebApp.Api.JwtFeatures
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IConfigurationSection _goolgeSettings;
        private readonly UserManager<AppUser> _userManager;
        private IHttpContextAccessor _httpAccessor;

        public JwtHandler(IConfiguration configuration, UserManager<AppUser> userManager, IHttpContextAccessor httpAccessor)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
            _userManager = userManager;
            _goolgeSettings = _configuration.GetSection("GoogleAuthSettings");
            _httpAccessor = httpAccessor;
        }

        public SigningCredentials GetSigningCredentials()
        {
            byte[]? key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value!);
            SymmetricSecurityKey? secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<List<Claim>> GetClaims(AppUser user)
        {
            List<Claim>? claims = new List<Claim>
           {
              new Claim(ClaimTypes.Email, user.Email!),
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.MobilePhone, user.PhoneNumber??"null"),
              new Claim(ClaimTypes.Name, user.FirstName??"null"),
              new Claim(ClaimTypes.Gender, user.Gender.ToString()??"null"),
           };

            IList<string>? roles = await _userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(item: new Claim(ClaimTypes.Role, role));
            }


            return claims;
        }

        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
             JwtSecurityToken? tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        public async Task<string> GenerateToken(AppUser user)
        {
            SigningCredentials? signingCredentials = GetSigningCredentials();
            List<Claim>? claims = await GetClaims(user);
            JwtSecurityToken? tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            string? token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }

        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDto externalAuth)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _goolgeSettings.GetSection("clientId").Value }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
