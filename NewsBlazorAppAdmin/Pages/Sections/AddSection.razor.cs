using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.Sections
{
    public partial class AddSection
    {
        [Parameter]
        public SectionDTO Item { get; set; }
        [Parameter]
        public EventCallback<SectionDTO> OnSaveChanges { get; set; }
    }
}
