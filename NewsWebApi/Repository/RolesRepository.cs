using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;

namespace NewsWebApi.Repository
{
    public class RolesRepository:IRolesRepository
    {
        public RolesRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public AppDbContext AppDbContext { get; }

        public async Task<RoleModel> GetAsync(string name) {
          return await AppDbContext.Set<RoleModel>().FirstOrDefaultAsync(x => x.Name == name);
        }

    }
}
