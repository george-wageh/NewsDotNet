using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.Admins
{
    public partial class Admins
    {
        [Inject]
        AdminsServices AdminsServices { get; set; }

        AdminModelDTO ModelToAdd { get; set; }

        AdminModelDTO ModelToEdit { get; set; }

        AdminModelDTO ModelToDelete { get; set; }

        IEnumerable<AdminModelDTO> admins { get; set; }

        ResponseDTO<object> Message { get; set; }


        protected override async Task OnInitializedAsync()
        {
          var response = await AdminsServices.GetAllAsync();
            if (response?.Success == true) {
                admins = response.Data;
            }
        }
        public async Task ShowMessage(ResponseDTO<object> message)
        {
            this.Message = message;
            StateHasChanged();
            await Task.Delay(2000);
            this.Message = null;
            StateHasChanged();
        }
        public void OpenAdd() {
            ModelToAdd = new AdminModelDTO
            {
                Id = 0,
                Roles = new List<string>()
            };
        }
        public void OpenEdit(int id)
        {
            ModelToEdit = admins.Where(x => x.Id == id).FirstOrDefault()?.Clone();
            StateHasChanged();
        }
        public void OpenDelete(int id)
        {
            ModelToDelete = admins.Where(x => x.Id == id).FirstOrDefault()?.Clone();
            StateHasChanged();
        }
        public async Task onSaveAdd(AdminModelDTO adminModel) {
            if (adminModel != null) {
                var response = await AdminsServices.AddAsync(adminModel);
                if (response?.Success == true)
                {
                    adminModel.Id = response.Data;
                    admins = admins.Append(adminModel);
                }
                else if (response?.Success == false)
                {
                    ShowMessage(new ResponseDTO<object> { 
                        Success= false,
                        Message = response.Message
                    });
                }
            }
           
            ModelToAdd = null;
            StateHasChanged();
        }
        public async Task onSaveEdit(AdminModelDTO adminModel)
        {
            if (adminModel != null)
            {
                var response = await AdminsServices.EditAsync(adminModel);
                if (response?.Success == true)
                {
                    var admin = admins.Where(x => x.Id == adminModel.Id).FirstOrDefault();
                    admin.Name = adminModel.Name;
                }
                else if (response?.Success == false)
                {
                    ShowMessage(new ResponseDTO<object>
                    {
                        Success = false,
                        Message = response.Message
                    });
                }
            }
            ModelToEdit = null;
            StateHasChanged();
        }
        public async Task onSaveDelete(AdminModelDTO adminModel)
        {
            if (adminModel != null)
            {
                var response = await AdminsServices.DeleteAsync(adminModel);
                if (response?.Success == true)
                {
                    admins = admins.Where(x => x.Id != adminModel.Id).ToList();
                }
                else if (response?.Success == false)
                {
                    ShowMessage(new ResponseDTO<object>
                    {
                        Success = false,
                        Message = response.Message
                    });
                }
            }
            ModelToDelete = null;
            StateHasChanged();
        }
        public async Task AddToRole(string email) {
            var response = await AdminsServices.AssignToRole(email);
            if (response?.Success == true)
            {
                var admin = admins.Where(x => x.Email == email).FirstOrDefault();
                if (admin != null) {
                    admin.Roles = admin.Roles.Append("admin");
                }
            }
            else if (response?.Success == false)
            {
                ShowMessage(new ResponseDTO<object>
                {
                    Success = false,
                    Message = response.Message
                });
            }
            StateHasChanged();
        }

        public async Task RemoveFromRole(string email)
        {
            var response = await AdminsServices.RemoveFromRole(email);
            if (response?.Success == true)
            {
                var admin = admins.Where(x => x.Email == email).FirstOrDefault();
                if (admin != null)
                {
                    admin.Roles = new List<string>();
                }
            }
            else if (response?.Success == false)
            {
                ShowMessage(new ResponseDTO<object>
                {
                    Success = false,
                    Message = response.Message
                });
            }
            StateHasChanged();
        }
    }
}
