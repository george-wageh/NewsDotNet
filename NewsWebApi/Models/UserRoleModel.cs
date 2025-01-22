using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebApi.Models
{
    public class UserRoleModel
    {
        public int AdminId { get; set; }
        [ForeignKey(nameof(AdminId))]
        public AdminModel Admin { get; set; }

        public int RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public RoleModel Role { get; set; }
    }
}
