using EmployeeServiceProg.Models.Dto;
using EmployeeServiceProg.Models.Requests;
using Microsoft.AspNetCore.Identity.Data;

namespace EmployeeServiceProg.Services.Interfaces
{
    public interface IAuthenticateService
    {
        AuthenticationResponse Login(AuthenticationRequest authenticationRequest);

        public SessionDto GetSession(string sessionToken);
    }
}
