using Microsoft.AspNetCore.Components;
using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.Admins
{
    public partial class EditAdmin
    {
        [Parameter]
        public AdminModelDTO Item { get; set; }
        [Parameter]
        public EventCallback<AdminModelDTO> OnSaveChanges { get; set; }
    }
}
