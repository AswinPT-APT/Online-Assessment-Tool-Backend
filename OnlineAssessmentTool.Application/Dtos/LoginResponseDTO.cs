using OnlineAssessmentTool.Application.Dtos.User;

namespace OnlineAssessmentTool.Dtos
{
    public class LoginResponseDTO
    {
        public UserDetailsDTO UserDetails { get; set; }
        public string Token { get; set; }
    }
}
