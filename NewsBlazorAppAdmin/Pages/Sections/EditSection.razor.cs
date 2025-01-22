using Microsoft.AspNetCore.Components;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.Sections
{
    public partial class EditSection
    {
        [Parameter]
        public SectionDTO Item { get; set; }
        [Parameter]
        public EventCallback<SectionDTO> OnSaveChanges { get; set; }
    }
}
