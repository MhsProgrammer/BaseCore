using BaseCore.Application.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Application.Contracts.Identity
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task ChangeRole(string userId);
        //Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
    }
}
