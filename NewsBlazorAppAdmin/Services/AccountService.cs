using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Services
{
    public class AccountService
    {

        public AccountService(ApiServices apiServices)
        {
            ApiServices = apiServices;
        }

        public ApiServices ApiServices { get; }

        public async Task<ResponseDTO<string>> Login(UserLoginDTO UserLogin)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<string>>("/api/Account/Login", UserLogin);
        }

        public async Task<ResponseDTO<string>> SendPasswordToMail(string email)
        {
            return await ApiServices.PostJsonAsync<ResponseDTO<string>>("/api/Account/SendPasswordToMail", email);
        }
    }
}
