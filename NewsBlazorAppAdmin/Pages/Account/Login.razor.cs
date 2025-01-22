using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.Account
{
    public partial class Login
    {
        public UserLoginDTO UserLogin { get; set; }

        public string AdminEmail { get; set; }
        public bool sentEmail { get; set; } = false;
        

        ResponseDTO<object> message { get; set; }

        [Inject]
        AccountService AccountService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        ILocalStorageService LocalStorage { get; set; }


        private async Task ValidSubmit()
        {
            var response = await AccountService.Login(UserLogin);
            if (response != null)
            {
                if (response.Success)
                {
                    string token = (string)response.Data;
                    await LocalStorage.SetItemAsync("token", token);
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    message = new ResponseDTO<object>()
                    {
                        Data = response.Data,
                        Success = false,
                        Message = response.Message
                    };
                }
            }
            else
            {
                message = new ResponseDTO<object>()
                {
                    Data = "",
                    Success = false,
                    Message = "حدث خطا في الخادم"
                };
            }
        }
        private async Task SendEmail() {
           var response = await AccountService.SendPasswordToMail(AdminEmail);
            if (response?.Success == true)
            {
                message = new ResponseDTO<object>()
                {
                    Success = true,
                    Message = "تم ارسال كلمه المرور عبر الايميل"
                };
                sentEmail = true;
                StateHasChanged();
            }
            else if(response!=null) {
                message = new ResponseDTO<object>()
                {
                    Success = false,
                    Message = response.Message
                };
            }
        }
        protected override void OnInitialized()
        {
            UserLogin = new UserLoginDTO
            {
                Email = "",
                Password = ""
            };
        }
    }
}
