using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Services
{
    public class AdminsServices
    {
        public AdminsServices(ApiServices apiServices)
        {
            ApiServices = apiServices;
        }

        public ApiServices ApiServices { get; }

        public async Task<ResponseDTO<int>> AddAsync(AdminModelDTO adminModelDTO) {
            return await ApiServices.PostJsonAsync<ResponseDTO<int>>("/api/Admins", adminModelDTO);
        }
        public async Task<ResponseDTO<object>> EditAsync(AdminModelDTO adminModelDTO)
        {
            return await ApiServices.PutJsonAsync<ResponseDTO<object>>("/api/Admins", adminModelDTO);
        }
        public async Task<ResponseDTO<object>> DeleteAsync(AdminModelDTO adminModelDTO)
        {
            return await ApiServices.DeleteJsonAsync<ResponseDTO<object>>($"/api/Admins?email={adminModelDTO.Email}");
        }
        public async Task<ResponseDTO<IEnumerable<AdminModelDTO>>> GetAllAsync() {
          return await ApiServices.GetJsonAsync<ResponseDTO<IEnumerable<AdminModelDTO>>>("/api/Admins");
        }

        public async Task<ResponseDTO<object>> AssignToRole(string email)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Admins/AssignToRole", email);
        }

        public async Task<ResponseDTO<object>> RemoveFromRole(string email)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<object>>("/api/Admins/RemoveFromRole", email);
        }

    }
}
