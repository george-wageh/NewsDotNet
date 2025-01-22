using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.News
{
    public partial class NewsDetails
    {
        [Parameter]
        public int Id { set; get; }
        [Inject]
        NewsService NewsService { get; set; }

        NewDetailsDTO NewDetailsDTO { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            //return base.OnParametersSetAsync();
            var response = await NewsService.GetAsync(Id);
            if (response?.Success == true) {
                NewDetailsDTO = response.Data;
            }
        }
    }
}
