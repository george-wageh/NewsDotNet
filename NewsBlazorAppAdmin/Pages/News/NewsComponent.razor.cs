using Microsoft.AspNetCore.Components;
using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.News
{
    public partial class NewsComponent
    {
        [Parameter]
        public IEnumerable<NewAdminDTO> News { get; set; }
        [Parameter]
        public string SectionName { get; set; }

        [Parameter]
        public EventCallback<int> OnAddToTop15 { get; set; }

        [Parameter]
        public EventCallback<int> OnDeleteToTop15 { get; set; }

        [Parameter]
        public EventCallback<NewDTO> OnDeleteNews{ get; set; }
    }
}
