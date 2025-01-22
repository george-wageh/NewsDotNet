namespace NewsWebApi.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<UserRoleModel> UserRoles { get; set; }

    }
}
