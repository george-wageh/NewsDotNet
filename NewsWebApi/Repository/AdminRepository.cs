using Microsoft.EntityFrameworkCore;
using NewsWebApi.Data;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using System;

namespace NewsWebApi.Repository
{
    public class AdminRepository:IAdminRepository
    {
        public AppDbContext AppDbContext { get; }
        public AppUnitWork AppUnitWork { get; }

        public AdminRepository(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }
        public async Task<IEnumerable<AdminModel>> GetAllAsync()
        {
            return await AppDbContext.Set<AdminModel>().OrderBy(x => x.Id).Include(x=>x.UserRoles).ThenInclude(x=>x.Role).ToListAsync();
        }

        public async Task AddAsync(AdminModel adminModel)
        {
            await AppDbContext.Set<AdminModel>().AddAsync(adminModel);
        }

        public async Task<AdminModel> GetAsync(string email)
        {
            return await AppDbContext.Set<AdminModel>().Where(x => x.Email == email).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RoleModel>> GetAllRolesAsync(string email)
        {
            var roles = await AppDbContext.Set<AdminModel>()
                .Where(x => x.Email == email)
                .SelectMany(x => x.UserRoles)
                .Select(ur => ur.Role)
                .ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<RoleModel>> GetAllRolesAsync(int id)
        {
            var roles = await AppDbContext.Set<AdminModel>()
                          .Where(x => x.Id == id)
                          .SelectMany(x => x.UserRoles)
                          .Select(ur => ur.Role)
                          .ToListAsync();
            return roles;
        }

        public void Remove(AdminModel adminModel)
        {
            AppDbContext.Remove(adminModel);
        }
    }
}
