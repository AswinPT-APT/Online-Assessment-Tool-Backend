﻿using OnlineAssessmentTool.Shared.Enums;

namespace OnlineAssessmentTool.Application.Dtos.User
{
    public class CreateUserDTO
    {
        public int userId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public UserType UserType { get; set; }
        public Guid UUID { get; set; }
    }
}
