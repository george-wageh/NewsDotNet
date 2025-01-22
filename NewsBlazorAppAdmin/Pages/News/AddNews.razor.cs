using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using NewsBlazorAppAdmin.Services;
using NewsBlazorAppAdmin.Shared;
using SharedLib.DTO;
using System.Text.Json;

namespace NewsBlazorAppAdmin.Pages.News
{
    public partial class AddNews
    {
        public NewAddDTO newAddDTO { get; set; }
        public IEnumerable<SectionDTO> sectionDTOs { get; set; }
        [Inject]
        public NewsService NewsService { get; set; }

        [Inject]
        public SectionsService SectionsService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }
        IBrowserFile formFile { get; set; }
        protected async override Task OnInitializedAsync()
        {
            newAddDTO = new NewAddDTO();
            var response = await SectionsService.GetAllAsync();
            if (response.Success)
            {
                sectionDTOs = response.Data;
            }
        }

        public async Task OnValidSubmit() {
            newAddDTO.Image = Path.GetFileName(newAddDTO.Image);
            var response = await NewsService.AddAsync(newAddDTO);
            if (response?.Success == true) {
                NavigationManager.NavigateTo($"News/{response.Data}");
            }
        }
        public void OnChangeFile(InputFileChangeEventArgs inputFileChangeEventArgs) {
            formFile = inputFileChangeEventArgs.File;
        }
        public async Task UploadImage() {
            if (formFile != null && formFile.Size > 5 && formFile.ContentType == "image/jpeg")
            {
                var response = await NewsService.UploadImage(formFile);
                if (response?.Success == true)
                {
                    newAddDTO.Image = response.Data;
                }
            }
        }
    }
}
