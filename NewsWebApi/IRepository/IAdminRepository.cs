using NewsWebApi.Data;
using NewsWebApi.Models;

namespace NewsWebApi.IRepository
{
    public interface IAdminRepository
    {
        public Task AddAsync(AdminModel sectionModel);
        public Task<IEnumerable<AdminModel>> GetAllAsync();
        public Task<AdminModel> GetAsync(string email);
        public Task<IEnumerable<RoleModel>> GetAllRolesAsync(string email);
        public Task<IEnumerable<RoleModel>> GetAllRolesAsync(int id);
        public void Remove(AdminModel adminModel);
    }
}
