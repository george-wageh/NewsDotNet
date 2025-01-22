using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.TopNews
{
    public partial class Home
    {
        [Inject]
        Top15NewsService Top15NewsService { get; set; }

        IEnumerable<NewDTO> news { get; set; }
        public async Task ReloadNews() {

            var response = await Top15NewsService.GetAll();
            if (response?.Success == true)
            {
                news = response.Data;
                StateHasChanged();
            }
        }
        protected override async Task OnInitializedAsync()
        {
            await ReloadNews();
        }


        public async Task OnDeleteTop15(int id) {
            var response = await Top15NewsService.Delete(id);
            if (response?.Success == true)
            {
                await ReloadNews();
            }
        }

        public async Task OnUpNews(int id) {
            var response = await Top15NewsService.SetUp(id);
            if (response?.Success == true)
            {
                await ReloadNews();
            }
        }
        public async Task OnDownNews(int id)
        {
            var response = await Top15NewsService.SetDown(id);
            if (response?.Success == true)
            {
                await ReloadNews();
            }
        }
    }
}
