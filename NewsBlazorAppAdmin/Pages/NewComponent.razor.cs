using Microsoft.AspNetCore.Components;
using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages
{
    public partial class NewComponent
    {
        [Parameter]
        public NewDTO item { get; set; }
    }
}
