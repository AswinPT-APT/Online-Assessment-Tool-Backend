namespace OnlineAssessmentTool.Application.Dtos.RolePermission
{
    public class CreateRoleDTO
    {
        public string RoleName { get; set; }
        public List<int> PermissionIds { get; set; }
    }
}
