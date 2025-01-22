using NewsWebApi.Models;

namespace NewsWebApi.IRepository
{
    public interface IRolesRepository
    {
        public Task<RoleModel> GetAsync(string name);
           
    }
}
