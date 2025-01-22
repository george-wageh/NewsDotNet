using Microsoft.AspNetCore.Components;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.News
{
    public partial class DeleteConfirmation
    {
        [Parameter]
        public NewDTO Item { get; set; }
        [Parameter]
        public EventCallback<NewDTO> OnSaveChanges { get; set; }
    }
}
