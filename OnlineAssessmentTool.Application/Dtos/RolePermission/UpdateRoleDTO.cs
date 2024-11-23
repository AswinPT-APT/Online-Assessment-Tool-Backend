namespace OnlineAssessmentTool.Application.Dtos.RolePermission
{
    public class UpdateRoleDTO
    {
        public string RoleName { get; set; }
        public ICollection<CreatePermissionDTO> Permissions { get; set; }
    }
}
