using NewsWebApi.Data;
using NewsWebApi.Enums;
using NewsWebApi.IRepository;
using NewsWebApi.Models;
using NewsWebApi.Repository;
using SharedLib.AdminDTO;
using SharedLib.DTO;
using System.Data;

namespace NewsWebApi.Services
{
    public class AdminService
    {
        public AdminService(IAdminRepository adminRepository,IRolesRepository rolesRepository, AppUnitWork appUnitWork)
        {
            AdminRepository = adminRepository;
            RolesRepository = rolesRepository;
            AppUnitWork = appUnitWork;
        }

        public IAdminRepository AdminRepository { get; }
        public IRolesRepository RolesRepository { get; }
        public AppUnitWork AppUnitWork { get; }

        public async Task<ResponseDTO<IEnumerable<AdminModelDTO>>> GetAllAsync()
        {
            var admins = await AdminRepository.GetAllAsync();
            var adminsDTO = admins.Select(x => new AdminModelDTO
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Roles = x.UserRoles.Select(x=>x.Role.Name).ToList()
            });
            return new ResponseDTO<IEnumerable<AdminModelDTO>>
            {
                Success = true,
                Data = adminsDTO
            };
        }
        public async Task<ResponseDTO<AdminModelDTO>> GetAsync(string email)
        {
            var admin = await AdminRepository.GetAsync(email);
            if (admin != null)
            {
                return new ResponseDTO<AdminModelDTO>
                {
                    Success = true,
                    Data = new AdminModelDTO
                    {
                        Id = admin.Id,
                        Name = admin.Name,
                        Email = admin.Email,
                        Roles = admin.UserRoles.Select(admin => admin.Role.Name).ToList()
                    }
                };
            }
            else {
                return new ResponseDTO<AdminModelDTO>
                {
                    Message = "المسؤول غير موجود"
                };
            }
        }
        public async Task<ResponseDTO<int>> AddAsync(AdminModelDTO adminModelDTO)
        {

            AdminModel admin = await AdminRepository.GetAsync(adminModelDTO.Email);
            if (admin == null)
            {
                admin = new AdminModel
                {
                    Email = adminModelDTO.Email,
                    Name = adminModelDTO.Name
                };
                await AdminRepository.AddAsync(admin);
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<int> { Success = true, Data = admin.Id };
            }
            return new ResponseDTO<int>
            {
                Message = "المسؤول موجود بالفعل"
            };

        }
        public async Task<ResponseDTO<object>> EditAsync(AdminModelDTO adminModelDTO)
        {
            AdminModel admin = await AdminRepository.GetAsync(adminModelDTO.Email);
            if (admin != null)
            {
                admin.Name = adminModelDTO.Name;
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object> { Success = true };
            }
            return new ResponseDTO<object>
            {
                Message = "المسؤول ليس موجود"
            };

        }
        public async Task<ResponseDTO<object>> RemoveAsync(string email) {
            AdminModel admin = await AdminRepository.GetAsync(email);
            var roles = await AdminRepository.GetAllRolesAsync(email);
            var rolesStr = roles.Select(x => x.Name);
            if (!rolesStr.Contains(RoleTypes.owner.ToString()))
            {
                if (admin != null)
                {
                    AdminRepository.Remove(admin);
                    await AppUnitWork.SaveChangesAsync();
                    return new ResponseDTO<object>
                    {
                        Success = true
                    };
                }
                else
                {
                    return new ResponseDTO<object>
                    {
                        Message = "المسؤول ليس موجود"
                    };
                }

            }
            else {
                return new ResponseDTO<object>
                {
                    Message = "المسؤول غير قابل للإزالة"
                };
            }
            
        }

        public async Task<ResponseDTO<object>> AddRoleToUserAsync(string email , string role) {
            try {
                Enum.Parse<RoleTypes>(role);
            }
            catch(Exception e) {
                return new ResponseDTO<object> { Message = "خطا في تحديد المسؤوليه" };
            }
            var admin = await AdminRepository.GetAsync(email);
            if (admin != null) {
                var roles = admin.UserRoles.Select(x=>x.Role).Select(x=>x.Name);
                if (roles.Contains(role)) { 
                    return new ResponseDTO<object> { Message = "موجود بالفعل"};
                }
                var roleModel = await RolesRepository.GetAsync(role);
                admin.UserRoles.Add(new UserRoleModel
                {
                    Role = roleModel
                });
                await AppUnitWork.SaveChangesAsync();
                return new ResponseDTO<object> { Success = true };
            }
            return new ResponseDTO<object> { Message = "المسؤول غير موجود" };
        }
        public async Task<ResponseDTO<object>> RemoveFromRoleAsync(string email, string role)
        {
            try
            {
                Enum.Parse<RoleTypes>(role);
            }
            catch (Exception e)
            {
                return new ResponseDTO<object> { Message = "خطا في تحديد المسؤوليه" };
            }
            var admin = await AdminRepository.GetAsync(email);
            if (admin != null)
            {
                var roles = admin.UserRoles.Select(x => x.Role).Select(x => x.Name);
                if (roles.Contains(RoleTypes.owner.ToString())) { 
                     return new ResponseDTO<object> { Message = "غير قادر علي مسح المسؤوليه" };
                }
                if (roles.Contains(role))
                {
                    var roleModel = admin.UserRoles.FirstOrDefault(x => x.Role.Name == role);
                    if (roleModel != null) { 
                        admin.UserRoles.Remove(roleModel);
                        await AppUnitWork.SaveChangesAsync();
                    }
                    return new ResponseDTO<object> {Success = true};
                }
                else { 
                    return new ResponseDTO<object> { Message = "المسؤوليه غير موجوده" };

                }
            }
            return new ResponseDTO<object> { Message = "المسؤول غير موجود" };
        }
    }
}
