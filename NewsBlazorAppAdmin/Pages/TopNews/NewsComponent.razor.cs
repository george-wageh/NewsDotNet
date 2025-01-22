using Microsoft.AspNetCore.Components;
using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.TopNews
{
    public partial class NewsComponent
    {
        [Parameter]
        public IEnumerable<NewDTO> News { get; set; }

        [Parameter]
        public EventCallback<int> OnDelete { get; set; }
        [Parameter]
        public EventCallback<int> OnSetUp { get; set; }
        [Parameter]
        public EventCallback<int> OnSetDown { get; set; }
    }
}
