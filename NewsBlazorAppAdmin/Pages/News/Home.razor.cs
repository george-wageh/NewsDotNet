using Microsoft.AspNetCore.Components;
using NewsBlazorAppAdmin.Services;
using SharedLib.AdminDTO;
using SharedLib.DTO;

namespace NewsBlazorAppAdmin.Pages.News
{
    public partial class Home
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public int? SectionId { get; set; } = 0;

        public IEnumerable<NewAdminDTO> News { get; set; }

        public NewDTO ModelToDelete { get; set; } = null;

        IEnumerable<SectionDTO> SectionsOptions { get; set; }

        public NewsQueryDTO NewsQuery { get; set; }
        int pagesCount = 1;
        string sectionName = "كل الفئات";
        [Inject]
        NewsService NewsService { get; set; }

        [Inject]
        SectionsService SectionsService { get; set; }

        [Inject]
        Top15NewsService Top15NewsService { get; set; }

        public async Task ReloadNewsAsync()
        {
            var response = await NewsService.Query(NewsQuery);
            if (response?.Success == true)
            {
                News = response.Data;
                pagesCount = response.PagesCount;
                sectionName = response.SectionName;
            }
        }

        public async Task LoadSections()
        {
            var response = await SectionsService.GetAllAsync();
            if (response?.Success == true)
            {
                SectionsOptions = response.Data;
            }
        }

        protected override void OnInitialized()
        {
            NewsQuery = new NewsQueryDTO()
            {
                SectionId = SectionId??0,
                PageNum = 1,
                QString = ""
            };

        }

        protected async override Task OnInitializedAsync()
        {

            await LoadSections();
            await ReloadNewsAsync();
            await Task.CompletedTask;
        }

        async Task Search()
        {
            await ReloadNewsAsync();
        }

        async Task ChangePage(int Index)
        {
            NewsQuery.PageNum = Index;
            await ReloadNewsAsync();
        }

        async Task OnAddToTop15(int id) {
            var response = await Top15NewsService.Push(id);
            if (response?.Success == true) {
                var newsTop = News.Where(x => x.Id == id).FirstOrDefault();
                if(newsTop!=null)
                    newsTop.IsInTopNews = true;
                StateHasChanged();
            }
        }

        async Task OnDeleteToTop15(int id)
        {
            var response = await Top15NewsService.Delete(id);
            if (response?.Success == true)
            {
                var newsTop = News.Where(x => x.Id == id).FirstOrDefault();
                if (newsTop != null)
                    newsTop.IsInTopNews = false;
                StateHasChanged();
            }
        }

        void OpenToDelete(NewDTO newDTO) {
            ModelToDelete = newDTO;
            StateHasChanged();
        }

        async Task OnDeleteModel(NewDTO newDTO) {
            if (newDTO != null) {
                var response = await NewsService.DeleteAsync(newDTO.Id);
                if (response?.Success == true)
                {
                    News = News.Where(x => x.Id != newDTO.Id).ToList();
                    StateHasChanged();
                }
            }
            ModelToDelete = null;
            StateHasChanged();

        }
    }
}
