using BaseCore.Application.Contracts.Identity;
using BaseCore.Application.Exeptions;
using BaseCore.Application.Models.Authentication;
using BaseCore.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IBlacklistTokenRepository _blacklistTokenRepository;
        
        public AuthenticationService(UserManager<AppUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<AppUser> signInManager,
            IBlacklistTokenRepository blacklistTokenRepository)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _blacklistTokenRepository = blacklistTokenRepository;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new Exception($"User with {request.UserName} not found.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new Exception($"Credentials for '{request.UserName} aren't valid'.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName!
            };

            user.AccessToken = response.Token;
            user.AccessTokenExpireDate = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            await _userManager.UpdateAsync(user);

            return response;
        }

        public async Task ChangeRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("کاربر" , userId);
            var isSuperAdmin = await _userManager.IsInRoleAsync(user , "SuperAdmin");

            if (isSuperAdmin)
            {
                await _userManager.RemoveFromRoleAsync(user , "SuperAdmin");
                await _userManager.AddToRoleAsync(user , "User");
            } else
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "SuperAdmin");
            }

            await _blacklistTokenRepository.AddTokenToBlacklistAsync(user.AccessToken , user.AccessTokenExpireDate);

        }

        //public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        //{
        //    var existingUser = await _userManager.FindByNameAsync(request.UserName);

        //    if (existingUser != null)
        //    {
        //        throw new Exception($"Username '{request.UserName}' already exists.");
        //    }

        //    var user = new ApplicationUser
        //    {
        //        Email = request.Email,
        //        FirstName = request.FirstName,
        //        LastName = request.LastName,
        //        UserName = request.UserName,
        //        EmailConfirmed = true
        //    };

        //    var existingEmail = await _userManager.FindByEmailAsync(request.Email);

        //    if (existingEmail == null)
        //    {
        //        var result = await _userManager.CreateAsync(user, request.Password);

        //        if (result.Succeeded)
        //        {
        //            return new RegistrationResponse() { UserId = user.Id };
        //        }
        //        else
        //        {
        //            throw new Exception($"{result.Errors}");
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception($"Email {request.Email} already exists.");
        //    }
        //}

        private async Task<JwtSecurityToken> GenerateToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat , DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
