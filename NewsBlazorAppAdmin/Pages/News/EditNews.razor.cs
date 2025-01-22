using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using NewsBlazorAppAdmin.Services;
using SharedLib.DTO;
using System.Text.Json;

namespace NewsBlazorAppAdmin.Pages.News
{
    public partial class EditNews
    {
        public NewAddDTO EditNewsDTO { get; set; }
        public IEnumerable<SectionDTO> sectionDTOs { get; set; }

        [Parameter]
        public int? Id { get; set; }

        [Inject]
        public NewsService NewsService { get; set; }

        [Inject]
        public SectionsService SectionsService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        IBrowserFile file { get; set; }
        protected async override Task OnInitializedAsync()
        {
            {
                var response = await NewsService.GetAsync(Id ?? 0);
                if (response?.Success == true)
                {
                    var NewDTO = response.Data;
                    EditNewsDTO = new NewAddDTO
                    {
                        Author = NewDTO.Author,
                        Description = NewDTO.Description,
                        Id = NewDTO.Id,
                        Image = NewDTO.Image,
                        SectionId = NewDTO.SectionId,
                        Title = NewDTO.Title,
                    };
                }
                else
                {
                    //error
                }

            }
            {
                var response = await SectionsService.GetAllAsync();
                if (response.Success)
                {
                    sectionDTOs = response.Data;
                }
            }

        }

        public async Task OnValidSubmit()
        {
            EditNewsDTO.Image = Path.GetFileName(EditNewsDTO.Image);
            var response = await NewsService.EditAsync(EditNewsDTO);
            if (response?.Success == true)
            {
                NavigationManager.NavigateTo($"News/{EditNewsDTO.Id}");
            }
            await Console.Out.WriteLineAsync(response.Success.ToString());
        }

        void OnChangeFile(InputFileChangeEventArgs inputFileChangeEventArgs) {
            file =inputFileChangeEventArgs.File;
        }

        async Task UploadImage() {
            if (file != null && file.Size > 5 && file.ContentType == "image/jpeg") {
                var response = await NewsService.UploadImage(file);
                if (response?.Success == true)
                {
                    EditNewsDTO.Image = response.Data;
                }
            }
        }
    }
}
