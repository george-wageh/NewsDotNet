using NewsWebApi.Data;
using NewsWebApi.Enums;
using NewsWebApi.Models;
using SharedLib.AdminDTO;

namespace NewsWebApi.Services
{
    public class StartupTaskService : IHostedService
    {
        public StartupTaskService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            using (var scope = ServiceProvider.CreateScope())
            {
                AppDbContext appDbContext = scope.ServiceProvider.GetService<AppDbContext>();
                foreach (int i in Enum.GetValues<RoleTypes>())
                {
                    var roleStr = ((RoleTypes)(i)).ToString();
                    var roleModel = appDbContext.Set<RoleModel>().FirstOrDefault(x => x.Name == roleStr);
                    if (roleModel == null)
                    {
                        await appDbContext.Set<RoleModel>().AddAsync(new RoleModel
                        {
                            Name = roleStr
                        });
                        await appDbContext.SaveChangesAsync();
                    }
                }
                AdminService adminService = scope.ServiceProvider.GetService<AdminService>();
                var responseOwnerDto = await adminService.GetAsync("gorgewageh604@gmail.com");
                if (!responseOwnerDto.Success)
                {
                    var ownerDto = new AdminModelDTO
                    {
                        Email = "gorgewageh604@gmail.com",
                        Name = "George wagih"
                    };
                    await adminService.AddAsync(ownerDto);
                    await adminService.AddRoleToUserAsync(ownerDto.Email, RoleTypes.owner.ToString());
                    await adminService.AddRoleToUserAsync(ownerDto.Email, RoleTypes.admin.ToString());
                }
            }


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
