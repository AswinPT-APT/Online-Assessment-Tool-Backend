using OnlineAssessmentTool.Application.Dtos.User;
using OnlineAssessmentTool.Dtos;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IService
{
    public interface IAuthService
    {
        JwtSecurityToken ReadJwtToken(string token);
        public Task<ApiResponse> AuthenticateSSOUser(string token);
        public Task<ApiResponse> AuthenticateUser(LoginRequestDTO loginRequest);
        public string GenerateJwtToken(UserDetailsDTO user);
        public string GenerateOtp();
    }
}
