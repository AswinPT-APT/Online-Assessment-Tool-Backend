using FluentEmail.Core.Models;
using OnlineAssessmentTool.Dtos;

namespace OnlineAssessmentTool.Application.Common.Interfaces.IService
{
    public interface IEmailService
    {
        Task<SendResponse> SendEmailAsync(string toEmail, string subject, string body, List<AttachmentDTO> attachments = null);
    }
}
